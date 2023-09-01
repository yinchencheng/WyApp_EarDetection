using HalconDotNet;
using HslCommunication.Profinet.Keyence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BunifuTextBox.Transitions;
using Utilities.BunifuToolTip.Transitions;

namespace WY_App.Utility
{
    public class Parameter
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public enum LogLevelEnum
        {
            Debug = 0,
            Info = 1,
            Warn = 2,
            Error = 3,
            Fatal = 4
        }
        public class CameraParam
        {
            public string[] CameraId = new string[4];
            public double[] Gain = new double[4];
            public double[] Shutter = new double[4];
            public double[] Black_Level = new double[4];
            public double[] Gamma = new double[4];

            public CameraParam()
            {
                for (int i = 0; i < 4; i++)
                {
                    CameraId[i] = "Cam" + i;
                    Gain[i] = 10;
                    Shutter[i] = 10;
                    Black_Level[i] = 0;
                    Gamma[i] = 0;
                }
            }
        }
        public static CameraParam cameraParam = new CameraParam();
        public struct Rect1
        {
            public double Row1;
            
            public double Colum1;

            public double Row2;

            public double Colum2;

            public double Length1;

            public double Length2;

            public double 阈值;

            public string 极性;

            public double simga;

            public int Index;

            public double LineLength;
        }
        public struct HRect1
        {
            public HTuple Row1;

            public HTuple Colum1;

            public HTuple Row2;

            public HTuple Colum2;

            public HTuple RectLength1;

            public HTuple RectLength2;

            public HTuple 阈值;

            public string 极性;

            public HTuple simga;
        }

      
        public struct Rect2
        {
            public double Row;

            public double Colum;

            public double Phi;

            public double Length1;

            public double Length2;

            public double 阈值;

            public string 极性;

            public double simga;
        }
        public struct Spec
        {
            public double value;

            public double min;

            public double max;

            public double adjust;
        }


        public class SpecificationsCam1
        {
            public Rect1 矩形检测区域;
            public Rect1[] 模板区域 = new Rect1[9];
            public Rect1[] 基准 = new Rect1[3];

            public Spec 肩宽; //胶高=肩宽

            public Spec 肩高;

            public Spec 胶线;

            public double BaseRow;

            public double BaseColumn;

            public bool TrianEnabled;

            public double PixelResolution;

            public bool SaveOrigalImage;

            public bool SaveDefeatImage;


            public double ThresholdLow;

            public double ThresholdHigh;

            public double AreaLow;

            public double AreaHigh;


            public double DeepLearningRate;


            public SpecificationsCam1()
            {
                DeepLearningRate = 0.1;
            }
        }
        public class SpecificationsCam2
        {
            public Rect1[] 矩形检测区域 = new Rect1[10];
            public Rect1[] 模板区域 = new Rect1[20];
            public Rect1[] 基准 = new Rect1[3];

            public Spec[] 检测规格 =new Spec[10];

            public bool[] 倒角检测 = new bool[4];


            public double BaseRow;

            public double BaseColumn;

            public double PixelResolution;

            public bool SaveOrigalImage;

            public bool SaveDefeatImage;

            public double[] ThresholdLow = new double[10];

            public double[] ThresholdHigh = new double[10];

            public double[] AreaLow = new double[10];

            public double[] AreaHigh = new double[10];

            public double DeepLearningRate;


            public SpecificationsCam2()
            {
                DeepLearningRate = 0.1;
                for (int i = 0; i < 10; i++)
                {
                    ThresholdLow[i] = 10;
                    ThresholdHigh[i] = 100;
                    AreaLow[i] = 10;
                    AreaHigh[i] = 1000;
                   
                }
            }
        }

        public struct rent1
        {
            public double Column1;
            public double Row1;
            public double Column2;
            public double Row2;
            public double sigma;
            public double threshold;
            public string polarity;
            public double x_shift;
            public double y_shift;
        }

        public struct rent2
        {
            public double Column;
            public double Row;
            public double Phi;
            public double Length1;
            public double Length2;
            public double Bettween;
            public double Measure_num;
            public double sigma;
            public double threshold;
            public string polarity;
            public double x_shift;
            public double y_shift;
        }

        public class Rent2
        {
            public rent2 rent2;

            public Rent2()
            {
                rent2.Column = 100;
                rent2.Row = 100;
                rent2.Phi = 0;
                rent2.Length1 = 10;
                rent2.Length2 = 20;
                rent2.Bettween = 93;
                rent2.Measure_num = 10;
                rent2.sigma = 1;
                rent2.threshold = 20;
                rent2.polarity = "all";
                rent2.x_shift = 0;
                rent2.y_shift = 0;


            }
        }

        public static SpecificationsCam1[] specificationsCam1 = new SpecificationsCam1[2];
        public static SpecificationsCam2[] specificationsCam2 = new SpecificationsCam2[2];
        public class Commministion
        {
            /// <summary>
            /// 当前保存日志等级
            /// </summary>
            public LogLevelEnum LogLevel;

            /// <summary>
            /// 日志存放路径
            /// </summary>
            public string LogFilePath;

            /// <summary>
            /// 日志存放天数
            /// </summary>
            public int LogFileExistDay;

            /// <summary>
            /// plc启用标志
            /// </summary>
            public bool PlcEnable;

            /// <summary>
            /// plc型号
            /// </summary>
            public string PlcType;

            /// <summary>
            /// plc ip地址
            /// </summary>
            public string PlcIpAddress;

            /// <summary>
            /// plc ip端口号
            /// </summary>
            public int PlcIpPort;

            /// <summary>
            /// plc 站号/型号
            /// </summary>
            public string PlcDevice;

            /// <summary>
            /// Tcp客户端启用标志
            /// </summary>
            public bool TcpClientEnable;

            /// <summary>
            /// tcp ip地址
            /// </summary>
            public string TcpClientIpAddress;

            /// <summary>
            /// tcp ip端口号
            /// </summary>
            public int TcpClientIpPort;

            /// <summary>
            /// Tcp服务端启用标志
            /// </summary>
            public bool TcpServerEnable;

            /// <summary>
            /// tcp服务器 ip地址
            /// </summary>
            public string TcpServerIpAddress;

            /// <summary>
            /// tcp服务器 ip端口号
            /// </summary>
            public int TcpServerIpPort;
            /// <summary>
            /// path
            /// </summary>
            public string ImagePath;

            /// <summary>
            /// path
            /// </summary>
            public string ImageSavePath;
            /// <summary>
            /// path
            /// </summary>
            public string productName;

            public string DeviceID;
            /// <summary>
            /// 联机参数设置
            /// </summary>
            public Commministion()
            {
                //PLC启用标志
                PlcEnable = false;
                //--PLC参数设置--
                //--欧姆龙Omron.OmronFinsNet--
                //--西门子Siemens.SiemensS7Net--
                //--三菱Melsec.MelsecMcNet--
                //--汇川Inovance.InovanceSerialOverTcp--
                //--ModbusTcp

                //PLC 类型
                PlcType = "Omron.OmronFinsNet";
                //PLC 地址
                PlcIpAddress = "127.0.0.1";
                //PLC站号/型号
                PlcDevice = "200";
                //PLC 端口号
                PlcIpPort = 9600;

                //--TCP客户端参数设置--
                //Tcp客户端启用标志
                TcpClientEnable = true;
                //TCP 客户端 地址
                TcpClientIpAddress = "127.0.0.1";
                //TCP 客户端 端口号
                TcpClientIpPort = 9600;

                //--TCP服务器参数设置--
                //Tcp服务器启用标志
                TcpServerEnable = false;
                //TCP服务器 地址
                TcpServerIpAddress = "127.0.0.1";
                //TCP服务器 端口号
                TcpServerIpPort = 9600;

                ImagePath = @"D:\VisionDetect\InspectImage\";
                ImageSavePath = @"D:\Image";
                DeviceID = "";
                productName = "初始化";
            }
        }

        public static Commministion commministion = new Commministion();

        public class DeviceName
        {
            public string DeviceID;
            public DeviceName()
            {
                DeviceID = "";
            }
        }
        public static DeviceName deviceName = new DeviceName();

        public class PLCParams
        {
            public string Trigger_Detection0;
            public string Trigger_Detection1;
            public string Completion1;
            public string Trigger_Detection2;
            public string Completion0;
            public string Completion2;
            public string StartAdd;
            public string HeartBeatAdd;

            public PLCParams()
            {
                Trigger_Detection0 = "D800";
                Trigger_Detection1 = "D100";
                Completion1 = "D102";
                StartAdd = "D104";
                Trigger_Detection1 = "D106";
                Completion0 = "D110";
                Completion1 = "D108";
                HeartBeatAdd = "";
            }
        }

        public class Counts
        {
            public int[] Counts1 = new int[5];
            public int[] Counts2 = new int[5];
            public int[] Counts3 = new int[6];
            public int[] Counts4 = new int[6];

            public Counts()
            {
              
                
            }
        }

        public static PLCParams plcParams = new PLCParams();
        public static Counts counts = new Counts();
    }
}