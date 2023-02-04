using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IoTSample
{
    internal class Program
    {
        // Replace this text with the IoT endpoint for your account. You can locate this bu going to 
        // your AWS account. Select IoT COre from the AWS Services menu. Scroll down to settings on the 
        // left hand nafigation panel. You will see the enbpoint listed under "Device data endpoint."
        private const string iotEndpoint = "_ENDPOINT_HERE_";
        private const int brokerPort = 8883;
        // Topic name that you use to publish and subscribe to messages
        private const string topic = "sdk/test/dotnet";
        private const string message = "Ping! ";

        private const string _clientId = "DotNetTestClient";

        private static MqttClient _client;

        // Replace this with the prefix that is part of the generated file name for your certificates.
        private const string thingName = "THING_NAME_HERE";

        static void Main(string[] args)
        {
            String BaseDirectory;
            X509Certificate2 clientCert;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                clientCert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificates/certificate.cert.pfx"), "Password1");
            }
            else
            {
                var certPem = File.ReadAllText(string.Format("Certificates/{0}-certificate.pem.crt", thingName));
                var eccPem = File.ReadAllText(string.Format("Certificates/{0}-private.pem.key", thingName));
                clientCert = X509Certificate2.CreateFromPem(certPem, eccPem);
            }
            var caCert = X509Certificate.CreateFromCertFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificates/AmazonRootCA1.pem"));
            
            _client = new MqttClient(iotEndpoint, brokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
            _client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            _client.ConnectionClosed += Client_ConnectionClosed;

            try
            {
                _client.Connect(_clientId);
                _client.Subscribe(new string[] { topic }, new byte[] { 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copnnecting... " + ex.ToString());
                return;
            }

            int i = 0;
            while (true)
            {
                try
                {
                    _client.Publish(topic, Encoding.UTF8.GetBytes($"{message} {i}"));
                    Console.WriteLine($"Published: {message} {i}");
                    i++;
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

        private static void Client_ConnectionClosed (object sender, EventArgs e)
        {
            // Reconnect the client.
            _client.Connect(_clientId);
            _client.Subscribe(new string[] { topic }, new byte[] { 0 });
        }

    }

    
}