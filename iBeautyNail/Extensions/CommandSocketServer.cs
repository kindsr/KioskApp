using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace iBeautyNail.Extensions
{
    public class CommandSocketServer
    {
        public event Action<string> ReceviedMessage;

        int port = 5000;
        TcpListener Listener;
        NetworkStream NS;
        StreamReader SR;
        StreamWriter SW;
        TcpClient client;

        System.Threading.Thread thServer;

        public void Start()
        {
            thServer = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                try
                {
                    Listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
                    Listener.Start(); // Listener 동작 시작

                    while (true)
                    {
                        client = Listener.AcceptTcpClient();

                        NS = client.GetStream(); // 소켓에서 메시지를 가져오는 스트림
                        SR = new StreamReader(NS, Encoding.UTF8); // Get message
                        SW = new StreamWriter(NS, Encoding.UTF8); // Send message

                        string message = string.Empty;
                        try
                        {
                            while (client.Connected == true) //클라이언트 메시지받기
                            {
                                message = SR.ReadLine();

                                if (string.IsNullOrEmpty(message))
                                {
                                    throw new Exception();
                                }
                                else
                                {
                                    Console.WriteLine("Log: {0} [{1}]", message, DateTime.Now);
                                    ReceviedMessage?.Invoke(message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SW.Close();
                            SR.Close();
                            client.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
                finally
                {
                    SW?.Close();
                    SR?.Close();
                    client?.Close();
                    NS?.Close();
                }
            }));

            thServer.Start();
        }

        public void Stop()
        {
            SW?.Close();
            SR?.Close();
            client?.Close();
            NS?.Close();
            Listener.Stop();

            thServer.Abort();
        }
    }
}