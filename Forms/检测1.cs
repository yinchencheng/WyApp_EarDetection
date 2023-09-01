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
    public partial class 检测1 : Form
    {
        public static int Round_Edge = 0;
        System.Drawing.Point downPoint;
        public 检测1()
        {
            InitializeComponent();
            MainForm.isTestMode = false;
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
                Halcon.ImgDisplay(uiComboBox1.SelectedIndex, openfile.FileName, hWindowControl1.HalconWindow);;
            }
            openfile.Dispose();
        }

        private void btn_绘制检测区域_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawRectAOI(hWindowControl1.HalconWindow,MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].矩形检测区域);
        }

        

        

        private void btn_SaveParams_Click(object sender, EventArgs e)
        {
            XMLHelper.serialize<Parameter.SpecificationsCam1>(Parameter.specificationsCam1[uiComboBox1.SelectedIndex], Parameter.commministion.productName + "/Cam1Specifications" + uiComboBox1.SelectedIndex + ".xml");
        }

        private void chk_Cricle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void 检测设置_Load(object sender, EventArgs e)
        {
            uiComboBox1.SelectedIndex = 0;
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage[0], hWindowControl1.HalconWindow);
            num_AreaHigh.Value = Parameter.specificationsCam1[0].AreaHigh;
            num_AreaLow.Value = Parameter.specificationsCam1[0].AreaLow;
            num_ThresholdHigh.Value = Parameter.specificationsCam1[0].ThresholdHigh;
            num_ThresholdLow.Value = Parameter.specificationsCam1[0].ThresholdLow;
            num_PixelResolution.Value=Parameter.specificationsCam1[0].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameter.specificationsCam1[0].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameter.specificationsCam1[0].SaveDefeatImage;
            uiDoubleUpDown1.Value = Parameter.specificationsCam1[0].DeepLearningRate;
        }

       
        public static bool Detection(int i, HWindow hWindow,HObject hImage, ref bool[] result,ref double[] value)
        {
            try
            {
                bool[] resultvalue = new bool[8];
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                //HOperatorSet.DispObj(hImage, hWindow);
                Halcon.DetectionHalconLine(i,hWindow, hImage, Parameter.specificationsCam1[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i,hWindow, hImage, Parameter.specificationsCam1[i].基准[1], ref BaseReault[1]);

                HTuple angle, Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);

                HOperatorSet.AngleLx(BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out angle);
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
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, - Row + Parameter.specificationsCam1[i].BaseRow, - Column + Parameter.specificationsCam1[i].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageAffineTran, out ImageAffineTrans, HomMat2DRotate, "constant", "false");
                HOperatorSet.AffineTransImage(ImageAffineTran, out MainForm.hImage[i], HomMat2DRotate, "constant", "false");
                HOperatorSet.DispObj(ImageAffineTrans, hWindow);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].基准[1], ref BaseReault[1]);
                resultvalue[0] = Halcon.DetectionHalconLine(i,hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[0], ref pointReault[0]);
                resultvalue[1] = Halcon.DetectionHalconLine(i,hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[1], ref pointReault[1]);
                resultvalue[2] = Halcon.DetectionHalconLine(i,hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[2], ref pointReault[2]);
                if (i == 1)
                {
                    Parameter.specificationsCam1[i].模板区域[4].LineLength = Parameter.specificationsCam1[i].模板区域[3].LineLength;
                    Parameter.specificationsCam1[i].模板区域[4].Row1 = BaseReault[1].Row1 + Parameter.specificationsCam1[i].模板区域[4].LineLength;
                    Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[2].Colum1;
                    Parameter.specificationsCam1[i].模板区域[4].Row2 = BaseReault[1].Row2 + Parameter.specificationsCam1[i].模板区域[4].LineLength;
                    Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[2].Colum2;

                    resultvalue[4] = Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[4]);
                    if (resultvalue[4])
                    {
                        Parameter.specificationsCam1[i].模板区域[4].Row1 = pointReault[4].Row1 - Parameter.specificationsCam1[i].模板区域[4].LineLength / 4 * 3;
                        Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[4].Colum1;
                        Parameter.specificationsCam1[i].模板区域[4].Row2 = pointReault[4].Row2 - Parameter.specificationsCam1[i].模板区域[4].LineLength / 4 * 3;
                        Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[4].Colum2;
                        Parameter.specificationsCam1[i].模板区域[4].LineLength = Parameter.specificationsCam1[i].模板区域[3].LineLength / 2;
                        Halcon.DetectionHalconLine(i,hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[5]);

                        Parameter.specificationsCam1[i].模板区域[4].Row1 = (pointReault[4].Row1 + pointReault[1].Row1) / 2; ;
                        Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[4].Colum1;
                        Parameter.specificationsCam1[i].模板区域[4].Row2 = (pointReault[4].Row1 + pointReault[1].Row1) / 2; ;
                        Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[4].Colum2;
                        Parameter.specificationsCam1[i].模板区域[4].LineLength = pointReault[4].Row1 - pointReault[1].Row1 - 5;

                        Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[6]);
                    }
                }
                else
                {


                    Parameter.specificationsCam1[i].模板区域[4].LineLength = Parameter.specificationsCam1[i].模板区域[3].LineLength;
                    Parameter.specificationsCam1[i].模板区域[4].Row1 = BaseReault[1].Row1 - Parameter.specificationsCam1[i].模板区域[4].LineLength ;
                    Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[2].Colum1;
                    Parameter.specificationsCam1[i].模板区域[4].Row2 = BaseReault[1].Row2 - Parameter.specificationsCam1[i].模板区域[4].LineLength ;
                    Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[2].Colum2;
                    resultvalue[4] = Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[4]);
                    if (resultvalue[4])
                    {

                        Parameter.specificationsCam1[i].模板区域[4].Row1 = (pointReault[4].Row1 + pointReault[1].Row1) / 2;
                        Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[4].Colum1;
                        Parameter.specificationsCam1[i].模板区域[4].Row2 = (pointReault[4].Row1 + pointReault[1].Row1) / 2;
                        Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[4].Colum2;
                        Parameter.specificationsCam1[i].模板区域[4].LineLength = pointReault[1].Row1 - pointReault[4].Row1 - 5;
                        Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[5]);
                        Parameter.specificationsCam1[i].模板区域[4].Row1 = pointReault[4].Row1 - Parameter.specificationsCam1[i].模板区域[4].LineLength / 4*3 ;
                        Parameter.specificationsCam1[i].模板区域[4].Colum1 = pointReault[4].Colum1;
                        Parameter.specificationsCam1[i].模板区域[4].Row2 = pointReault[4].Row2 - Parameter.specificationsCam1[i].模板区域[4].LineLength / 4*3 ;
                        Parameter.specificationsCam1[i].模板区域[4].Colum2 = pointReault[4].Colum2;
                        Parameter.specificationsCam1[i].模板区域[4].LineLength = Parameter.specificationsCam1[i].模板区域[3].LineLength/2;
                        Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].模板区域[4], ref pointReault[6]);
                    }
                }
                
                Halcon.DetectionHalconRect(i,hWindow, ImageAffineTrans, Parameter.specificationsCam1[i].矩形检测区域, ref result[3]);
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                bool resultdpl = false;//false
                Halcon.DetectionHalconDeepLearning(i,hWindow, ImageAffineTrans, hv_DLModelHandle[i], hv_DLPreprocessParam[i], hv_InferenceClassificationThreshold[i], hv_InferenceSegmentationThreshold[i], Row, Column, ref resultdpl);
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
                HOperatorSet.DistanceSs(pointReault[0].Row1, pointReault[0].Colum1, pointReault[0].Row2, pointReault[0].Colum2,
                    pointReault[1].Row1, pointReault[1].Colum1, pointReault[1].Row2, pointReault[1].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 100, 100);
                value[0] = minDistance * Parameter.specificationsCam1[i].PixelResolution + Parameter.specificationsCam1[i].肩高.adjust;
                if (value[0] - Parameter.specificationsCam1[0].肩高.value < Parameter.specificationsCam1[0].肩高.min ||
                    value[0] - Parameter.specificationsCam1[0].肩高.value > Parameter.specificationsCam1[0].肩高.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[0] = false;
                }
                else
                {
                    result[0] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "肩高" + value[0]);
                HOperatorSet.DistanceSs(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 200, 100);
                value[1] = minDistance * Parameter.specificationsCam1[i].PixelResolution + Parameter.specificationsCam1[i].肩宽.adjust;

                if (value[1] - Parameter.specificationsCam1[0].肩宽.value < Parameter.specificationsCam1[0].肩宽.min ||
                    value[1] - Parameter.specificationsCam1[0].肩宽.value > Parameter.specificationsCam1[0].肩宽.max)
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    result[1] = false;
                }
                else
                {
                    result[1] = true;
                    HOperatorSet.SetColor(hWindow, "green");
                }
                HOperatorSet.WriteString(hWindow, "肩宽" + value[1]);
                HOperatorSet.DistanceSs(BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2,
                    pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2, out minDistance, out maxDistance);
                HOperatorSet.SetTposition(hWindow, 300, 100);
                value[2] = Math.Abs ( pointReault[4].Row1.D- BaseReault[1].Row1.D) * Parameter.specificationsCam1[i].PixelResolution + Parameter.specificationsCam1[i].胶线.adjust;

                if (value[2] - Parameter.specificationsCam1[0].胶线.value < Parameter.specificationsCam1[0].胶线.min ||
                    value[2] - Parameter.specificationsCam1[0].胶线.value > Parameter.specificationsCam1[0].胶线.max)
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
                MainForm.AlarmList.Add("程序检测时间:" + System.DateTime.Now.ToString("ss-fff") + "程序检测时长:" + milliseconds.ToString() + "ms");
                return true;
            }
            catch 
            {
                return false;
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

        private void btn_显示检测区域_Click(object sender, EventArgs e)
        { 
            bool result = true;
            Halcon.DetectionHalconRect(uiComboBox1.SelectedIndex,hWindowControl1.HalconWindow,MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].矩形检测区域,ref result);
        }

        private void num_PixelResolution_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].PixelResolution = num_PixelResolution.Value;
        }

        private void chk_SaveDefeatImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].SaveDefeatImage = chk_SaveDefeatImage.Checked;
        }

        private void chk_SaveOrigalImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].SaveOrigalImage = chk_SaveOrigalImage.Checked;
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4]);           
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3]);
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[0]);
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[1]);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[2]);
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
        public static HRect1[] pointReault = new HRect1[8];
        static HRect1[] BaseReault = new HRect1[3];
        private void uiButton10_Click(object sender, EventArgs e)
        {
            
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex,hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[0], ref pointReault[0]);

        }

        private void uiButton8_Click(object sender, EventArgs e)
        {         
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[1],  ref pointReault[1]);

        }

        private void uiButton9_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[2], ref pointReault[2]);
        }




        private void uiButton11_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);
            bool result = false;
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3],  ref pointReault[3]);
            if (uiComboBox1.SelectedIndex == 1)
            {
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = BaseReault[1].Row1 + Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[2].Colum1;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = BaseReault[1].Row2 + Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[2].Colum2;

                result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[4]);
                if (result)
                {
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = pointReault[4].Row1 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength / 4*3;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[4].Colum1;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = pointReault[4].Row2 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength / 4*3;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[4].Colum2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3].LineLength/2;
                    result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[5]);

                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = (pointReault[4].Row1 + pointReault[1].Row1) / 2; ;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[4].Colum1;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = (pointReault[4].Row1 + pointReault[1].Row1) / 2; ;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[4].Colum2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = pointReault[4].Row1 - pointReault[1].Row1 - 5;

                    result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[6]);
                }
            }
            else
            {
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = BaseReault[1].Row1 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[2].Colum1;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = BaseReault[1].Row2 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[2].Colum2;
                result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[4]);
                if (result)
                {
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = (pointReault[4].Row1 + pointReault[1].Row1)/2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[4].Colum1;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = (pointReault[4].Row1 + pointReault[1].Row1) / 2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[4].Colum2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = pointReault[1].Row1- pointReault[4].Row1-5;
                    result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[5]);

                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row1 = pointReault[4].Row1 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength / 4*3;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum1 = pointReault[4].Colum1;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Row2 = pointReault[4].Row2 - Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength / 4*3;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].Colum2 = pointReault[4].Colum2;
                    Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4].LineLength = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[3].LineLength / 2;
                    result = Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].模板区域[4], ref pointReault[6]);
                }
            }
                  
        }

       
        public static HTuple[] hv_DLModelHandle=new HTuple[2];
        public static HTuple[] hv_DLPreprocessParam = new HTuple[2];
        public static HTuple[] hv_InferenceClassificationThreshold = new HTuple[2];
        public static HTuple[] hv_InferenceSegmentationThreshold = new HTuple[2];
        private void uiButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
                               //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
                               //Halcon.DetectionHalconDeepLearning(hWindowControl1.HalconWindow, 主窗体.hImage, 0, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, point[1], point[2], point[4]);
            DateTime dtNow = System.DateTime.Now;  // 获取系统当前时间
            MainForm.strDateTime = dtNow.ToString("yyyyMMddHHmmss");
            MainForm.strDateTimeDay = dtNow.ToString("yyyy-MM-dd");
            bool[] result=new bool[5];
            double[] value=new double[3];
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Detection(uiComboBox1.SelectedIndex,hWindowControl1.HalconWindow ,MainForm.hImage[uiComboBox1.SelectedIndex], ref result,ref value);
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
            
        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            //Halcon.DetectionDrawRect2AOI(hWindowControl1.HalconWindow, 主窗体.hImage, ref Parameter.specifications.检测基准N1[0]);
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[0]);
        }

        private void uiButton16_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[1]);
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[2]);
        }

        static HTuple[] point= new HTuple[6];

        private void uiButton18_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[0],ref BaseReault[0]);            
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
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);
        }

        private void uiButton19_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[2], ref BaseReault[2]);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uiButton46_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HTuple Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseColumn = Column;
                HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();

                HTuple angle;
                HOperatorSet.AngleLx(BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out angle);
                HTuple HomMat2DIdentity;
                HTuple HomMat2DRotate;
                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dRotate(HomMat2DIdentity, -angle, Row, Column, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(MainForm.hImage[uiComboBox1.SelectedIndex], out MainForm.hImage[uiComboBox1.SelectedIndex], HomMat2DRotate, "constant", "false");

                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();

                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseRow, -Column + Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(MainForm.hImage[uiComboBox1.SelectedIndex], out MainForm.hImage[uiComboBox1.SelectedIndex], HomMat2DRotate, "constant", "false");

                HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage[uiComboBox1.SelectedIndex], Parameter.specificationsCam1[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameter.specificationsCam1[uiComboBox1.SelectedIndex].BaseColumn = Column;
                HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();



                XMLHelper.serialize<Parameter.SpecificationsCam1>(Parameter.specificationsCam1[uiComboBox1.SelectedIndex], Parameter.commministion.productName + "/Cam1Specifications" + uiComboBox1.SelectedIndex + ".xml");
            }
            catch
            {

            }
            
        }

        private void num_ThresholdLow_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].ThresholdLow = value;
        }

        private void num_ThresholdHigh_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].ThresholdHigh = value;
        }

        private void num_AreaLow_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].AreaLow = value;
        }

        private void num_AreaHigh_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].AreaHigh = value;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void uiDoubleUpDown1_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam1[uiComboBox1.SelectedIndex].DeepLearningRate = value;
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm.formloadIndex = uiComboBox1.SelectedIndex + 3;
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            num_AreaHigh.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].AreaHigh;
            num_AreaLow.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].AreaLow;
            num_ThresholdHigh.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].ThresholdHigh;
            num_ThresholdLow.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].ThresholdLow;
            num_PixelResolution.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].SaveDefeatImage;
            uiDoubleUpDown1.Value = Parameter.specificationsCam1[uiComboBox1.SelectedIndex].DeepLearningRate;
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 0;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton12_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 1;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();

        }

        private void uiButton3_Click_1(object sender, EventArgs e)
        {
            MainForm.LineIndex = 2;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 3;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton20_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 4;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton21_Click_1(object sender, EventArgs e)
        {
            MainForm.LineIndex = 5;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton22_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 6;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            HTuple row, col, grayval;
            row = (int)e.Y;
            col = (int)e.X;
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "white");
            HOperatorSet.GetGrayval(MainForm.hImage[uiComboBox1.SelectedIndex], row, col, out grayval);
            HOperatorSet.SetTposition(hWindowControl1.HalconWindow, row, col);
            HOperatorSet.WriteString(hWindowControl1.HalconWindow, "Gray:" + grayval);
            row.Dispose();
            col.Dispose();
            grayval.Dispose();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                MainForm.isTestMode = true;
            }
            else
            {
                MainForm.isTestMode = false;
            }
        }
    }
}
