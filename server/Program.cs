﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TEC.SimpleServer
{
    public class Program
    {
        private static System.Net.Sockets.TcpListener listener;

        public class Cords
        {
            public string X {get;set;} = "";
            public string Y {get;set;} = "";
        }

        private static Dictionary<string, Cords> allCords = new Dictionary<string, Cords>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Program started.");
            bool isRunning = true;

            Console.CancelKeyPress += (sender, eArgs) =>
            {
                listener.Stop(); isRunning = false;
                eArgs.Cancel = true;
            };

            var addr = IPAddress.Parse("127.0.0.1");
            listener = new System.Net.Sockets.TcpListener(addr, 1234);
            listener.Start();

            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client " + client.Client.RemoteEndPoint + " connected.");
                NetworkStream nStream = client.GetStream();
                List<byte> bytes = new List<byte>();
                if (nStream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    do
                    {
                        try
                        {
                            int count = nStream.Read(readBuffer, 0, readBuffer.Length);
                            byte[] data = new byte[count];
                            for (int i = 0; i < count; i++)
                            {
                                data[i] = readBuffer[i];
                            }
                            bytes.AddRange(data);
                        }
                        catch (Exception ex)
                        {
                            Console.Write("[ERROR]: " + ex.ToString());
                        }
                        System.Threading.Thread.Sleep(1);
                    }
                    while (nStream.DataAvailable);
                }

                string received_string = System.Text.Encoding.UTF8.GetString(bytes.ToArray());

                Console.Write(received_string); Console.Write(System.Environment.NewLine);
                Console.WriteLine("--received " + received_string.Length + " bytes");
                //Console.WriteLine($"--received {received_string.Length} bytes");

                byte[] response_bytes = GetResponse(received_string);
                if (nStream.CanWrite)
                {
                    try
                    {
                        nStream.Write(response_bytes, 0, response_bytes.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.Write("[ERROR]: " + ex.ToString());
                    }
                }
                nStream.Close();
                client.Close();

                System.Threading.Thread.Sleep(10);
            }

            listener.Stop();
            Console.WriteLine("Program terminated.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Что-то типа "Nick-10-15"</param>
        /// <returns></returns>
        public static byte[] GetResponse(string request)
        {
            string[] rData = request.Split('-');
            
            if (allCords.ContainsKey(rData[0]))
            {
                allCords[rData[0]] = new Cords() {X = rData[1], Y = rData[2] };
            }
            else
            {
                allCords.Add(rData[0], new Cords() {X = rData[1], Y = rData[2] });
            }

            string[] allCordsKeys = new string[allCords.Keys.Count];
            allCords.Keys.CopyTo(allCordsKeys, 0);

            string resultStr = string.Empty;
            for (int i = 0; i < allCordsKeys.Length; i++)
            {
                string curUser = $"{allCordsKeys[i]}-{allCords[allCordsKeys[i]].X}-{allCords[allCordsKeys[i]].Y}";
                if (resultStr != string.Empty)
                {
                    resultStr += "_";
                }
                resultStr += curUser;
            }

            byte[] response_bytes = System.Text.Encoding.UTF8.GetBytes(resultStr);
            
            return response_bytes;
        }
    }
}