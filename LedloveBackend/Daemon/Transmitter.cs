using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace LedloveBackend.Daemon
{
    public class Transmitter
    {
        private String server = "10.10.10.99";
        private Int32 port = 10001;

        public Transmitter() { }

        public Transmitter(String remoteServer, Int32 remotePort) {
            server = remoteServer;
            port = remotePort;
        }

        private static String Checksum(String value)
        {
            char check = (char)0;
            for (int index = 0; index < value.Length; index++)
            {
                check = (char)(check ^ value[index]);
            }
            return String.Format("{0:X}", Convert.ToInt32(check));
        }

        public String Send(String text) 
        {
            String msgcode = "<L1><PA><FE><MA><WC><FE>" + text;
            String fullmsg = "<ID00>" + msgcode + Checksum(msgcode) + "<E>";
            String responseData = null;
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                using (TcpClient client = new TcpClient(server, port))
                {

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(fullmsg);

                    // Get a client stream for reading and writing.
                    NetworkStream stream = client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    //Console.WriteLine("Sent: {0}", fullmsg);

                    // Read the first batch of the TcpServer response bytes.
                    // Instead of blocking, use async sockets: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
                    //data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    // Close everything.
                    stream.Close();
                    client.Close();

                    return responseData;
                }
            } catch (ArgumentNullException e)
            {
                return String.Format("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                return String.Format("SocketException: {0}", e);
            }
        }
    }
}