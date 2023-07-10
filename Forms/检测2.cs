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
using static WY_App.Utility.Parameters;
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
            XMLHelper.serialize<Parameters.SpecificationsCam2>(Parameters.specificationsCam2[uiComboBox1.SelectedIndex], Parameters.commministion.productName + "/Cam2Specifications" + uiComboBox1.SelectedIndex + ".xml");
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
            num_AreaHigh.Value = Parameters.specificationsCam2[0].AreaHigh[0];
            num_AreaLow.Value = Parameters.specificationsCam2[0].AreaLow[0];
            num_ThresholdHigh.Value = Parameters.specificationsCam2[0].ThresholdHigh[0];
            num_ThresholdLow.Value = Parameters.specificationsCam2[0].ThresholdLow[0];
            num_PixelResolution.Value = Parameters.specificationsCam2[0].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameters.specificationsCam2[0].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameters.specificationsCam2[0].SaveDefeatImage;
        }

        private void uiComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            num_AreaHigh.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex];
            num_AreaLow.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex];
            num_ThresholdHigh.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex];
            num_ThresholdLow.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex];
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiComboBox2.SelectedIndex = 0;
            MainForm.formloadIndex = uiComboBox1.SelectedIndex + 5;

            uiComboBox2_SelectedIndexChanged( sender,  e);
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);//设置窗体的规格 
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);

            num_AreaHigh.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex];
            num_AreaLow.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex];
            num_ThresholdHigh.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex];
            num_ThresholdLow.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex];
            num_PixelResolution.Value = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].PixelResolution;
            chk_SaveOrigalImage.Checked = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].SaveOrigalImage;
            chk_SaveDefeatImage.Checked = Parameters.specificationsCam2[uiComboBox1.SelectedIndex].SaveDefeatImage;

        }

        private void uiButton14_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[0]);
        }

        private void uiButton16_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[1]);
        }

        private void uiButton15_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[2]);
        }

        private void uiButton18_Click(object sender, EventArgs e)
        {
            HObject hImage = new HObject();
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            HOperatorSet.Threshold(MainForm.hImage2[uiComboBox1.SelectedIndex], out hImage, 16, 255);
           // HOperatorSet.DispObj(hImage, hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex+2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[0],  ref BaseReault[0]);
            hImage.Dispose();

        }

        private void uiButton17_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex+2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[1],  ref BaseReault[1]);
        }

        private void uiButton19_Click(object sender, EventArgs e)
        {
            HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex + 2, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[2], ref BaseReault[2]);
        }

        private void uiButton46_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HTuple Row, Column, IsOverlapping;
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn = Column;
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
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow, -Column + Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(MainForm.hImage2[uiComboBox1.SelectedIndex], out MainForm.hImage2[uiComboBox1.SelectedIndex], HomMat2DRotate, "constant", "false");

                HOperatorSet.DispObj(MainForm.hImage2[uiComboBox1.SelectedIndex], hWindowControl1.HalconWindow);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[0], ref BaseReault[0]);

                Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].基准[1], ref BaseReault[1]);

                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseRow = Row;
                Parameters.specificationsCam2[uiComboBox1.SelectedIndex].BaseColumn = Column;
                HOperatorSet.DispCross(hWindowControl1.HalconWindow, Row, Column, 60, 0);
                Row.Dispose();
                Column.Dispose();
                IsOverlapping.Dispose();



                XMLHelper.serialize<Parameters.SpecificationsCam2>(Parameters.specificationsCam2[uiComboBox1.SelectedIndex], Parameters.commministion.productName + "/Cam2Specifications" + uiComboBox1.SelectedIndex + ".xml");
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
                Halcon.DetectionHalconRect1(uiComboBox1.SelectedIndex, uiComboBox2.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex], ref result);
                bool dtResult = false;
                Halcon.DetectionHalconRect2(uiComboBox1.SelectedIndex, uiComboBox2.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex], ref dtResult);
            }
            catch { }
            
        }

        private void uiButton29_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawRectAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].矩形检测区域[uiComboBox2.SelectedIndex]);
        }

        private void num_ThresholdLow_ValueChanged(object sender, double value)
        {
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdLow[uiComboBox2.SelectedIndex] =value;
        }

        private void num_ThresholdHigh_ValueChanged(object sender, double value)
        {
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].ThresholdHigh[uiComboBox2.SelectedIndex] = value;
        }

        private void num_AreaLow_ValueChanged(object sender, double value)
        {
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaLow[uiComboBox2.SelectedIndex] = value;
        }

        private void num_AreaHigh_ValueChanged(object sender, double value)
        {
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].AreaHigh[uiComboBox2.SelectedIndex] = value;
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex*2]);
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2+1]);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[2]);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[3]);
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[4]);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[5]);
        }

		private void uiButton30_Click(object sender, EventArgs e)//总宽直线拟合1
		{
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[6]);
        }

		private void uiButton33_Click(object sender, EventArgs e)//总宽直线拟合2
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[7]);
		}
		private void uiButton36_Click(object sender, EventArgs e)//左短端直线拟合1
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[8]);
        }
		private void uiButton37_Click(object sender, EventArgs e)//左短端直线拟合2
		{
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[9]);
        }
		private void uiButton42_Click(object sender, EventArgs e)//右短端直线拟合1
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[10]);
        }

		private void uiButton43_Click(object sender, EventArgs e)//右短端直线拟合2
        {
            Halcon.DetectionDrawLineAOI(hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], ref Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[11]);
        }


		private void uiButton10_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2],  ref pointReault[uiComboBox3.SelectedIndex * 2]);
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            Halcon.DetectionHalconLine(uiComboBox1.SelectedIndex, hWindowControl1.HalconWindow, MainForm.hImage2[uiComboBox1.SelectedIndex], Parameters.specificationsCam2[uiComboBox1.SelectedIndex].模板区域[uiComboBox3.SelectedIndex * 2+1], ref pointReault[uiComboBox3.SelectedIndex * 2+1]);
        }

		private void uiButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
                               //Halcon.DetectionHalcon(hWindowControl1.HalconWindow, 主窗体.hImage,ref 主窗体.result);
                               //Halcon.DetectionHalconDeepLearning(hWindowControl1.HalconWindow, 主窗体.hImage, 0, hv_DLModelHandle, hv_DLPreprocessParam, hv_InferenceClassificationThreshold, hv_InferenceSegmentationThreshold, point[1], point[2], point[4]);
            bool[] result = new bool[10];
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
        public static void Detection(int i, HWindow hWindow, HObject hImage, ref bool[] result, ref double[] value)
        {
            try
            {
                bool[] resultvalue = new bool[20];
                System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码运行时间
                                   //HOperatorSet.DispObj(hImage, hWindow);
                
                Halcon.DetectionHalconLine(i, hWindow, hImage, Parameters.specificationsCam2[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i, hWindow, hImage, Parameters.specificationsCam2[i].基准[1], ref BaseReault[1]);

                HTuple angle, Row, Column, IsOverlapping;
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
                HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, -Row + Parameters.specificationsCam2[i].BaseRow, -Column + Parameters.specificationsCam2[i].BaseColumn, out HomMat2DRotate);
                HOperatorSet.AffineTransImage(ImageAffineTran, out MainForm.hImage2[i], HomMat2DRotate, "constant", "false");
                HOperatorSet.AffineTransImage(ImageAffineTran, out ImageAffineTrans, HomMat2DRotate, "constant", "false");
                HOperatorSet.DispObj(ImageAffineTrans, hWindow);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameters.specificationsCam2[i].基准[0], ref BaseReault[0]);
                Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameters.specificationsCam2[i].基准[1], ref BaseReault[1]);
                for(int index=0; index < 14; index++)
				{
                    resultvalue[index] = Halcon.DetectionHalconLine(i, hWindow, ImageAffineTrans, Parameters.specificationsCam2[i].模板区域[index], ref pointReault[index]);
                }
                Rect1 rect1 = new Rect1();
                HOperatorSet.IntersectionLines(pointReault[2].Row1, pointReault[2].Colum1, pointReault[2].Row2, pointReault[2].Colum2,
                    pointReault[4].Row1, pointReault[4].Colum1, pointReault[4].Row2, pointReault[4].Colum2, out Row, out Column, out IsOverlapping);
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                rect1.Row1 = Row + 30;
                rect1.Colum1 = Column + 30;
                Parameters.specificationsCam2[i].矩形检测区域[1].Row1 = Row + 30;
                Parameters.specificationsCam2[i].矩形检测区域[1].Colum1 = Column + 30;
                HOperatorSet.IntersectionLines(pointReault[3].Row1, pointReault[3].Colum1, pointReault[3].Row2, pointReault[3].Colum2,
                    pointReault[5].Row1, pointReault[5].Colum1, pointReault[5].Row2, pointReault[5].Colum2, out Row, out Column, out IsOverlapping);
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                bool dplResult = true;

                rect1.Row2 = Row - 30;
                rect1.Colum2 = Column - 30;
                Parameters.specificationsCam2[i].矩形检测区域[1].Row2 = Row - 30;
                Parameters.specificationsCam2[i].矩形检测区域[1].Colum2 = Column - 30;
                bool[] grayResult = new bool[8] { false,false,false,false,false,false,false,false};
                bool[] dtResult = new bool[8] { false, false, false, false, false, false, false, false };
                for(int index=0;index <8;index ++)
                {
                    try
                    {
                        HOperatorSet.SetTposition(hWindow, Parameters.specificationsCam2[i].矩形检测区域[index].Row1, Parameters.specificationsCam2[i].矩形检测区域[index].Colum1);
                        HOperatorSet.WriteString(hWindow, index + 1);
                        Halcon.DetectionHalconRect1(i, index, hWindow, ImageAffineTrans, Parameters.specificationsCam2[i].矩形检测区域[index], ref grayResult[index]);
                        Halcon.DetectionHalconRect2(i, index, hWindow, ImageAffineTrans, Parameters.specificationsCam2[i].矩形检测区域[index], ref dtResult[index]);
                    }
                    catch
                    {

                    }
                    
                }
                Halcon.DetectionHalconDeepLearning1(i, hWindow, ImageAffineTrans, hv_DLModelHandle[i], hv_DLPreprocessParam[i], hv_InferenceClassificationThreshold[i], hv_InferenceSegmentationThreshold[i], rect1, ref dplResult);
                if (grayResult[0] && dtResult[0]/*&& dplResult*/)
                {
                    result[0] = true;
                }
                else
                {
                    result[0] = false;
                }
                if (grayResult[1] && dtResult[1]/*&& dplResult*/)
                {
                    result[1] = true;
                }
                else
                {
                    result[1] = false;
                }

                if (grayResult[2] && dtResult[2]/*&& dplResult*/)
                {
                    result[2] = true;
                }
                else
                {
                    result[2] = false;
                }
                for (int index = 0; index < 5; index++)
                {
                    if (grayResult[index+3] && dtResult[index+3]/*&& dplResult*/)
                    {
                        result[0] = true;
                    }
                }
                HOperatorSet.IntersectionLines(BaseReault[0].Row1, BaseReault[0].Colum1, BaseReault[0].Row2, BaseReault[0].Colum2,
                    BaseReault[1].Row1, BaseReault[1].Colum1, BaseReault[1].Row2, BaseReault[1].Colum2, out Row, out Column, out IsOverlapping);
                
                HOperatorSet.DispCross(hWindow, Row, Column, 60, 0);
                HTuple minDistance, maxDistance;
                for (int index = 0; index <7; index++)
                {
					HOperatorSet.DistanceSs(pointReault[index * 2].Row1, pointReault[index * 2].Colum1, pointReault[index * 2].Row2, pointReault[index * 2].Colum2,
						  pointReault[index * 2 + 1].Row1, pointReault[index * 2 + 1].Colum1, pointReault[index * 2 + 1].Row2, pointReault[index * 2 + 1].Colum2, out minDistance, out maxDistance);
					value[index] = minDistance * Parameters.specificationsCam2[i].PixelResolution + Parameters.specificationsCam2[i].检测规格[index].adjust;
                    if (value[index] - Parameters.specificationsCam2[0].检测规格[index].value < Parameters.specificationsCam2[0].检测规格[index].min ||
                        value[index] - Parameters.specificationsCam2[0].检测规格[index].value > Parameters.specificationsCam2[0].检测规格[index].max)
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
            }
            catch
            {

            }

        }

        private void num_PixelResolution_ValueChanged(object sender, double value)
        {
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].PixelResolution= value;
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
            Parameters.specificationsCam2[uiComboBox1.SelectedIndex].DeepLearningRate = value;
        }
	}
}
