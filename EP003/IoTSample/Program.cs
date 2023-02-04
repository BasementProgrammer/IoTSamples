using IoTSample.Models;
using IoTSample.Sensors;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using uPLibrary.Networking.M2Mqtt;

namespace IoTSample
{
    internal class Program
    {
        // AWS's IoT Endpoint. This is specific to the AWS Account.
        private const string iotEndpoint = "a1t7e1kccwk449-ats.iot.us-east-2.amazonaws.com";
        // The port that AWS IoT uses for MQTT over TLS.
        private const int brokerPort = 8883;
        // The topic that we are going to publish to.
        private const string topic = "sdk/test/dotnet";

        
        // The time period in ms that we generate outbound messages.
        private const int polingTime = 5000;


        private static IGpsSensor _gpsSensor = new MockGpsSensor();
        private static IWaterLevelSensor _waterLevelSensor = new MockWaterLevelSensor();


        static void Main(string[] args)
        {
            // Create the client certificate to authenticate with AWS IoT
            var caCert = X509Certificate.CreateFromCertFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificates/AmazonRootCA1.pem"));
            X509Certificate2 clientCert;
            var certPem = File.ReadAllText("Certificates/dotnet-thing-certificate.pem.crt");
            var eccPem = File.ReadAllText("Certificates/dotnet-thing-private.pem.key");
            clientCert = X509Certificate2.CreateFromPem(certPem, eccPem);

            // Create the client and connect to AWS IoT
            var client = new MqttClient(iotEndpoint, brokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);

            // Receive messages from the published topic.
            // Note you will nromally want to subscribe to a topic that is specific to your device.
            // however in this case we are subscribing to a single topic for Echo proposes.
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            // Create a unique client ID. This would normally be tied to the physical device.
            // however in this test application we are just going to have a random ID.
            string clientId = Guid.NewGuid().ToString();

            // Connect to the AWS IoT Core service, and if successful subscribe to the topic.
            try
            {
                client.Connect(clientId);
                client.Subscribe(new string[] { topic }, new byte[] { 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copnnecting... " + ex.ToString());
                return;
            }

            int messageCount = 0;

            // Simulate a sensor running constantly and publishing data back to the service.
            while (true)
            {
                try
                {
                    Models.WaterLevelMessage currentMessage = new Models.WaterLevelMessage (
                        clientId,
                        messageCount,
                        DateTime.Now,
                        _gpsSensor.GpsLocation.ToString(),
                        _waterLevelSensor.WaterLevel
                        );

                    string messageText = JsonSerializer.Serialize<Models.WaterLevelMessage>(currentMessage);
                    
                    client.Publish(topic, Encoding.UTF8.GetBytes(messageText));
                    Console.WriteLine($"Published: {messageText}");
                    messageCount++;
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Publishing data " + ex.ToString());
                    break;
                }
            }

            Console.WriteLine("Hello, World!");
        }

        private static void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach ( byte b in e.Message)
                {
                sb.Append(Convert.ToChar( b));
            }

            string message = sb.ToString();
            Console.WriteLine("Message Received: " + message);
        }
    }
}