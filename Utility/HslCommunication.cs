using System;
using System.Threading;
using HslCommunication;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Siemens;
using HslCommunication.Profinet.Inovance;
using WY_App.Utility;
using HslCommunication.Core;
using System.IO;

using HslCommunication.ModBus;
//power pmac配置
using ODT.PowerPmacComLib;
using ODT.Common.Services;
using ODT.Common.Core;
using System.Windows.Forms;
using System.Collections.Generic;
using static WY_App.Utility.Parameter;
using static OpenCvSharp.FileStorage;

namespace WY_App
{
    internal class HslCommunication
    {
        public static OperateResult _connected ;
        public static NetworkDeviceBase _NetworkTcpDevice;
        static ISyncGpasciiCommunicationInterface communication = null;
        deviceProperties currentDeviceProp = new deviceProperties();
        deviceProperties currentDevProp = new deviceProperties();
        string commands = String.Empty;
        public static string response = String.Empty;

        public static bool plc_connect_result = false;


        public HslCommunication()
        {
            try
            {
                Parameter.plcParams = XMLHelper.BackSerialize<Parameter.PLCParams>("Parameter/PLCParams.xml");
            }
            catch
            {
                Parameter.plcParams = new Parameter.PLCParams();
                XMLHelper.serialize<Parameter.PLCParams>(Parameter.plcParams, "Parameter/PLCParams.xml");
            }
           
            Thread th = new Thread(ini_PLC_Connect);
            th.IsBackground = true;
            th.Start();

            Thread PLC_Read = new Thread(ini_PLC_Read);
            PLC_Read.IsBackground = true;
            PLC_Read.Start();
        }
        public void ini_PLC_Connect()
        {
            if (!Authorization.SetAuthorizationCode("f562cc4c-4772-4b32-bdcd-f3e122c534e3"))
            {
                LogHelper.Log.WriteError("HslCommunication 组件认证失败，组件只能使用8小时!");
                MainForm.AlarmList.Add("HslCommunication 组件认证失败，组件只能使用8小时!");
            }           
            while (!plc_connect_result)
            {
                Thread.Sleep(3000);
                try
                {
                    //欧姆龙PLC Omron.PMAC.CK3M通讯
                    if ("Omron.PMAC.CK3M".Equals(Parameter.commministion.PlcType))
                    {
                        currentDevProp.IPAddress = Parameter.commministion.PlcIpAddress;
                        currentDevProp.Password = "deltatau";
                        currentDevProp.PortNumber = Parameter.commministion.PlcIpPort;
                        currentDevProp.User = "root";
                        currentDevProp.Protocol = CommunicationGlobals.ConnectionTypes.SSH;
                        communication = Connect.CreateSyncGpascii(currentDevProp.Protocol, communication);
                        plc_connect_result = communication.ConnectGpAscii(currentDevProp.IPAddress, currentDevProp.PortNumber, currentDevProp.User, currentDevProp.Password);                                              
                    }
                    //欧姆龙PLC OmronFinsNet通讯
                    else if ("Omron.OmronFinsNet".Equals(Parameter.commministion.PlcType))
                    {
                        OmronFinsNet Client = new OmronFinsNet();
                        Client.IpAddress = Parameter.commministion.PlcIpAddress;
                        Client.Port = Parameter.commministion.PlcIpPort;
                        Client.SA1 = Convert.ToByte(Parameter.commministion.PlcDevice);
                        _NetworkTcpDevice = Client;
                        _connected = _NetworkTcpDevice.ConnectServer();
                        plc_connect_result = _connected.IsSuccess;
                    }
                    //三菱PLC Melsec.MelsecMcNet通讯
                    else if ("Melsec.MelsecMcNet".Equals(Parameter.commministion.PlcType))
                    {
                        MelsecMcNet Client = new MelsecMcNet();
                        Client.IpAddress = Parameter.commministion.PlcIpAddress;
                        Client.Port = Parameter.commministion.PlcIpPort;
                        _NetworkTcpDevice = Client;
                        _connected = _NetworkTcpDevice.ConnectServer();
                        plc_connect_result = _connected.IsSuccess;
                    }
                  
                    //西门子PLC Siemens.SiemensS7Net通讯
                    else if ("Siemens.SiemensS7Net".Equals(Parameter.commministion.PlcType))
                    {
                        SiemensS7Net Client = new SiemensS7Net((SiemensPLCS)Convert.ToInt16(Parameter.commministion.PlcDevice), Parameter.commministion.PlcIpAddress);
                        Client.IpAddress = Parameter.commministion.PlcIpAddress;
                        Client.Port = Parameter.commministion.PlcIpPort;
                        _NetworkTcpDevice = Client;
                        _connected = _NetworkTcpDevice.ConnectServer();
                        plc_connect_result = _connected.IsSuccess;
                    }
                    //汇川PLC Inovance.InovanceSerialOverTcp通讯
                    else if ("Inovance.InovanceSerialOverTcp".Equals(Parameter.commministion.PlcType))
                    {
                        InovanceSerialOverTcp Client = new InovanceSerialOverTcp();
                        Client.IpAddress = Parameter.commministion.PlcIpAddress;
                        Client.Port = Parameter.commministion.PlcIpPort;
                        Client.DataFormat = DataFormat.ABCD;
                        _NetworkTcpDevice = Client;
                        _connected = _NetworkTcpDevice.ConnectServer();
                        plc_connect_result = _connected.IsSuccess;
                    }
                    //ModbusTcp通讯
                    else if ("ModbusTcpNet".Equals(Parameter.commministion.PlcType))
                    {
                        ModbusTcpNet Client = new ModbusTcpNet();
                        Client.IpAddress = Parameter.commministion.PlcIpAddress;
                        Client.Port = Parameter.commministion.PlcIpPort;
                        Client.Station = 0x01;
                        _NetworkTcpDevice = Client;
                        _connected = _NetworkTcpDevice.ConnectServer();
                        plc_connect_result = _connected.IsSuccess;
                    }
                    //新增通讯添加else if判断创建连接

                    //Parameter.PlcType字符错误或未定义
                    else
                    {
                        LogHelper.Log.WriteError(Parameter.commministion.PlcType + "类型未定义!!!");
                        MainForm.AlarmList.Add(Parameter.commministion.PlcType + "类型未定义!!!");
                        plc_connect_result = false;
                    }
                   
                    if (plc_connect_result)
                    {
                        LogHelper.Log.WriteInfo(Parameter.commministion.PlcType + "连接成功:IP" + Parameter.commministion.PlcIpAddress + "  Port:" + Parameter.commministion.PlcIpPort);
                        MainForm.AlarmList.Add(Parameter.commministion.PlcType + "连接成功:IP" + Parameter.commministion.PlcIpAddress + "  Port:" + Parameter.commministion.PlcIpPort);
                        plc_connect_result = true;
                    }
                    else
                    {
                        LogHelper.Log.WriteError(Parameter.commministion.PlcType + "连接失败:IP" + Parameter.commministion.PlcIpAddress + "  Port:" + Parameter.commministion.PlcIpPort);
                        MainForm.AlarmList.Add(Parameter.commministion.PlcType + "连接失败:IP" + Parameter.commministion.PlcIpAddress + "  Port:" + Parameter.commministion.PlcIpPort);
                        plc_connect_result = false;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log.WriteError(Parameter.commministion.PlcType + "初始化失败:", ex.Message);
                    MainForm.AlarmList.Add(Parameter.commministion.PlcType + "初始化失败:"+ ex.Message);
                    plc_connect_result = false;
                }
            }
            while (plc_connect_result)
            {
                ////心跳读写，判断PLC是否掉线，不建议线程对plc链接释放重连
                if ("Omron.PMAC.CK3M".Equals(Parameter.commministion.PlcType))
                {
                    plc_connect_result = ReadWritePmacVariables(Parameter.plcParams.HeartBeatAdd);
                    Thread.Sleep(3000);
                }
                else
                {
                    try
                    {
                        if(Parameter.plcParams.HeartBeatAdd!=null|| Parameter.plcParams.HeartBeatAdd!="")
                        {
                            _connected = _NetworkTcpDevice.Write(Parameter.plcParams.HeartBeatAdd, 1);
                            Thread.Sleep(1000);
                            _connected = _NetworkTcpDevice.Write(Parameter.plcParams.HeartBeatAdd, 0);
                            Thread.Sleep(1000);
                            plc_connect_result = true;
                        }
                        else
                        {
                            Thread.Sleep(100000);
                        }
                        
                    }
                    catch
                    {
                        _NetworkTcpDevice.Dispose();
                        _connected.IsSuccess = false;
                        plc_connect_result = false;                        
                    }
                }
            }             
        }
        public void ini_PLC_Read()
        { 
            while(true)
            {
                if(plc_connect_result)
                {
                    if ("Omron.PMAC.CK3M".Equals(Parameter.commministion.PlcType))
                    {

                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                  
                }                
            }
        }


        //对终端操作的通用方法/
        public static bool ReadWritePmacVariables(string command)
        {
            var commads = new List<string>();
            List<string> responses;
            commads.Add(command.ToString());
            var communicationStatus = communication.GetResponse(commads, out responses, 3);
            if (communicationStatus == Status.Ok)
            {
                response = string.Join("", responses.ToArray());
                command = null;
                return  true;
            }
            else
            {
                return  false;
            }
        }


        public static  double plc_Readdouble(string ReadAddress)
        {
            return -1;
        }
        public static void plc_WriteDouble()
        {

        }

        public static void plc_WriteBool()
        {
           
        }
    }
}
