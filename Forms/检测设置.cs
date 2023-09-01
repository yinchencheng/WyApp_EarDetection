using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.ObjectModel;
using WY_App.Utility;
using OpenCvSharp.XImgProc;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using HalconDotNet;
using System.Runtime.CompilerServices;
using Sunny.UI;
using OpenCvSharp.Flann;
using System.Numerics;
using OpenCvSharp.Internal.Vectors;
using Sunny.UI.Win32;
using System.Security.Cryptography;
using static WY_App.Utility.Parameter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using SevenZip.Compression.LZ;

namespace WY_App
{
    public partial class 检测设置 : Form
    {
        public static int Round_Edge = 0;
        System.Drawing.Point downPoint;
        public 检测设置()
        {
            InitializeComponent();          
        }
        
        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + e.X - downPoint.X,
                    this.Location.Y + e.Y - downPoint.Y);
            }
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new System.Drawing.Point(e.X, e.Y);
        }


        private void btn_Close_System_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btn_Close_System_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        /// <summary>
        /// 霍夫变换-直线
        /// </summary>
        /// <param name="imagePath"></param>
        private static void HoughtLine(Mat MatImage, PictureBox picture0)
        {
            
            using (Mat dst = new Mat(MatImage.Size(), MatType.CV_8UC3, Scalar.Blue))
            {
                // 1:边缘检测
                Mat canyy = new Mat(MatImage.Size(), MatImage.Type());
                Cv2.Canny(MatImage, canyy, 60, 200, 3, false);

                /*
                 *  HoughLinesP:使用概率霍夫变换查找二进制图像中的线段。
                 *  参数：
                 *      1； image: 输入图像 （只能输入单通道图像）
                 *      2； rho:   累加器的距离分辨率(以像素为单位) 生成极坐标时候的像素扫描步长
                 *      3； theta: 累加器的角度分辨率(以弧度为单位)生成极坐标时候的角度步长，一般取值CV_PI/180 ==1度
                 *      4； threshold: 累加器阈值参数。只有那些足够的行才会返回 投票(>阈值)；设置认为几个像素连载一起                     才能被看做是直线。
                 *      5； minLineLength: 最小线长度，设置最小线段是有几个像素组成。
                 *      6；maxLineGap: 同一条线上的点之间连接它们的最大允许间隙。(默认情况下是0）：设置你认为像素之间                     间隔多少个间隙也能认为是直线
                 *      返回结果:
                 *      输出线。每条线由一个4元向量(x1, y1, x2，y2)
                 */
                LineSegmentPoint[] linePiont = Cv2.HoughLinesP(canyy, 1, 1, 1, 5, 10);//只能输入单通道图像
                Scalar color = new Scalar(0, 255, 255);
                for (int i = 0; i < linePiont.Count(); i++)
                {
                    OpenCvSharp.Point p1 = linePiont[i].P1;
                    OpenCvSharp.Point p2 = linePiont[i].P2;
                    Cv2.Line(dst, p1, p2, color, 4, LineTypes.Link8);
                }

                picture0.Image = dst.ToBitmap();
                Cv2.WaitKey(0);
                
            }
        }



        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_加载检测图片_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK && (openfile.FileName != "")) 
            {
                //picture0.ImageLocation = openfile.FileName;
                //MatImage = Cv2.ImRead(openfile.FileName);
                Halcon.ImgDisplay(0,openfile.FileName, hWindowControl1.HalconWindow);;
            }
            openfile.Dispose();
        }

        private void btn_绘制检测区域_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawRectAOI(hWindowControl1.HalconWindow,主窗体.hImage[0], ref Parameter.specifications.矩形检测区域N1);
        }

        

        

        private void btn_SaveParams_Click(object sender, EventArgs e)
        {
            XMLHelper.serialize<Parameter.Specifications>(Parameter.specifications, "Parameter/Specifications.xml");
        }

        private void chk_Cricle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void 检测设置_Load(object sender, EventArgs e)
        {
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(主窗体.hImage[0], hWindowControl1.HalconWindow);
            num_AreaHigh.Value = Parameter.specifications.AreaHigh;
            num_AreaLow.Value = Parameter.specifications.AreaLow;
            num_ThresholdHigh.Value = Parameter.specifications.ThresholdHigh;
            num_ThresholdLow.Value = Parameter.specifications.ThresholdLow;

            num_PixelResolution.Value=Parameter.specifications.PixelResolution;

            chk_SaveOrigalImage.Checked = Parameter.specifications.SaveOrigalImage;

            chk_SaveDefeatImage.Checked = Parameter.specifications.SaveDefeatImage;

            uiDoubleUpDown1.Value = Parameter.specifications.DeepLearningRateN1;
            uiDoubleUpDown2.Value = Parameter.specifications.DeepLearningRateN2;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
            Halcon.DetectionHalconReadDlModel(Application.StartupPath + "/halcon/N1.hdl", out hv_DLModelHandle, out hv_DLPreprocessParam, out hv_InferenceClassificationThreshold, out hv_InferenceSegmentationThreshold);

            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
        }
        public static void defectionN1(HWindow hWindow,HObject hImage, ref bool[] result,ref double[] value)
        {
            try
            {
                bool[] resultvalue = new bool[8];
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                HOperatorSet.DispObj(hImage, hWindow);
                Halcon.DetectionHalconLine(hWindow, hImage, Parameter.specifications.基准N1[0], 200, ref BaseReaultN1[0]);
                Halcon.DetectionHalconLine(hWindow, hImage, Parameter.specifications.基准N1[1], 200, ref BaseReaultN1[1]);

                HTuple angle, Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReaultN1[0].Row1, BaseReaultN1[0].Colum1, BaseReaultN1[0].Row2, BaseReaultN1[0].Colum2,
                    BaseReaultN1[1].Row1, BaseReaultN1[1].Colum1, BaseReaultN1[1].Row2, BaseReaultN1[1].Colum2, out Row, out Column, out IsOverlapping);

                HOperatorSet.AngleLx(BaseReaultN1[1].Row1, BaseReaultN1[1].Colum1, BaseReaultN1[1].Row2, BaseReaultN1[1].Colum2, out angle);
                HTuple HomMat2DIdentity;
                HTuple HomMat2DRotate;
                HObject ImageAffineTran;
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, - angle, Row, Column, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(hImage, out ImageAffineTran, HomMat2DRotate, "constant", "false");

                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();

                HObject ImageAffineTrans;
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, - Row + Parameter.specifications.BaseRowN1, - Column + Parameter.specifications.BaseColumnN1, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageAffineTran, out ImageAffineTrans, HomMat2DRotate, "constant", "false");

                HOperatorSet.DispObj(ImageAffineTrans, hWindow);
                resultvalue[0] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[0], 100, ref pointReaultN1[0]);
                resultvalue[1] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[1], 100, ref pointReaultN1[1]);
                resultvalue[2] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[2], 100, ref pointReaultN1[2]);
                resultvalue[3] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[3], 10, ref pointReaultN1[3]);
                if (resultvalue[3])
                {

                    Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[3].Row1 + 40;
                    Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[3].Colum1;
                    Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[3].Row2 + 40;
                    Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[3].Colum2;
                    resultvalue[4] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[4], 30, ref pointReaultN1[4]);
                    if (resultvalue[4])
                    {

                        Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[4].Row1 - (pointReaultN1[4].Row1 - pointReaultN1[3].Row1)/2;
                        Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[4].Colum1;
                        Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[4].Row2 - (pointReaultN1[4].Row1 - pointReaultN1[3].Row1)/2;
                        Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[4].Colum2;
                        resultvalue[5] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[4], (pointReaultN1[4].Row1 - pointReaultN1[3].Row1) / 6, ref pointReaultN1[5]);

                        Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[4].Row1 + 20;
                        Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[4].Colum1;
                        Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[4].Row2 + 20;
                        Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[4].Colum2;
                        resultvalue[6] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N1[4], 10, ref pointReaultN1[6]);
                    }
                }
                Halcon.DetectionHalconRect(hWindow, ImageAffineTrans, Parameter.specifications.矩形检测区域N1, ref result[3]);

                Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.基准N1[0], 200, ref BaseReaultN1[0]);
                Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.基准N1[1], 200, ref BaseReaultN1[1]);

                HOperatorSet.IntersectionLines(BaseReaultN1[0].Row1, BaseReaultN1[0].Colum1, BaseReaultN1[0].Row2, BaseReaultN1[0].Colum2,
                    BaseReaultN1[1].Row1, BaseReaultN1[1].Colum1, BaseReaultN1[1].Row2, BaseReaultN1[1].Colum2, out Row, out Column, out IsOverlapping);
                bool resultdpl = false;
                Halcon.DetectionHalconDeepLearningN1(hWindow, ImageAffineTrans, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, Row, Column, ref resultdpl);
                if(resultvalue[5] || resultvalue[6] || !resultdpl)
                {
                    result[4] = false;
                }
                else
                {
                    result[4] = true;
                }
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                HTuple minDistance, maxDistance;
                HOperatorSet.DistanceSs(pointReaultN1[0].Row1, pointReaultN1[0].Colum1, pointReaultN1[0].Row2, pointReaultN1[0].Colum2,
                    pointReaultN1[1].Row1, pointReaultN1[1].Colum1, pointReaultN1[1].Row2, pointReaultN1[1].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 100, 100);
                value[0] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN1;
                if (value[0] - Parameter.specifications.胶宽.value < Parameter.specifications.胶宽.min ||
                    value[0] - Parameter.specifications.胶宽.value > Parameter.specifications.胶宽.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[0] = false;
                }
                else
                {
                    result[0] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "胶宽" + value[0]);
                HOperatorSet.DistanceSs(pointReaultN1[2].Row1, pointReaultN1[2].Colum1, pointReaultN1[2].Row2, pointReaultN1[2].Colum2,
                    pointReaultN1[3].Row1, pointReaultN1[3].Colum1, pointReaultN1[3].Row2, pointReaultN1[3].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 150, 100);
                value[1] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN1;

                if (value[1] - Parameter.specifications.胶高.value < Parameter.specifications.胶高.min ||
                    value[1] - Parameter.specifications.胶高.value > Parameter.specifications.胶高.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[1] = false;
                }
                else
                {
                    result[1] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "胶高" + value[1]);
                HOperatorSet.DistanceSs(pointReaultN1[3].Row1, pointReaultN1[3].Colum1, pointReaultN1[3].Row2, pointReaultN1[3].Colum2,
                    pointReaultN1[4].Row1, pointReaultN1[4].Colum1, pointReaultN1[4].Row2, pointReaultN1[4].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 200, 100);
                value[2] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN1;

                if (value[2] - Parameter.specifications.胶线.value < Parameter.specifications.胶线.min ||
                    value[2] - Parameter.specifications.胶线.value > Parameter.specifications.胶线.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[2] = false;
                }
                else
                {
                    result[2] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                minDistance.Dispose();
                maxDistance.Dispose();
                HOperatorSet.WriteString(hWindow, "胶线" + value[2]);

                
                angle.Dispose();
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();
                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();
                ImageAffineTran.Dispose();
                ImageAffineTrans.Dispose();
                stopwatch.Stop(); //  停止监视
                TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
                LogHelper.Log.WriteInfo("程序检测时间:" + System.DateTime.Now.ToString("ss-fff") + "程序检测时长:" + milliseconds.ToString() + "ms");
                主窗体.AlarmList.Add("程序检测时间:" + System.DateTime.Now.ToString("ss-fff") + "程序检测时长:" + milliseconds.ToString() + "ms");
            }
            catch 
            {
            
            }
           
        }

        public static void defectionN2(HWindow hWindow,HObject hImage, ref bool[] result,ref double[] value)
        {
            try
            {
                bool[] resultvalue = new bool[8];
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                HOperatorSet.DispObj(hImage, hWindow);
                Halcon.DetectionHalconLine(hWindow, hImage, Parameter.specifications.基准N2[0], 200, ref BaseReaultN2[0]);
                Halcon.DetectionHalconLine(hWindow, hImage, Parameter.specifications.基准N2[1], 200, ref BaseReaultN2[1]);


                HTuple angle, Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReaultN2[0].Row1, BaseReaultN2[0].Colum1, BaseReaultN2[0].Row2, BaseReaultN2[0].Colum2,
                    BaseReaultN2[1].Row1, BaseReaultN2[1].Colum1, BaseReaultN2[1].Row2, BaseReaultN2[1].Colum2, out Row, out Column, out IsOverlapping);

                HOperatorSet.AngleLx(BaseReaultN2[1].Row1, BaseReaultN2[1].Colum1, BaseReaultN2[1].Row2, BaseReaultN2[1].Colum2, out angle);
                HTuple HomMat2DIdentity;
                HTuple HomMat2DRotate;
                HObject ImageAffineTran;
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, -angle, Row, Column, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(hImage, out ImageAffineTran, HomMat2DRotate, "constant", "false");

                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();

                HObject ImageAffineTrans;
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameter.specifications.BaseRowN2, -Column + Parameter.specifications.BaseColumnN2, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageAffineTran, out ImageAffineTrans, HomMat2DRotate, "constant", "false");


                HOperatorSet.DispObj(ImageAffineTrans, hWindow);
                resultvalue[0] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[0], 100, ref pointReaultN2[0]);
                resultvalue[1] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[1], 100, ref pointReaultN2[1]);
                resultvalue[2] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[2], 100, ref pointReaultN2[2]);
                resultvalue[3] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[3], 10, ref pointReaultN2[3]);
                if (resultvalue[3])
                {
                    Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[3].Row1 - 40;
                    Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[3].Colum1;
                    Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[3].Row2 - 40;
                    Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[3].Colum2;
                    resultvalue[4] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[4], 30, ref pointReaultN2[4]);
                    if (resultvalue[4])
                    {
                        Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[4].Row1 - 20;
                        Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[4].Colum1;
                        Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[4].Row2 - 20;
                        Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[4].Colum2;
                        resultvalue[5] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[4], 10, ref pointReaultN2[5]);

                        Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[4].Row1 + (pointReaultN2[3].Row1 - pointReaultN2[4].Row1) / 2;
                        Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[4].Colum1;
                        Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[4].Row2 + (pointReaultN2[3].Row1 - pointReaultN2[4].Row1) / 2;
                        Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[4].Colum2;
                        resultvalue[6] = Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.模板区域N2[4], (pointReaultN2[3].Row1 - pointReaultN2[4].Row1) / 6, ref pointReaultN2[6]);
                    }
                }
                result[3] = false;
                Halcon.DetectionHalconRect(hWindow, ImageAffineTrans, Parameter.specifications.矩形检测区域N2, ref result[3]);
                Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.基准N2[0], 200, ref BaseReaultN2[0]);
                Halcon.DetectionHalconLine(hWindow, ImageAffineTrans, Parameter.specifications.基准N2[1], 200, ref BaseReaultN2[1]);

                HOperatorSet.IntersectionLines(BaseReaultN2[0].Row1, BaseReaultN2[0].Colum1, BaseReaultN2[0].Row2, BaseReaultN2[0].Colum2,
                    BaseReaultN2[1].Row1, BaseReaultN2[1].Colum1, BaseReaultN2[1].Row2, BaseReaultN2[1].Colum2, out Row, out Column, out IsOverlapping);
                bool resultdpl = false;
                Halcon.DetectionHalconDeepLearningN2(hWindow, ImageAffineTrans, hv_DLModelHandle2, hv_DLPreprocessParam2, hv_InferenceClassificationThreshold2, hv_InferenceSegmentationThreshold2, Row, Column, ref resultdpl);
                if (resultvalue[5] || resultvalue[6] || !resultdpl)
                {
                    result[4] = false;
                }
                else
                {
                    result[4] = true;
                }
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                HTuple minDistance, maxDistance;
                HOperatorSet.DistanceSs(pointReaultN2[0].Row1, pointReaultN2[0].Colum1, pointReaultN2[0].Row2, pointReaultN2[0].Colum2,
                    pointReaultN2[1].Row1, pointReaultN2[1].Colum1, pointReaultN2[1].Row2, pointReaultN2[1].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 100, 100);
                value[0] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN2;
                if (value[0] - Parameter.specifications.胶宽.value < Parameter.specifications.胶宽.min ||
                    value[0] - Parameter.specifications.胶宽.value > Parameter.specifications.胶宽.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[0] = false;
                }
                else
                {
                    result[0] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "胶宽" + value[0]);
                HOperatorSet.DistanceSs(pointReaultN2[2].Row1, pointReaultN2[2].Colum1, pointReaultN2[2].Row2, pointReaultN2[2].Colum2,
                    pointReaultN2[3].Row1, pointReaultN2[3].Colum1, pointReaultN2[3].Row2, pointReaultN2[3].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 150, 100);
                value[1] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN2;

                if (value[1] - Parameter.specifications.胶高.value < Parameter.specifications.胶高.min ||
                    value[1] - Parameter.specifications.胶高.value > Parameter.specifications.胶高.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[1] = false;
                }
                else
                {
                    result[1] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "胶高" + value[1]);
                HOperatorSet.DistanceSs(pointReaultN2[3].Row1, pointReaultN2[3].Colum1, pointReaultN2[3].Row2, pointReaultN2[3].Colum2,
                    pointReaultN2[4].Row1, pointReaultN2[4].Colum1, pointReaultN2[4].Row2, pointReaultN2[4].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 200, 100);
                value[2] = minDistance * Parameter.specifications.PixelResolution + Parameter.specifications.胶宽.adjustN2;

                if (value[2] - Parameter.specifications.胶线.value < Parameter.specifications.胶线.min ||
                    value[2] - Parameter.specifications.胶线.value > Parameter.specifications.胶线.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[2] = false;
                }
                else
                {
                    result[2] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                minDistance.Dispose();
                maxDistance.Dispose();
                HOperatorSet.WriteString(hWindow, "胶线" + value[2]);

                
                angle.Dispose();
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();
                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();
                ImageAffineTran.Dispose();
                ImageAffineTrans.Dispose();
                stopwatch.Stop(); //  停止监视
                TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
                double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
                LogHelper.Log.WriteInfo("程序检测时间:" + System.DateTime.Now.ToString("ss-fff") + "程序检测时长:" + milliseconds.ToString() + "ms");
                主窗体.AlarmList.Add("程序检测时间:" + System.DateTime.Now.ToString("ss-fff") + "程序检测时长:" + milliseconds.ToString() + "ms");
            }
            catch
            { 
            
            }
           
        }

        public void Bitmap2HObjectBpp24(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);

            }
            catch
            {
                image = null;
            }
        }

        public void Mat2HObjectBpp8(Mat mat, out HObject image)
        {
            int ImageWidth = mat.Width;
            int ImageHeight = mat.Height;
            int channel = mat.Channels();
            long size = ImageWidth * ImageHeight * channel;
            int col_byte_num = ImageWidth * channel;

            byte[] rgbValues = new byte[size];
            //IntPtr imgptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(rgbValues.Length);
            unsafe
            {
                for (int i = 0; i < mat.Height; i++)
                {
                    IntPtr c = mat.Ptr(i);
                    //byte* c1 = (byte*)c;
                    System.Runtime.InteropServices.Marshal.Copy(c, rgbValues, i * col_byte_num, col_byte_num); // 一行一行将mat 像素复制到byte[], 
                }

                void* p;
                IntPtr ptr;
                fixed (byte* pc = rgbValues)
                {
                    p = (void*)pc;
                    ptr = new IntPtr(p);

                }

                HOperatorSet.GenImage1(out image, "byte", ImageWidth, ImageHeight, ptr);
            }

        }

        private void chk_Rect_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chk_trian_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_显示检测区域_Click(object sender, EventArgs e)
        { 
            bool result = true;
            Halcon.DetectionHalconRect(hWindowControl1.HalconWindow,主窗体.hImage[0], Parameter.specifications.矩形检测区域N1,ref result);
        }

        private void num_PixelResolution_ValueChanged(object sender, double value)
        {
            Parameter.specifications.PixelResolution = num_PixelResolution.Value;
        }

        private void chk_SaveDefeatImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specifications.SaveDefeatImage = chk_SaveDefeatImage.Checked;
        }

        private void chk_SaveOrigalImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specifications.SaveOrigalImage = chk_SaveOrigalImage.Checked;
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.模板区域N1[4]);           
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.模板区域N1[3]);
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.模板区域N1[0]);
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.模板区域N1[1]);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.模板区域N1[2]);
        }

        public void Mat2HObjectBpp24(Mat mat, out HObject image)
        {
            int ImageWidth = mat.Width;
            int ImageHeight = mat.Height;
            int channel = mat.Channels();
            long size = ImageWidth * ImageHeight * channel;
            int col_byte_num = ImageWidth * channel;

            byte[] rgbValues = new byte[size];
            //IntPtr imgptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(rgbValues.Length);
            unsafe
            {
                for (int i = 0; i < mat.Height; i++)
                {
                    IntPtr c = mat.Ptr(i);
                    //byte* c1 = (byte*)c;
                    System.Runtime.InteropServices.Marshal.Copy(c, rgbValues, i * col_byte_num, col_byte_num);
                }

                void* p;
                IntPtr ptr;
                fixed (byte* pc = rgbValues)
                {
                    p = (void*)pc;
                    ptr = new IntPtr(p);

                }
                HOperatorSet.GenImageInterleaved(out image, ptr, "bgr", ImageWidth, ImageHeight, 0, "byte", 0, 0, 0, 0, -1, 0);

            }

        }
        public static HRect1[] pointReaultN1 = new HRect1[8];
        public static HRect1[] pointReaultN2 = new HRect1[8];
        static HRect1[] BaseReaultN1 = new HRect1[3];
        static HRect1[] BaseReaultN2 = new HRect1[3];
        private void uiButton10_Click(object sender, EventArgs e)
        {
            
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[0], 100, ref pointReaultN1[0]);

        }

        private void uiButton8_Click(object sender, EventArgs e)
        {         
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[1], 100,  ref pointReaultN1[1]);

        }

        private void uiButton9_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[2], 100, ref pointReaultN1[2]);
        }

        private void uiButton11_Click(object sender, EventArgs e)
        {
            bool result = Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[3], 10, ref pointReaultN1[3]);
            if (result)
            {
                Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[3].Row1 + 40;
                Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[3].Colum1;
                Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[3].Row2 + 40;
                Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[3].Colum2;
                result = Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[4], 20, ref pointReaultN1[4]);
                if (result)
                {
                    Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[4].Row1 - 20;
                    Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[4].Colum1;
                    Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[4].Row2 - 20;
                    Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[4].Colum2;
                    Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[4], 10, ref pointReaultN1[5]);

                    Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[4].Row1 + 20;
                    Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[4].Colum1;
                    Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[4].Row2 + 20;
                    Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[4].Colum2;
                    Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[4], 10, ref pointReaultN1[6]);

                }
            }                     
        }

        private void uiButton3_Click_1(object sender, EventArgs e)
        {
            HObject Rectangle1;
            //HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
            //HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
            //HOperatorSet.GenRectangle1(out Rectangle1,
            //    Parameter.specifications.矩形检测区域.Row1, Parameter.specifications.矩形检测区域.Colum1,
            //    Parameter.specifications.矩形检测区域.Row2, Parameter.specifications.矩形检测区域.Colum2);
            HTuple hv_width, hv_height;
            HOperatorSet.CropPart(主窗体.hImage[0], out Rectangle1, Parameter.specifications.矩形检测区域N1.Row1, Parameter.specifications.矩形检测区域N1.Colum1, Parameter.specifications.矩形检测区域N1.Colum2 - Parameter.specifications.矩形检测区域N1.Colum1, Parameter.specifications.矩形检测区域N1.Row2 - Parameter.specifications.矩形检测区域N1.Row1);

            HOperatorSet.GetImageSize(Rectangle1, out hv_width, out hv_height);
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, - 1, - 1);//设置窗体的规格HOperatorSet.DispObj(Rectangle1, hWindowControl1.HalconWindow);
            HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);
            HOperatorSet.DispObj(Rectangle1, hWindowControl1.HalconWindow);
            HOperatorSet.GenRectangle1(out Rectangle1, Parameter.specifications.矩形模板区域.Row1, Parameter.specifications.矩形模板区域.Colum1, Parameter.specifications.矩形模板区域.Row2, Parameter.specifications.矩形模板区域.Colum2);
            HOperatorSet.DispObj(Rectangle1, hWindowControl1.HalconWindow);
        }
        public static HTuple hv_DLModelHandle;
        public static HTuple hv_DLPreprocessParam;
        public static HTuple hv_InferenceClassificationThreshold;
        public static HTuple hv_InferenceSegmentationThreshold;
        private void uiButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
                               //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
                               //Halcon.DetectionHalconDeepLearning(hWindowControl1.HalconWindow, 主窗体.hImage, 0, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, point[1], point[2], point[4]);
            bool[] result=new bool[5];
            double[] value=new double[3];
            defectionN1(hWindowControl1.HalconWindow ,主窗体.hImage[0], ref result,ref value);
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
            
        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            //Halcon.DetectionDrawRect2AOI(hWindowControl1.HalconWindow, 主窗体.hImage, ref Parameter.specifications.检测基准N1[0]);
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.基准N1[0]);
        }

        private void uiButton16_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.基准N1[1]);
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[0], ref Parameter.specifications.基准N1[2]);
        }

        static HTuple[] point= new HTuple[6];

        private void uiButton18_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.基准N1[0], 200, ref BaseReaultN1[0]);            
        }

        private void uiButton21_Click(object sender, EventArgs e)
        {
            相机配置 flg = new 相机配置();
            flg.ShowDialog();
        }

        private void btn_Minimizid_System_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void uiButton17_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.基准N1[1], 200, ref BaseReaultN1[1]);
        }

        private void uiButton19_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.基准N1[2], 200, ref BaseReaultN1[2]);
        }

        private void uiButton25_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.基准N2[0]);
        }

        private void uiButton27_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.基准N2[1]);
        }

        private void uiButton26_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.基准N2[2]);
        }
        public static HTuple hv_DLModelHandle2;
        public static HTuple hv_DLPreprocessParam2;
        public static HTuple hv_InferenceClassificationThreshold2;
        public static HTuple hv_InferenceSegmentationThreshold2;

        private void uiButton42_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
            Halcon.DetectionHalconReadDlModel(Application.StartupPath + "/halcon/N2.hdl", out hv_DLModelHandle2, out hv_DLPreprocessParam2, out hv_InferenceClassificationThreshold2, out hv_InferenceSegmentationThreshold2);

            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
        }

        private void uiButton31_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
            //Halcon.DetectionHalconDeepLearning(hWindowControl1.HalconWindow, 主窗体.hImage, 0, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, point[1], point[2], point[4]);
            bool[] result = new bool[5];
            double[] value = new double[3];
            defectionN2(hWindowControl1.HalconWindow,主窗体.hImage[1], ref result,ref value);
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
        }
        static HTuple[] point2 = new HTuple[6];
        private void uiButton29_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.基准N2[0], 200, ref BaseReaultN2[0]);
        }

        private void uiButton28_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.基准N2[1], 200, ref BaseReaultN2[1]);
        }

        private void uiButton30_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.基准N2[2], 200, ref BaseReaultN2[2]);
        }

        private void uiButton12_Click(object sender, EventArgs e)
        {
            bool result = Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[4], 10, ref pointReaultN1[4]);
            if(result)
            {
                Parameter.specifications.模板区域N1[4].Row1 = pointReaultN1[4].Row1 + 20;
                Parameter.specifications.模板区域N1[4].Colum1 = pointReaultN1[4].Colum1;
                Parameter.specifications.模板区域N1[4].Row2 = pointReaultN1[4].Row2 + 20;
                Parameter.specifications.模板区域N1[4].Colum2 = pointReaultN1[4].Colum2;
                Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.模板区域N1[4], 10, ref pointReaultN1[5]);
            }
            
        }

        private void uiButton36_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.模板区域N2[0]);
        }

        private void uiButton38_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.模板区域N2[1]);
        }

        private void uiButton37_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.模板区域N2[2]);
        }

        private void uiButton33_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.模板区域N2[3]);
        }

        private void uiButton32_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.模板区域N2[4]);
        }

        private void uiButton40_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[0], 100, ref pointReaultN2[0]);
        }

        private void uiButton39_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[1], 100, ref pointReaultN2[1]);
        }

        private void uiButton41_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[2], 100, ref pointReaultN2[2]);
        }

        private void uiButton34_Click(object sender, EventArgs e)
        {
            bool result = Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[3], 10, ref pointReaultN2[3]);
            if (result)
            {
                Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[3].Row1 - 40;
                Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[3].Colum1;
                Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[3].Row2 - 40;
                Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[3].Colum2;
                Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[4], 20, ref pointReaultN2[4]);
                if (result)
                {
                    Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[4].Row1 - 20;
                    Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[4].Colum1;
                    Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[4].Row2 - 20;
                    Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[4].Colum2;
                    Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[4], 10, ref pointReaultN2[5]);

                    Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[4].Row1 + 20;
                    Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[4].Colum1;
                    Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[4].Row2 + 20;
                    Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[4].Colum2;
                    Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[4], 10, ref pointReaultN2[6]);
                }
            }
            
        }

        private void uiButton35_Click(object sender, EventArgs e)
        {
            bool result = Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[4], 10, ref pointReaultN2[4]);
            if (result)
            {
                Parameter.specifications.模板区域N2[4].Row1 = pointReaultN2[4].Row1 - 20;
                Parameter.specifications.模板区域N2[4].Colum1 = pointReaultN2[4].Colum1;
                Parameter.specifications.模板区域N2[4].Row2 = pointReaultN2[4].Row2 - 20;
                Parameter.specifications.模板区域N2[4].Colum2 = pointReaultN2[4].Colum2;
                Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.模板区域N2[4], 10, ref pointReaultN2[5]);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uiButton43_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawRectAOI(hWindowControl1.HalconWindow, 主窗体.hImage[1], ref Parameter.specifications.矩形检测区域N2);
        }

        private void uiButton45_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.基准N2[0], 200, ref BaseReaultN2[0]);

                Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.基准N2[1], 200, ref BaseReaultN2[1]);
                HTuple Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReaultN2[0].Row1, BaseReaultN2[0].Colum1, BaseReaultN2[0].Row2, BaseReaultN2[0].Colum2,
                    BaseReaultN2[1].Row1, BaseReaultN2[1].Colum1, BaseReaultN2[1].Row2, BaseReaultN2[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameter.specifications.BaseRowN2 = Row;
                Parameter.specifications.BaseColumnN2 = Column;
                HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();
                XMLHelper.serialize<Parameter.Specifications>(Parameter.specifications, "Parameter/Specifications.xml");

            }
            catch
            {

            }
        }

        private void uiButton46_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.基准N1[0], 200, ref BaseReaultN1[0]);

            Halcon.DetectionHalconLine(hWindowControl1.HalconWindow, 主窗体.hImage[0], Parameter.specifications.基准N1[1], 200, ref BaseReaultN1[1]);

            HTuple Row, Column, IsOverlapping;
            HOperatorSet.IntersectionLines(BaseReaultN1[0].Row1, BaseReaultN1[0].Colum1, BaseReaultN1[0].Row2, BaseReaultN1[0].Colum2,
                BaseReaultN1[1].Row1, BaseReaultN1[1].Colum1, BaseReaultN1[1].Row2, BaseReaultN1[1].Colum2, out Row, out Column, out IsOverlapping);
            Parameter.specifications.BaseRowN1 = Row;
            Parameter.specifications.BaseColumnN1 = Column;
            HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
            Row.Dispose();
            Column.Dispose();
            IsOverlapping.Dispose();
            XMLHelper.serialize<Parameter.Specifications>(Parameter.specifications, "Parameter/Specifications.xml");
        }

        private void uiButton44_Click(object sender, EventArgs e)
        {
            bool result = true;
            Halcon.DetectionHalconRect(hWindowControl1.HalconWindow, 主窗体.hImage[1], Parameter.specifications.矩形检测区域N2,ref result);
        }

        private void num_ThresholdLow_ValueChanged(object sender, double value)
        {
            Parameter.specifications.ThresholdLow = value;
        }

        private void num_ThresholdHigh_ValueChanged(object sender, double value)
        {
            Parameter.specifications.ThresholdHigh = value;
        }

        private void num_AreaLow_ValueChanged(object sender, double value)
        {
            Parameter.specifications.AreaLow = value;
        }

        private void num_AreaHigh_ValueChanged(object sender, double value)
        {
            Parameter.specifications.AreaHigh = value;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void uiDoubleUpDown1_ValueChanged(object sender, double value)
        {
            Parameter.specifications.DeepLearningRateN1 = value;
        }

        private void uiDoubleUpDown2_ValueChanged(object sender, double value)
        {
            Parameter.specifications.DeepLearningRateN2 = value;
        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK && (openfile.FileName != ""))
            {
                //picture0.ImageLocation = openfile.FileName;
                //MatImage = Cv2.ImRead(openfile.FileName);
                Halcon.ImgDisplay(1,openfile.FileName, hWindowControl1.HalconWindow); ;
            }
            openfile.Dispose();
        }
    }
}
