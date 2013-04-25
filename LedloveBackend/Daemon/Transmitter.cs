using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace LedloveBackend.Daemon
{
    public class Transmitter
    {
        private static String server = "10.10.10.99";
        private static Int32 port = 10001;

        public static String checksum(String value)
        {
            char check = (char)0;
            for (int index = 0; index < value.Length; index++)
            {
                //int code = value[index].GetHashCode();
                //hashValue = hashValue ^ code;
                check = (char)(check ^ value[index]);
            }
            return String.Format("{0:X}", Convert.ToInt32(check));
        }

        public static bool send(String text) 
        {
            String msgcode = "<L1><PA><FE><MA><WC><FE>" + text;
            String fullmsg = "<ID00>" + msgcode + checksum(msgcode) + "<E>";
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(fullmsg);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", fullmsg);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();

                return true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return false;

        }
    }
}