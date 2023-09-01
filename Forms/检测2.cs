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
using HalconDotNet;
using Sunny.UI;
using static WY_App.Utility.Parameter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OpenCvSharp.Flann;

namespace WY_App
{
    public partial class 检测2 : Form
    {

        public static HRect1[] pointReault = new HRect1[20];
        static HRect1[] BaseReault = new HRect1[3];
        public 检测2()
        {
            InitializeComponent();
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage2[0], hWindowControl1.HalconWindow);
            MainForm.isTestMode = false;
        }
        Point downPoint;

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - downPoint.X,
                    this.Location.Y + e.Y - downPoint.Y);
            }
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btn_加载检测图片_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK && (openfile.FileName != ""))
            {
                Halcon.ImgDisplay2(uiComboBox1.SelectedIndex, openfile.FileName, hWindowControl1.HalconWindow);
            }
            openfile.Dispose();
        }

        private void btn_SaveParams_Click(object sender, EventArgs e)
        {
            XMLHelper.serialize<Parameter.SpecificationsCam2>(Parameter.specificationsCam2[uiComboBox1.SelectedIndex], Parameter.commministion.productName + "/Cam2Specifications" + uiComboBox1.SelectedIndex + ".xml");
        }

        private void btn_Close_System_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 检测2_Load(object sender, EventArgs e)
        {
            uiComboBox1.SelectedIndex = 0;
            uiComboBox2.SelectedIndex = 0;
            uiComboBox3.Clear();
            for ( int index = 0;index < 7;index++)
			{
                uiComboBox3.Items.Add(MainForm.DName[index]);
            }
            uiComboBox3.SelectedIndex = 0;
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage2[0], hWindowControl1.HalconWindow);
            num_AreaHigh.Value = Parameter.specificationsCam2[0].AreaHigh[0];
            num_AreaLow.Value = Parameter.specificationsCam2[0].AreaLow[0];
            num_ThresholdHigh.Value = Parameter.specificationsCam2[0].ThresholdHigh[0];
            num_ThresholdLow.Value = Parameter.specificationsCam2[0].ThresholdLow[0];
            num_PixelResolution.Value = Parameter.specificationsCam2[0].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameter.specificationsCam2[0].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameter.specificationsCam2[0].SaveDefeatImage;

            checkBox1.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[0];
            checkBox2.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[1];
            checkBox3.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[2];
            checkBox4.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[3];
        }

        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            num_AreaHigh.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex];
            num_AreaLow.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex];
            num_ThresholdHigh.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex];
            num_ThresholdLow.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex];
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiComboBox2.SelectedIndex = 0;
            MainForm.formloadIndex = uiComboBox1.SelectedIndex + 5;

            uiComboBox2_SelectedIndexChanged( sender,  e);
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);

            num_AreaHigh.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex];
            num_AreaLow.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex];
            num_ThresholdHigh.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex];
            num_ThresholdLow.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex];
            num_PixelResolution.Value = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].SaveDefeatImage;

            checkBox1.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[0];
            checkBox2.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[1];
            checkBox3.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[2];
            checkBox4.Checked = Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[3];

        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[0]);
        }

        private void uiButton16_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[1]);
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[2]);
        }

        private void uiButton18_Click(object sender, EventArgs e)
        {
            HObject hImage = new HObject();
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            HOperatorSet.Threshold(MainForm.hImage2[uiComboBox1.SelectedIndex], out hImage, 16, 255);
           // HOperatorSet.DispObj(hImage, hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex+2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[0],  ref BaseReault[0]);
            hImage.Dispose();

        }

        private void uiButton17_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex+2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[1],  ref BaseReault[1]);
        }

        private void uiButton19_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex + 2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[2], ref BaseReault[2]);
        }

        private void uiButton46_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HTuple Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn = Column;
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
                HOperatorSet.AffineTransImage(MainForm.hImage2[uiComboBox1.SelectedIndex], out MainForm.hImage2[uiComboBox1.SelectedIndex], HomMat2DRotate, "constant", "false");

                HomMat2DIdentity.Dispose();
                HomMat2DRotate.Dispose();

                HOperatorSet.HomMat2dIdentity(out HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow, -Column + Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(MainForm.hImage2[uiComboBox1.SelectedIndex], out MainForm.hImage2[uiComboBox1.SelectedIndex], HomMat2DRotate, "constant", "false");

                HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameter.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn = Column;
                HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();



                XMLHelper.serialize<Parameter.SpecificationsCam2>(Parameter.specificationsCam2[uiComboBox1.SelectedIndex], Parameter.commministion.productName + "/Cam2Specifications" + uiComboBox1.SelectedIndex + ".xml");
            }
            catch
            {

            }
            
        }

        private void btn_显示检测区域_Click(object sender, EventArgs e)
        {
            try
            {
                bool result = true;
                HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
                //Halcon.DetectionHalconRect1(uiComboBox1.SelectedIndex, uiComboBox2.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex], ref result);
                //bool dtResult = false;
                //Halcon.DetectionHalconRect2(uiComboBox1.SelectedIndex, uiComboBox2.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex], ref dtResult);
            }
            catch { }
            
        }

        private void uiButton29_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawRectAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex]);
        }

        private void num_ThresholdLow_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex] =value;
        }

        private void num_ThresholdHigh_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex] = value;
        }

        private void num_AreaLow_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex] = value;
        }

        private void num_AreaHigh_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex] = value;
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex*2]);
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2+1]);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[2]);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[3]);
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[4]);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[5]);
        }

		private void uiButton30_Click(object sender, EventArgs e)//总宽直线拟合1
		{
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[6]);
        }

		private void uiButton33_Click(object sender, EventArgs e)//总宽直线拟合2
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[7]);
		}
		private void uiButton36_Click(object sender, EventArgs e)//左短端直线拟合1
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[8]);
        }
		private void uiButton37_Click(object sender, EventArgs e)//左短端直线拟合2
		{
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[9]);
        }
		private void uiButton42_Click(object sender, EventArgs e)//右短端直线拟合1
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[10]);
        }

		private void uiButton43_Click(object sender, EventArgs e)//右短端直线拟合2
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[11]);
        }


		private void uiButton10_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2],  ref pointReault[uiComboBox3.SelectedIndex * 2]);
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameter.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2+1], ref pointReault[uiComboBox3.SelectedIndex * 2+1]);
        }

		private void uiButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
                               //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
                               //Halcon.DetectionHalconDeepLearning(hWindowControl1.HalconWindow, 主窗体.hImage, 0, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, point[1], point[2], point[4]);
            DateTime dtNow = System.DateTime.Now;  // 获取系统当前时间
            MainForm. strDateTime = dtNow.ToString("yyyyMMddHHmmss");
            MainForm.strDateTimeDay = dtNow.ToString("yyyy-MM-dd");
            bool[] result = new bool[10] {true, true, true, true, true, true, true, true, true, true };
            double[] value = new double[10];
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Detection(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref result, ref value);
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数           
            time.Text = milliseconds.ToString();
        }
        public static HTuple[] hv_DLModelHandle = new HTuple[2];
        public static HTuple[] hv_DLPreprocessParam = new HTuple[2];
        public static HTuple[] hv_InferenceClassificationThreshold = new HTuple[2];
        public static HTuple[] hv_InferenceSegmentationThreshold = new HTuple[2];
        public static bool Detection(int i, HWindow hWindow, HObject hImage, ref bool[] result, ref double[] value)
        {
            try
            {
                bool[] resultvalue = new bool[20];
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                                   //HOperatorSet.DispObj(hImage, hWindow);
                HOperatorSet.SetLineWidth(hWindow, 1);
                Halcon.DetectionHalconLine(i, hWindow, hImage, Parameter.specificationsCam2[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i, hWindow, hImage, Parameter.specificationsCam2[i].基准[1], ref BaseReault[1]);

                HTuple angle, Row, Column, IsOverlapping, angleRad;
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);

                HOperatorSet.AngleLx(BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out angle);
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
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameter.specificationsCam2[i].BaseRow, -Column + Parameter.specificationsCam2[i].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageAffineTran, out MainForm.hImage2[i], HomMat2DRotate, "constant", "false");
                HOperatorSet.AffineTransImage(ImageAffineTran, out ImageAffineTrans, HomMat2DRotate, "constant", "false");
                HOperatorSet.DispObj(ImageAffineTrans, hWindow);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam2[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam2[i].基准[1], ref BaseReault[1]);
                for(int index=0; index < 14; index++)
				{
                    resultvalue[index] = Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameter.specificationsCam2[i].模板区域[index], ref pointReault[index]);
                }


                HOperatorSet.AngleLl(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                    pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2, out angle);
                HOperatorSet.TupleDeg(angle ,out angleRad);
                HOperatorSet.SetTposition(hWindow, pointReault[5].Row1, pointReault[2].Colum1);
                HOperatorSet.WriteString(hWindow, "angle:"+ angleRad);
                
                value[7] = angleRad.D;
                Rect1 rect1 = new Rect1();
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                    pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2, out Row, out Column, out IsOverlapping);
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                rect1.Row1 = Row + 30;
                rect1.Colum1 = Column + 30;
                Parameter.specificationsCam2[i].矩形检测区域[1].Row1 = Row + 30;
                Parameter.specificationsCam2[i].矩形检测区域[1].Colum1 = Column + 30;
                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                    pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2, out Row, out Column, out IsOverlapping);
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                bool dplResult = true;

                rect1.Row2 = Row - 30;
                rect1.Colum2 = Column - 30;
                Parameter.specificationsCam2[i].矩形检测区域[1].Row2 = Row - 30;
                Parameter.specificationsCam2[i].矩形检测区域[1].Colum2 = Column - 30;
                bool[] grayResult = new bool[8] { false,false,false,false,false,false,false,false};
                bool[] dtResult = new bool[8] { false, false, false, false, false, false, false, false};

                HTuple row0, col0;
                HTuple Row0 = new HTuple();
                HTuple Col0 = new HTuple();
                HObject[] hoRegion=new HObject[8];
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2, 
                                               pointReault[8].Row1, pointReault[8].Colum1, pointReault[8].Row2, pointReault[8].Colum2, 
                                               out row0, out col0, out IsOverlapping);
                Row0[0] = row0;
                Col0[0] = col0;

                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                               pointReault[8].Row1, pointReault[8].Colum1, pointReault[8].Row2, pointReault[8].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[1] = row0;
                Col0[1] = col0;
                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                               pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[2] = row0;
                Col0[2] = col0;
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                                               pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[3] = row0;
                Col0[3] = col0;

                HOperatorSet.GenRegionPolygonFilled(out hoRegion[0], Row0, Col0);

                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                                               pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[0] = row0;
                Col0[0] = col0;

                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                               pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[1] = row0;
                Col0[1] = col0;
                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                               pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[2] = row0;
                Col0[2] = col0;
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                                              pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2,
                                              out row0, out col0, out IsOverlapping);
                Row0[3] = row0;
                Col0[3] = col0;

                HOperatorSet.GenRegionPolygonFilled(out hoRegion[1], Row0, Col0);

                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                                              pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2,
                                              out row0, out col0, out IsOverlapping);
                Row0[0] = row0;
                Col0[0] = col0;

                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                               pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[1] = row0;
                Col0[1] = col0;
                HOperatorSet.IntersectionLines(pointReault[9].Row1, pointReault[9].Colum1, pointReault[9].Row2, pointReault[9].Colum2,
                                              pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                                              out row0, out col0, out IsOverlapping);
                Row0[2] = row0;
                Col0[2] = col0;
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                                               pointReault[9].Row1, pointReault[9].Colum1, pointReault[9].Row2, pointReault[9].Colum2,
                                               out row0, out col0, out IsOverlapping);
                Row0[3] = row0;
                Col0[3] = col0;

                HOperatorSet.GenRegionPolygonFilled(out hoRegion[2], Row0, Col0);

                Parameter.specificationsCam2[i].矩形检测区域[0].Row1 = pointReault[8].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[0].Colum1 = pointReault[2].Colum1;
                Parameter.specificationsCam2[i].矩形检测区域[0].Row2 = pointReault[4].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[0].Colum2 = pointReault[3].Colum1;

                Parameter.specificationsCam2[i].矩形检测区域[1].Row1 = pointReault[4].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[1].Colum1 = pointReault[2].Colum1;
                Parameter.specificationsCam2[i].矩形检测区域[1].Row2 = pointReault[5].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[1].Colum2 = pointReault[3].Colum1;

                Parameter.specificationsCam2[i].矩形检测区域[2].Row1 = pointReault[5].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[2].Colum1 = pointReault[2].Colum1;
                Parameter.specificationsCam2[i].矩形检测区域[2].Row2 = pointReault[9].Row1;
                Parameter.specificationsCam2[i].矩形检测区域[2].Colum2 = pointReault[3].Colum1;

                for (int index = 3; index < 8; index++)
                {
                    try
                    {
                        HOperatorSet.GenRectangle1(out hoRegion[index], Parameter.specificationsCam2[i].矩形检测区域[index].Row1, Parameter.specificationsCam2[i].矩形检测区域[index].Colum1, Parameter.specificationsCam2[i].矩形检测区域[index].Row2, Parameter.specificationsCam2[i].矩形检测区域[index].Colum2);
                    }
                    catch
                    {
                        return false;
                    }

                }
                for (int index = 0; index < 8; index++)
                {
                    try
                    {
                        HOperatorSet.SetColor(hWindow, "blue");
                        HOperatorSet.SetDraw(hWindow, "margin");
                        HOperatorSet.DispObj(hoRegion[index],hWindow);
                        HOperatorSet.SetTposition(hWindow, Parameter.specificationsCam2[i].矩形检测区域[index].Row1, Parameter.specificationsCam2[i].矩形检测区域[index].Colum1);
                        HOperatorSet.WriteString(hWindow, index + 1);
                        Halcon.DetectionHalconRect1(i, index, hWindow, ImageAffineTrans, hoRegion[index], ref grayResult[index]);
                        Halcon.DetectionHalconRect2(i, index, hWindow, ImageAffineTrans, hoRegion[index], ref dtResult[index]);
                    }
                    catch
                    {
                        return false;
                    }

                }

                bool Result = true;
                if (Parameter.specificationsCam2[i].倒角检测[0])
                {
                    HTuple Row1, Column1, IsOverlapping1, area, row, column;
                    HObject rect, ho_imagereduced, ho_image, ho_SelectShape, ho_MeanImage;
                    HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                        pointReault[8].Row1, pointReault[8].Colum1, pointReault[8].Row2, pointReault[8].Colum2, out Row1, out Column1, out IsOverlapping1);
                    HOperatorSet.GenRectangle1(out rect, Row1, Column1, Row1 + 100, Column1 + 100);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.SetDraw(hWindow, "margin");
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.Threshold(ho_imagereduced, out rect, 100, 255);
                    HOperatorSet.AreaCenter(rect, out area, out row, out column);
                    if (area.D > 9800)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        Result = false;
                    }
                    HOperatorSet.DispObj(rect, hWindow);
                    HOperatorSet.DilationCircle(rect, out ho_imagereduced,5);
                    HOperatorSet.ErosionCircle(ho_imagereduced,out rect, 10);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.MeanImage(ho_imagereduced,out ho_MeanImage, 15,15);

                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image,25,"dark");
                    HOperatorSet.Connection(ho_image,out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape,"area","and",10,5000);
                    if(ho_SelectShape.CountObj()>0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "light");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    rect.Dispose(); ho_imagereduced.Dispose(); ho_image.Dispose(); ho_SelectShape.Dispose(); ho_MeanImage.Dispose();

                    Row1.Dispose();
                    Column1.Dispose();
                    IsOverlapping1.Dispose();
                    area.Dispose();
                    row.Dispose();
                    column.Dispose();
                }
                if (Parameter.specificationsCam2[i].倒角检测[1])
                {
                    HTuple Row1, Column1, IsOverlapping1, area, row, column;
                    HObject rect, ho_imagereduced, ho_image, ho_SelectShape, ho_MeanImage;
                    HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                        pointReault[8].Row1, pointReault[8].Colum1, pointReault[8].Row2, pointReault[8].Colum2, out Row1, out Column1, out IsOverlapping1);
                    HOperatorSet.GenRectangle1(out rect, Row1, Column1 - 100, Row1 + 100, Column1);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.SetDraw(hWindow, "margin");
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.Threshold(ho_imagereduced, out rect, 50, 255);
                    HOperatorSet.AreaCenter(rect, out area, out row, out column);
                    if (area.D > 9800)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        Result = false;
                    }
                    HOperatorSet.DispObj(rect, hWindow);
                    HOperatorSet.DilationCircle(rect, out ho_imagereduced, 5);
                    HOperatorSet.ErosionCircle(ho_imagereduced, out rect, 10);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.MeanImage(ho_imagereduced, out ho_MeanImage, 15, 15);
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "dark");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "light");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    rect.Dispose(); ho_imagereduced.Dispose(); ho_image.Dispose(); ho_SelectShape.Dispose(); ho_MeanImage.Dispose();

                    Row1.Dispose();
                    Column1.Dispose();
                    IsOverlapping1.Dispose();
                    area.Dispose();
                    row.Dispose();
                    column.Dispose();
                }
                if (Parameter.specificationsCam2[i].倒角检测[2])
                {
                    HTuple Row1, Column1, IsOverlapping1, area, row, column;
                    HObject rect, ho_imagereduced, ho_image, ho_SelectShape, ho_MeanImage;
                    HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                        pointReault[9].Row1, pointReault[9].Colum1, pointReault[9].Row2, pointReault[9].Colum2, out Row1, out Column1, out IsOverlapping1);
                    HOperatorSet.GenRectangle1(out rect, Row1 - 100, Column1, Row1, Column1 + 100);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.SetDraw(hWindow, "margin");
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.Threshold(ho_imagereduced, out rect, 50, 255);
                    HOperatorSet.AreaCenter(rect, out area, out row, out column);
                    if (area.D > 9800)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        Result = false;
                    }
                    HOperatorSet.DispObj(rect, hWindow);
                    HOperatorSet.DilationCircle(rect, out ho_imagereduced, 5);
                    HOperatorSet.ErosionCircle(ho_imagereduced, out rect, 10);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.MeanImage(ho_imagereduced, out ho_MeanImage, 15, 15);
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "dark");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "light");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    rect.Dispose(); ho_imagereduced.Dispose(); ho_image.Dispose(); ho_SelectShape.Dispose(); ho_MeanImage.Dispose();
                    Row1.Dispose();
                    Column1.Dispose();
                    IsOverlapping1.Dispose();
                    area.Dispose();
                    row.Dispose();
                    column.Dispose();

                }
                if (Parameter.specificationsCam2[i].倒角检测[3])
                {
                    HTuple Row1, Column1, IsOverlapping1, area, row, column;
                    HObject rect, ho_imagereduced, ho_image, ho_SelectShape, ho_MeanImage;
                    HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                        pointReault[9].Row1, pointReault[9].Colum1, pointReault[9].Row2, pointReault[9].Colum2, out Row1, out Column1, out IsOverlapping1);
                    HOperatorSet.GenRectangle1(out rect, Row1 - 100, Column1 - 100, Row1, Column1);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.SetDraw(hWindow, "margin");
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.Threshold(ho_imagereduced, out rect, 50, 255);
                    HOperatorSet.AreaCenter(rect, out area, out row, out column);
                    if (area.D > 9800)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        Result = false;
                    }
                    HOperatorSet.DispObj(rect, hWindow);
                    HOperatorSet.DilationCircle(rect, out ho_imagereduced, 5);
                    HOperatorSet.ErosionCircle(ho_imagereduced, out rect, 10);
                    HOperatorSet.ReduceDomain(ImageAffineTrans, rect, out ho_imagereduced);
                    HOperatorSet.MeanImage(ho_imagereduced, out ho_MeanImage, 15, 15);
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "dark");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    HOperatorSet.DynThreshold(ho_MeanImage, ho_imagereduced, out ho_image, 25, "light");
                    HOperatorSet.Connection(ho_image, out rect);
                    HOperatorSet.SelectShape(rect, out ho_SelectShape, "area", "and", 10, 5000);
                    if (ho_SelectShape.CountObj() > 0)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        HOperatorSet.DispObj(ho_SelectShape, hWindow);
                        Result = false;
                    }
                    rect.Dispose(); ho_imagereduced.Dispose(); ho_image.Dispose(); ho_SelectShape.Dispose(); ho_MeanImage.Dispose();

                    Row1.Dispose();
                    Column1.Dispose();
                    IsOverlapping1.Dispose();
                    area.Dispose();
                    row.Dispose();
                    column.Dispose();
                }
                Halcon.DetectionHalconDeepLearning1(i, hWindow, ImageAffineTrans,hoRegion[1], hv_DLModelHandle[i], hv_DLPreprocessParam[i], hv_InferenceClassificationThreshold[i], hv_InferenceSegmentationThreshold[i], rect1, ref dplResult);
               
                if (Result && dplResult)   //倒角检测结果
                {
                    //result[0] = true;
                }
                else
                {
                    result[0] = false;
                }
                if (grayResult[1] && dtResult[1]/*&& dplResult*/)
                {
                    //result[1] = true;
                }
                else
                {
                    result[1] = false;
                }

                if (grayResult[2] && dtResult[2]/*&& dplResult*/)
                {
                    //result[2] = true;
                }
                else
                {
                    result[2] = false;
                }

               
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                HTuple minDistance, maxDistance;
                for (int index = 0; index < 8; index++)
                {
                    if (grayResult[index] && dtResult[index])
                    {

                    }
                    else
                    {
                        result[0] = false;
                    }
                }
                for (int index = 0; index <7; index++)
                {
                   
                    HOperatorSet.DistanceSs(pointReault[index * 2].Row1, pointReault[index * 2].Colum1, pointReault[index * 2].Row2, pointReault[index * 2].Colum2,
				    pointReault[index * 2 + 1].Row1, pointReault[index * 2 + 1].Colum1, pointReault[index * 2 + 1].Row2, pointReault[index * 2 + 1].Colum2, out minDistance, out maxDistance);
                    value[index] = minDistance * Parameter.specificationsCam2[i].PixelResolution + Parameter.specificationsCam2[i].检测规格[index].adjust;
                    if (value[index] - Parameter.specificationsCam2[0].检测规格[index].value < Parameter.specificationsCam2[0].检测规格[index].min ||
                        value[index] - Parameter.specificationsCam2[0].检测规格[index].value > Parameter.specificationsCam2[0].检测规格[index].max)
                    {
                        HOperatorSet.SetColor(hWindow, "red");
                        result[3+index] = false;
                    }
                    else
                    {
                        result[3+index] = true;
                        HOperatorSet.SetColor(hWindow, "green");
                    }
                    HOperatorSet.SetTposition(hWindow, 100 + index * 100, 100);
                    HOperatorSet.WriteString(hWindow, MainForm.DName[index] +  value[index]); 
                    
					minDistance.Dispose();
					maxDistance.Dispose();
				}
                

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
            catch (Exception ex)
            {
                return false;
            }

        }

        private void num_PixelResolution_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].PixelResolution= value;
        }

        private void uiButton20_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 0;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton21_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 1;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton22_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = 2;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton23_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = uiComboBox3.SelectedIndex * 2+3;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

        private void uiButton24_Click(object sender, EventArgs e)
        {
            MainForm.LineIndex = uiComboBox3.SelectedIndex * 2+ 4;
            直线工具属性 flg = new 直线工具属性();
            flg.ShowDialog();
        }

		private void uiDoubleUpDown1_ValueChanged(object sender, double value)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].DeepLearningRate = value;
        }

        private void chk_SaveDefeatImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].SaveDefeatImage = chk_SaveDefeatImage.Checked;
        }

        private void chk_SaveOrigalImage_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].SaveOrigalImage = chk_SaveOrigalImage.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[0] = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[1] = checkBox1.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[2] = checkBox1.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Parameter.specificationsCam2[uiComboBox1.SelectedIndex].倒角检测[3] = checkBox1.Checked;
        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            HTuple row, col, grayval;
            row = (int)e.Y;
            col = (int)e.X;
            HOperatorSet.SetColor(hWindowControl1.HalconWindow,"white");
            HOperatorSet.GetGrayval(MainForm.hImage2[uiComboBox1.SelectedIndex], row, col, out grayval);
            HOperatorSet.SetTposition(hWindowControl1.HalconWindow ,row, col);
            HOperatorSet.WriteString(hWindowControl1.HalconWindow, "Gray:" + grayval);
            row.Dispose();
            col.Dispose(); 
            grayval.Dispose();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox5.Checked)
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
