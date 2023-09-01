using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WY_App.Utility
{
    internal class TcpClient
    {
        static Socket socketSend;
        static string reciveStr;
        public static bool TcpClientConnectResult = false;
        public TcpClient()
        {
            Thread th = new Thread(ini_Tcp_Client);
            th.IsBackground = true;
            th.Start();
        }
        public static void ini_Tcp_Client()
        {
            while(!TcpClientConnectResult)
            {
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(Parameter.commministion.TcpClientIpAddress);
                IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(Parameter.commministion.TcpClientIpPort));
                socketSend.SendTimeout = 1000;
                socketSend.ReceiveTimeout = 3000;
                try
                {
                    socketSend.Connect(point);
                    LogHelper.Log.WriteInfo("TcpClientIP:" + Parameter.commministion.TcpClientIpAddress +"Port:"+ Parameter.commministion.TcpClientIpPort+ "链接成功");
                    string str = TcpClient.tcpClientSend("Tcp客户端接入");
                    MainForm.AlarmList.Add("TcpClientIP:" + Parameter.commministion.TcpClientIpAddress + "Port:" + Parameter.commministion.TcpClientIpPort + "链接成功");
                    TcpClientConnectResult = true;
                    Thread th = new Thread(ReciveMessagr);
                    th.IsBackground = true;
                    th.Start();

                }
                catch (Exception ex)
                {
                    LogHelper.Log.WriteError("TcpClientIP:" + Parameter.commministion.TcpClientIpAddress + "Port:" + Parameter.commministion.TcpClientIpPort +"链接失败:", ex.Message);
                    MainForm.AlarmList.Add("TcpClientIP:" + Parameter.commministion.TcpClientIpAddress + "Port:" + Parameter.commministion.TcpClientIpPort + "链接失败:" + ex.Message);
                    TcpClientConnectResult = false;
                }
            }          
        }

        public static string tcpClientSend(string sendstr)
        {
            //Task.Run(() => { });
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sendstr);
            socketSend.Send(buffer);
            LogHelper.Log.WriteInfo(socketSend.RemoteEndPoint + "发送数据:" + sendstr);
            MainForm.AlarmList.Add(socketSend.RemoteEndPoint + "发送数据:" + sendstr);
            reciveStr = Recive();
            LogHelper.Log.WriteInfo(socketSend.RemoteEndPoint + "接收数据:" + reciveStr);
            MainForm.AlarmList.Add(socketSend.RemoteEndPoint + "接收数据:" + reciveStr);
            return reciveStr;

        }
        static string Recive()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024 * 3];
                int r = socketSend.Receive(buffer);
                if (r == 0)
                {
                    reciveStr = "";
                }
                return Encoding.UTF8.GetString(buffer, 0, r);
            }
            catch 
            {
                return "检测超时";
            }
        }

        static void ReciveMessagr()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    int r = socketSend.Receive(buffer);

                    if (r == 0)
                    {
                        break;
                    }
                    string s = Encoding.UTF8.GetString(buffer, 0, r);
                    
                    MainForm.AlarmList.Add(socketSend.RemoteEndPoint + "接收数据:" + s);
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }
    }
}
