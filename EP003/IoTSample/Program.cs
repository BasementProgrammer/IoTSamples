using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IoTSample.Models;
using IoTSample.Sensors;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

namespace IoTSample
{
    internal class Program
    {
        // Replace this text with the IoT endpoint for your account. You can locate this bu going to 
        // your AWS account. Select IoT COre from the AWS Services menu. Scroll down to settings on the 
        // left hand navigation panel. You will see the endpoint listed under "Device data endpoint."
        private const string IotEndpoint = "_ENDPOINT_HERE_";
        private const string ThingName = "_THING_NAME_HERE_";
        private const string ClientId = "sdk-dotnet";
        
        private const int BrokerPort = 8883;
        private const string Topic = "sdk/test/dotnet";
        private static MqttClient? _client;

        // Mock Sensors for the project
        private static IGpsSensor _gpsSensor = new MockGpsSensor();
        private static IWaterLevelSensor _waterLevelSensor = new MockWaterLevelSensor();
        
        static void Main(string[] args)
        {
            var caCert = X509Certificate.CreateFromCertFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificates/root-CA.crt"));
            var certPem = File.ReadAllText(string.Format("Certificates/{0}.cert.pem", ThingName));
            var eccPem = File.ReadAllText(string.Format("Certificates/{0}.private.key", ThingName));
            var clientCert = X509Certificate2.CreateFromPem(certPem, eccPem);
            // Set up the cline to connect to AWS
            _client = new MqttClient(IotEndpoint, BrokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
            _client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            _client.ConnectionClosed += Client_ConnectionClosed;

            try
            {
                // Try connecting to AWS IoT Core
                _client.Connect(ClientId);
                // Subscribe to the topics to receive echo messages.
                // You would not normally do this in a client, we are just doing it here for testing.
                _client.Subscribe(new string[] { Topic }, new byte[] { 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting... " + ex.ToString());
                return;
            }

            var messageCount = 0;
            while (true)
            {
                try
                {
                    // Get current values from the sensors.
                    var waterLevel = _waterLevelSensor.WaterLevel;
                    var gpsLocation = _gpsSensor.GpsLocation;
                    // Format a message for the current readings.
                    var waterLevelMessage =
                        new WaterLevelMessage(ThingName, messageCount, DateTime.Now, gpsLocation.ToString(), waterLevel);
                    
                    // Serialize WaterLevelMessage to JSON string and publish to MQTT broker
                    var serializedMessage = JsonConvert.SerializeObject(waterLevelMessage);
                    _client.Publish(Topic, Encoding.UTF8.GetBytes($"{serializedMessage}"));
                    Console.WriteLine($"Published: {serializedMessage} ");
                    // Sleep the current thread for 5 seconds.
                    // You would normally have a longer gap between readings, but 5 seconds is good for testing.
                    Thread.Sleep(5000);
                    messageCount++;
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
            var sb = new StringBuilder();
            foreach ( var b in e.Message)
            {
                sb.Append(Convert.ToChar( b));
            }

            var message = sb.ToString();
            Console.WriteLine("Message Received: " + message);
        }

        private static void Client_ConnectionClosed (object sender, EventArgs e)
        {
            // Reconnect the client.
            _client?.Connect(ThingName);
            _client?.Subscribe(new string[] { Topic }, new byte[] { 0 });
        }

    }

    
}