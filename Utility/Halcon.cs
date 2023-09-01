using HalconDotNet;
using SevenZip.Compression.LZ;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WY_App;
using static WY_App.Utility.Parameter;

namespace WY_App.Utility
{
    public class Halcon
    {
        public static HTuple[] hv_Width = new HTuple[4];
        public static HTuple[] hv_Height = new HTuple[4];
        public static HTuple hv_AcqHandle1 = new HTuple();
        public static HTuple hv_AcqHandle0 = new HTuple();
        public static bool Cam1Connect = false;
        public static bool Cam2Connect = false;
        public static bool initalCamera(string CamID, ref HTuple hv_AcqHandle)
        {
            try
            {
                //获取相机句柄
                hv_AcqHandle.Dispose();
                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "progressive",-1, "default", -1, "false", "default", CamID, 0, -1, out hv_AcqHandle);
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "AcquisitionMode", "SingleFrame");
                HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
       
        public Halcon()
        {

                Thread th = new Thread(ini_Cam);
                th.IsBackground = true;
                th.Start();
            
        }

        private void ini_Cam()
        {
            while (true)
            {
                Thread.Sleep(5000);
                while(!Cam1Connect)
                {
                    Thread.Sleep(5000);
                    if (!Cam1Connect)
                    {
                        Cam1Connect = initalCamera("CAM0", ref hv_AcqHandle0);
                        LogHelper.Log.WriteError(System.DateTime.Now.ToString() + "相机1链接失败");
                        MainForm.AlarmList.Add(System.DateTime.Now.ToString() + "相机1链接失败");
                    }
                    else
                    {
                        LogHelper.Log.WriteError(System.DateTime.Now.ToString() + "相机1链接成功");
                        MainForm.AlarmList.Add(System.DateTime.Now.ToString() + "相机1链接成功");
                    }
                }              
                while (!Cam2Connect)
                {
                    Thread.Sleep(5000);
                    if (!Cam2Connect)
                    {
                        Cam2Connect = initalCamera("CAM1", ref hv_AcqHandle1);
                        LogHelper.Log.WriteError(System.DateTime.Now.ToString() + "相机2链接失败");
                        MainForm.AlarmList.Add(System.DateTime.Now.ToString() + "相机2链接失败");
                    }
                    else
                    {

                        LogHelper.Log.WriteError(System.DateTime.Now.ToString() + "相机2链接成功");
                        MainForm.AlarmList.Add(System.DateTime.Now.ToString() + "相机2链接成功");
                    }
                }
            }
        }

        public static bool GrabImage(HTuple hv_AcqHandle, out HObject ho_Image)
        {
            try
            {
                HOperatorSet.GrabImage(out ho_Image, hv_AcqHandle);
                return true;
            }
            catch
            {
                ho_Image=null;
                return false;
            }
        }

        public static bool GrabImageLive(HWindow hWindow, HTuple hv_AcqHandle, out HObject ho_Image)
        {
            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
            while (true)
            {
                HOperatorSet.GrabImageAsync(out ho_Image, hv_AcqHandle, -1);
                hWindow.DispObj(ho_Image);
            }    
        }
        public static bool StopImage(HTuple hv_AcqHandle)
        {
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
            return true;
        }
        public static bool ImgDisplay2(int i, string imgPath, HTuple Hwindow)
        {
            HOperatorSet.GenEmptyObj(out MainForm.hImage2[i]);
            HOperatorSet.SetPart(Hwindow, 0, 0, -1, -1);//设置窗体的规格
            HOperatorSet.ReadImage(out MainForm.hImage2[i], imgPath);//读取图片存入到HalconImage           
            HOperatorSet.GetImageSize(MainForm.hImage2[i], out hv_Width[i], out hv_Height[i]);//获取图片大小规格
            HOperatorSet.DispObj(MainForm.hImage2[i], Hwindow);//显示图片
            return true;
        }

        public static bool ImgDisplay(int i, string imgPath, HTuple Hwindow)
        {
            HOperatorSet.GenEmptyObj(out MainForm.hImage[i]);
            HOperatorSet.SetPart(Hwindow, 0, 0, -1, -1);//设置窗体的规格
            HOperatorSet.ReadImage(out MainForm.hImage[i], imgPath);//读取图片存入到HalconImage           
            HOperatorSet.GetImageSize(MainForm.hImage[i], out hv_Width[i], out hv_Height[i]);//获取图片大小规格
            HOperatorSet.DispObj(MainForm.hImage[i], Hwindow);//显示图片
            return true;
        }
        //        //-----------------------------------------------------------------------------
        public static void CloseFramegrabber(HTuple hv_AcqHandle)
        {
            HOperatorSet.CloseFramegrabber(hv_AcqHandle);
        }
        public static void TriggerModeOff(HTuple hv_AcqHandle)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "Off");
        }
        public static void TriggerModeOn(HTuple hv_AcqHandle)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "TriggerMode", "On");
        }
        public static void SetFramegrabberParam(HTuple hv_AcqHandle)
        {
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "gain", Parameter.cameraParam.Gain[0]);
            HOperatorSet.SetFramegrabberParam(hv_AcqHandle, "ExposureTime", Parameter.cameraParam.Shutter[0]);

        }
        public static void GrabImageAsync(HTuple hv_AcqHandle, out HObject himage)
        {
            HOperatorSet.GrabImageAsync(out himage, hv_AcqHandle, -1);
        }
        public static void GrabImageStart(HTuple hv_AcqHandle)
        {
            HOperatorSet.GrabImageStart(hv_AcqHandle, -1);
        }

        public static void DetectionDrawRectAOI(HWindow hWindow, HObject hImage, ref Parameter.Rect1 rect1)
        {
            // 初始化本地和输出图片变量
            //hWindowControl1.HalconWindow.DispObj(Himage);//显示图像、Region、Xld
            //设置显示对象颜色
            HOperatorSet.SetColor(hWindow, "green");
            //设置区域填充模式为margin填充边缘
            HOperatorSet.SetDraw(hWindow, "margin");
            hWindow.DispObj(hImage);

            //        vector_angle_to_rigid(0, 0, 0, Row3, Column3, Angle, HomMat2D)
            //affine_trans_contour_xld(ModelContours, ContoursAffinTrans, HomMat2D)
            //affine_trans_region(Rectangle1, RegionAffineTrans, HomMat2D, 'false')

            //        HOperatorSet.VectorAngleToRigid(0, 0, 0, ModelRow, ModelColumn, ModelAngle, HomMat2D);
            //HObject rgion;
            HObject Rectangle1;
            //HOperatorSet.DrawPolygon(out PolygonRegion, hWind);
            //HOperatorSet.DrawRegion(out Region, hWindowControl1.HalconWindow);
            hWindow.DrawRectangle1(out rect1.Row1, out rect1.Colum1, out rect1.Row2, out rect1.Colum2);
            HOperatorSet.GenRectangle1(out Rectangle1, rect1.Row1, rect1.Colum1, rect1.Row2, rect1.Colum2);

            //将'C:/Users/aa/Desktop/region.hobj'路劲的区域的读取到Region1
            //HOperatorSet.ReadRegion(out Region, Application.StartupPath + "/halcon/ClosedContours.tiff");
            //HOperatorSet.GenContoursSkeletonXld(Region, out Contours, 1, "filter");
            //HOperatorSet.UnionAdjacentContoursXld(Contours,out UnionContours, 10, 1, "attr_keep");
            //HOperatorSet.CloseContoursXld(UnionContours,out ClosedContours);
            HOperatorSet.DispObj(Rectangle1, hWindow);
            //try
            //{
            //    if (!path.Equals(""))
            //    {
            //        //将Region区域的保存到路劲为'C:/Users/aa/Desktop/region.hobj'的文件里
            //        HOperatorSet.WriteRegion(Rectangle1, path + "ClosedContours.tiff");

            //        //将'C:/Users/aa/Desktop/region.hobj'路劲的区域的读取到Region1
            //        HOperatorSet.ReadRegion(out HObject Region, path + "ClosedContours.tiff");
            //        //HOperatorSet.WriteImage(ClosedContours1, "tiff", 0, path + "02.tiff");                  
            //        MessageBox.Show("检测区域保存成功");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("图片保存失败" + ex.Message);
            //}
            Rectangle1.Dispose();
        }
        public static void DetectionDrawRect2AOI(HWindow hWindow, HObject hImage, ref Parameter.Rect2 rect2)
        {
            HOperatorSet.SetColor(hWindow, "green");
            HOperatorSet.SetDraw(hWindow, "margin");
            hWindow.DispObj(hImage);
            hWindow.DrawRectangle2(out rect2.Row, out rect2.Colum, out rect2.Phi, out rect2.Length1, out rect2.Length2);

        }
        public static void DetectionDrawLineAOI(HWindow hWindow, HObject hImage, ref Parameter.Rect1 rect1)
        {
            HOperatorSet.SetColor(hWindow, "green");
            HOperatorSet.SetDraw(hWindow, "margin");
            hWindow.DispObj(hImage);
            hWindow.DrawLine(out rect1.Row1, out rect1.Colum1, out rect1.Row2, out rect1.Colum2);
            HOperatorSet.DispLine(hWindow, rect1.Row1, rect1.Colum1, rect1.Row2, rect1.Colum2);

        }


        /// <summary>
        /// 直线卡尺工具
        /// </summary>
        /// <param name="hWindow"></param>
        /// <param name="hImage"></param>
        /// <param name="rect1"></param>
        /// <param name="PointXY"></param>
        /// <returns></returns>
        public static bool DetectionHalconLine(int i,HWindow hWindow, HObject hImage, Parameter.Rect1 rect1, ref HRect1 PointXY)
            {
            HObject ho_Contours, ho_Cross, ho_Contour;

            // Local control variables 

            HTuple hv_shapeParam = new HTuple();
            HTuple hv_MetrologyHandle = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Parameter = new HTuple();

            HTuple hv_Nc = new HTuple(), hv_Dist = new HTuple(), hv_Nr = new HTuple();
            try
            {
                HOperatorSet.SetLineWidth(hWindow, 1);
                //HOperatorSet.DispObj(hImage, hWindow);
                //标记测量位置         

                //HOperatorSet.CropPart(hImage, out ho_Rectangle, Parameter.specifications.矩形检测区域.Row1, Parameter.specifications.矩形检测区域.Colum1, Parameter.specifications.矩形检测区域.Colum2 - Parameter.specifications.矩形检测区域.Colum1, Parameter.specifications.矩形检测区域.Row2 - Parameter.specifications.矩形检测区域.Row1);

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_shapeParam = new HTuple();
                    hv_shapeParam = hv_shapeParam.TupleConcat(rect1.Row1, rect1.Colum1, rect1.Row2, rect1.Colum2);
                }
                //创建测量句柄
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                //添加测量对象
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width[i], hv_Height[i]);
                hv_Index.Dispose();
                HOperatorSet.AddMetrologyObjectGeneric(hv_MetrologyHandle, "line", hv_shapeParam, rect1.LineLength, 3, rect1.simga, rect1.阈值, new HTuple(), new HTuple(), out hv_Index);

                //执行测量，获取边缘点集
                HOperatorSet.SetColor(hWindow, "yellow");
                HOperatorSet.ApplyMetrologyModel(hImage, hv_MetrologyHandle);
                hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, hv_MetrologyHandle, "all", "all", out hv_Row, out hv_Column);
                if(MainForm.isTestMode)
                {
                    HOperatorSet.DispObj(ho_Contours, hWindow);
                }            
                HOperatorSet.SetColor(hWindow, "red");
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 6, 0.785398);
                //获取最终测量数据和轮廓线
                HOperatorSet.SetColor(hWindow, "green");
                HOperatorSet.SetLineWidth(hWindow, 1);
                hv_Parameter.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type", "all_param", out hv_Parameter);
                HOperatorSet.GetMetrologyObjectResultContour(out ho_Contour, hv_MetrologyHandle, "all", "all", 1.5);
                HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out PointXY.Row1, out PointXY.Colum1, out PointXY.Row2, out PointXY.Colum2, out hv_Nr, out hv_Nc, out hv_Dist);

                if (MainForm.isTestMode)
                {
                    HOperatorSet.DispObj(ho_Cross, hWindow);
                }

                HOperatorSet.SetColor(hWindow, "blue");
                HOperatorSet.DispObj(ho_Contour, hWindow);
                if (PointXY.Row1.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle); ;
                ho_Contours.Dispose();
                ho_Cross.Dispose();
                ho_Contour.Dispose();
                hv_shapeParam.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_Index.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
            }
            catch 
            {
                HOperatorSet.ClearMetrologyModel(hv_MetrologyHandle); 
                hv_shapeParam.Dispose();
                hv_MetrologyHandle.Dispose();
                hv_Index.Dispose();
                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Parameter.Dispose();
                return false;
            }
            
            // Initialize local and output iconic variables 

            
            //释放测量句柄
            
        }


        public static void DetectionHalconRect(int i, HWindow hWindow, HObject hImage, Parameter.Rect1 rect1, ref bool result)
        {
            HObject ho_Rectangle, ho_ImageReduced;
            HObject ho_Regions, ho_Regions1, ho_ConnectedRegions, ho_ConnectedRegions1;
            HObject ho_SelectedRegions, ho_SelectedRegions1;

            // Local control variables 

            HTuple hv_WindowHandle = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple();
            // Initialize local and output iconic variables 

            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            //dev_open_window(...);

            HOperatorSet.SetColor(hWindow, "green");
            HOperatorSet.SetDraw(hWindow, "margin");
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, rect1.Row1, rect1.Colum1, rect1.Row2, rect1.Colum2);
            HOperatorSet.DispObj(ho_Rectangle, hWindow);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 0, Parameter.specificationsCam1[i].ThresholdLow);
            ho_Regions1.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions1, Parameter.specificationsCam1[i].ThresholdHigh, 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_Regions1, out ho_ConnectedRegions1);
            HOperatorSet.SetColor(hWindow, "red");
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", Parameter.specificationsCam1[i].AreaLow, Parameter.specificationsCam1[i].AreaHigh);
            HOperatorSet.DispObj(ho_SelectedRegions, hWindow);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area", "and", Parameter.specificationsCam1[i].AreaLow, Parameter.specificationsCam1[i].AreaHigh);
            HOperatorSet.DispObj(ho_SelectedRegions1, hWindow);
            hv_Area.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose();
            HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row1, out hv_Column1);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
            HTuple end_val13 = hv_Number - 1;
            HTuple step_val13 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val13, step_val13); hv_Index = hv_Index.TupleAdd(step_val13))
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetTposition(hWindow, hv_Row1.TupleSelect(hv_Index), hv_Column1.TupleSelect(hv_Index));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, hv_Area.TupleSelect(hv_Index));
                }
            }
            hv_Area1.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose();
            HOperatorSet.AreaCenter(ho_SelectedRegions1, out hv_Area1, out hv_Row1, out hv_Column1);
            hv_Number1.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number1);
            HTuple end_val19 = hv_Number1 - 1;
            HTuple step_val19 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val19, step_val19); hv_Index1 = hv_Index1.TupleAdd(step_val19))
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetTposition(hWindow, hv_Row1.TupleSelect(hv_Index1), hv_Column1.TupleSelect(hv_Index1));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, hv_Area1.TupleSelect(hv_Index1));
                }
            }

            if (hv_Number == 0 && hv_Number1 == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();
            ho_Regions.Dispose();
            ho_Regions1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();

            hv_WindowHandle.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Area.Dispose();
            hv_Number.Dispose();
            hv_Index.Dispose();
            hv_Area1.Dispose();
            hv_Number1.Dispose();
            hv_Index1.Dispose();
            //return true;
        }

        public static void DetectionHalconRect1(int i, int j, HWindow hWindow, HObject hImage, HObject ho_Rectangle, ref bool result)
        {
            HObject ho_ImageReduced;
            HObject ho_Regions, ho_Regions1, ho_ConnectedRegions, ho_ConnectedRegions1;
            HObject ho_SelectedRegions, ho_SelectedRegions1;

            // Local control variables 

            HTuple hv_WindowHandle = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Number = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Area1 = new HTuple(), hv_Number1 = new HTuple();
            HTuple hv_Index1 = new HTuple();
            // Initialize local and output iconic variables 

            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            //dev_open_window(...);

            HOperatorSet.SetColor(hWindow, "green");
            HOperatorSet.SetDraw(hWindow, "margin");
            HOperatorSet.DispObj(ho_Rectangle, hWindow);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 100, 255);
            HOperatorSet.FillUp(ho_Regions, out ho_ImageReduced);
            if(j<3)
            {
                HOperatorSet.ErosionCircle(ho_ImageReduced, out ho_Rectangle, 20);
                HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            }
            else
            {

                HOperatorSet.ReduceDomain(hImage, ho_ImageReduced, out ho_ImageReduced);
            }
            
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_Regions);
            if(j==1)
            {
                string stfFileNameOut = MainForm.strDateTime;  // 默认的图像保存名称
                string pathOut = Parameter.commministion.ImageSavePath + "\\" + MainForm.strDateTimeDay + "\\Cam2\\DeepLearnImage";
                if (!System.IO.Directory.Exists(pathOut))
                {
                    System.IO.Directory.CreateDirectory(pathOut);//不存在就创建文件夹
                }
                HOperatorSet.WriteImage(ho_Regions, "bmp", 0, pathOut + "\\" + stfFileNameOut + ".bmp");
            }

            HOperatorSet.DispObj(ho_Rectangle, hWindow);
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions, 0, Parameter.specificationsCam2[i].ThresholdLow[j]);
            ho_Regions1.Dispose();
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Regions1, Parameter.specificationsCam2[i].ThresholdHigh[j], 255);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_ConnectedRegions);
            ho_ConnectedRegions1.Dispose();
            HOperatorSet.Connection(ho_Regions1, out ho_ConnectedRegions1);
            HOperatorSet.SetColor(hWindow, "red");
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", Parameter.specificationsCam2[i].AreaLow[j], Parameter.specificationsCam2[i].AreaHigh[j]);
            HOperatorSet.DispObj(ho_SelectedRegions, hWindow);
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions1, "area", "and", Parameter.specificationsCam2[i].AreaLow[j], Parameter.specificationsCam2[i].AreaHigh[j]);
            HOperatorSet.DispObj(ho_SelectedRegions1, hWindow);
            hv_Area.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose();
            HOperatorSet.AreaCenter(ho_SelectedRegions, out hv_Area, out hv_Row1, out hv_Column1);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);
            HTuple end_val13 = hv_Number - 1;
            HTuple step_val13 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val13, step_val13); hv_Index = hv_Index.TupleAdd(step_val13))
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetTposition(hWindow, hv_Row1.TupleSelect(hv_Index), hv_Column1.TupleSelect(hv_Index));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, hv_Area.TupleSelect(hv_Index));
                }
            }
            hv_Area1.Dispose(); hv_Row1.Dispose(); hv_Column1.Dispose();
            HOperatorSet.AreaCenter(ho_SelectedRegions1, out hv_Area1, out hv_Row1, out hv_Column1);
            hv_Number1.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number1);
            HTuple end_val19 = hv_Number1 - 1;
            HTuple step_val19 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val19, step_val19); hv_Index1 = hv_Index1.TupleAdd(step_val19))
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetTposition(hWindow, hv_Row1.TupleSelect(hv_Index1), hv_Column1.TupleSelect(hv_Index1));
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.WriteString(hWindow, hv_Area1.TupleSelect(hv_Index1));
                }
            }

            if (hv_Number == 0 && hv_Number1 == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            ho_ImageReduced.Dispose();
            ho_Regions.Dispose();
            ho_Regions1.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_ConnectedRegions1.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();

            hv_WindowHandle.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Area.Dispose();
            hv_Number.Dispose();
            hv_Index.Dispose();
            hv_Area1.Dispose();
            hv_Number1.Dispose();
            hv_Index1.Dispose();
            //return true;
        }
        public static void DetectionHalconRect2(int i,int j, HWindow hWindow, HObject hImage, HObject ho_Rectangle, ref bool result)
        {
            // Local iconic variables 

            HObject ho_ImageReduced;
            HObject ho_ImagePart, ho_ImageMean, ho_Region, ho_ConnectedRegions;
            HObject ho_SelectedRegions;

            // Local control variables 
            HTuple hv_Number = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_WindowHandle = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();

            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);

            //dev_open_window(...);

            hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();

            HOperatorSet.SetColor(hWindow, "green");
            HOperatorSet.SetDraw(hWindow, "margin");
            HOperatorSet.SetLineWidth(hWindow,1);           
            HOperatorSet.DispObj(ho_Rectangle, hWindow);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.Threshold(ho_ImageReduced, out ho_Rectangle, 100, 255);
            HOperatorSet.FillUp(ho_Rectangle, out ho_ImagePart);
            HOperatorSet.ErosionCircle(ho_ImagePart,out ho_Rectangle, 20);
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.DispObj(ho_Rectangle, hWindow);
            ho_ImagePart.Dispose();
            ho_ImageMean.Dispose();
            HOperatorSet.MeanImage(ho_ImageReduced, out ho_ImageMean, 35, 35);
            ho_Region.Dispose();
            HOperatorSet.DynThreshold(ho_ImageReduced, ho_ImageMean, out ho_Region, 25, "not_equal");
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_ConnectedRegions, out hv_Area, out hv_Row, out hv_Column);
            //HOperatorSet.DispObj(ho_ImagePart, hWindow);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area","and", Parameter.specificationsCam2[i].AreaLow[j], Parameter.specificationsCam2[i].AreaHigh[j]);
            HOperatorSet.SetColor(hWindow, "red");//red
            HOperatorSet.SetDraw(hWindow, "margin");

            HOperatorSet.DispObj(ho_SelectedRegions, hWindow);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions, out hv_Number);

            if (hv_Number == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            ho_ImageReduced.Dispose();
            ho_ImagePart.Dispose();
            ho_ImageMean.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();

            hv_Width.Dispose();
            hv_Height.Dispose();
            hv_WindowHandle.Dispose();
            hv_Row1.Dispose();
            hv_Column1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Area.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
        }
        public static bool DetectionHalconReadDlModel(string path, out HTuple hv_DLModelHandle, out HTuple hv_DLPreprocessParam, out HTuple hv_InferenceClassificationThreshold, out HTuple hv_InferenceSegmentationThreshold)
        {
            HTuple hv_DLDeviceHandles = new HTuple(), hv_DLDeviceHandle = new HTuple();
            HTuple hv_MetaData = new HTuple();
            HTuple hv_DLDatasetInfo = new HTuple();

            HOperatorSet.ReadDlModel(path, out hv_DLModelHandle);
            hv_DLDeviceHandles.Dispose();
            HOperatorSet.QueryAvailableDlDevices(((new HTuple("runtime")).TupleConcat("runtime")).TupleConcat("id"), ((new HTuple("gpu")).TupleConcat("cpu")).TupleConcat(0), out hv_DLDeviceHandles);
            if ((int)(new HTuple(hv_DLDeviceHandles.TupleEqual(new HTuple()))) != 0)
            {
                throw new HalconException("No suitable CPU or GPU was found.");
            }
            hv_DLDeviceHandle.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DLDeviceHandle = hv_DLDeviceHandles.TupleSelect(0);
            }
            HOperatorSet.SetDlModelParam(hv_DLModelHandle, "device", hv_DLDeviceHandle);

            hv_MetaData.Dispose();
            HOperatorSet.GetDlModelParam(hv_DLModelHandle, "meta_data", out hv_MetaData);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InferenceClassificationThreshold = ((hv_MetaData.TupleGetDictTuple("anomaly_classification_threshold"))).TupleNumber();
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_InferenceSegmentationThreshold = ((hv_MetaData.TupleGetDictTuple("anomaly_segmentation_threshold"))).TupleNumber();
            }
            HDevelopExport.create_dl_preprocess_param_from_model(hv_DLModelHandle, "none", "full_domain", new HTuple(), new HTuple(), new HTuple(), out hv_DLPreprocessParam);
            hv_DLDatasetInfo.Dispose();
            HOperatorSet.CreateDict(out hv_DLDatasetInfo);
            HOperatorSet.SetDictTuple(hv_DLDatasetInfo, "class_names", (new HTuple("ok")).TupleConcat("ng"));
            HOperatorSet.SetDictTuple(hv_DLDatasetInfo, "class_ids", (new HTuple(0)).TupleConcat(1));

            return true;
        }


        public static void DetectionHalconDeepLearning(int i, HWindow hWindow, HObject hImage, HTuple hv_DLModelHandle, HTuple hv_DLPreprocessParam, HTuple hv_InferenceClassificationThreshold, HTuple hv_InferenceSegmentationThreshold, HTuple RowY1, HTuple ColumX, ref bool result)
        {
            HObject ho_Rectangle;
            HObject ho_ImageReduced, ho_ImagePart;

            HTuple hv_DLDatasetInfo = new HTuple();
            HTuple hv_DLSample = new HTuple(), hv_DLResult = new HTuple();
            HTuple hv_anomaly_score = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.SetDraw(hWindow, "margin");
            if (0 == i)//N1区域深度学习设置
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, RowY1 - 50, 检测1.pointReault[0].Colum1 + 80, RowY1 + 30, 检测1.pointReault[1].Colum1 - 80);
                    HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImagePart);
                    HOperatorSet.CropDomain(ho_ImagePart, out ho_ImageReduced);

                    string stfFileNameOut = MainForm.strDateTime;  // 默认的图像保存名称
                    string pathOut = Parameter.commministion.ImageSavePath + "\\" + MainForm.strDateTimeDay + "\\Cam1\\DeepLearnImage" + "\\N" + i;
                    if (!System.IO.Directory.Exists(pathOut))
                    {
                        System.IO.Directory.CreateDirectory(pathOut);//不存在就创建文件夹
                    }
                    HOperatorSet.WriteImage(ho_ImageReduced, "bmp", 0, pathOut + "\\" + stfFileNameOut + ".bmp");
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispRectangle1(hWindow, RowY1 - 50, 检测1.pointReault[0].Colum1 + 80, RowY1 + 30, 检测1.pointReault[1].Colum1 - 80);
                }
            }
            else//N2区域设置
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, RowY1 - 30, 检测1.pointReault[0].Colum1 + 80, RowY1 + 50, 检测1.pointReault[1].Colum1 - 80);
                    HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImagePart);
                    HOperatorSet.CropDomain(ho_ImagePart, out ho_ImageReduced);

                    string stfFileNameOut = MainForm.strDateTime;  // 默认的图像保存名称
                    string pathOut = Parameter.commministion.ImageSavePath + "\\" + MainForm.strDateTimeDay + "\\Cam1\\DeepLearnImage" + "\\N" + i;
                    if (!System.IO.Directory.Exists(pathOut))
                    {
                        System.IO.Directory.CreateDirectory(pathOut);//不存在就创建文件夹
                    }
                    HOperatorSet.WriteImage(ho_ImageReduced, "bmp", 0, pathOut + "\\" + stfFileNameOut + ".bmp");
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispRectangle1(hWindow, RowY1 - 30, 检测1.pointReault[0].Colum1 + 80, RowY1 + 50, 检测1.pointReault[1].Colum1 - 80);
                }
            }
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            ho_ImagePart.Dispose();
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
            //write_image (ImagePart, 'bmp', 0, 'C:/Users/王印/Desktop/halconDPL/极耳/N1/N1训练图/OK/'+ Index +'.bmp')
            //HOperatorSet.WriteImage(ho_ImagePart, "bmp", 0, "C:/Users/王印/Desktop/halconDPL/极耳/N2/N2训练图/NG/0.bmp");
            //推理程序：调用model_best.hdl推理指定目录下测试图像
            //读取模型

            hv_DLSample.Dispose();
            HDevelopExport.gen_dl_samples_from_images(ho_ImagePart, out hv_DLSample);
            HDevelopExport.preprocess_dl_samples(hv_DLSample, hv_DLPreprocessParam);
            hv_DLResult.Dispose();
            HOperatorSet.ApplyDlModel(hv_DLModelHandle, hv_DLSample, new HTuple(), out hv_DLResult);
            HDevelopExport.threshold_dl_anomaly_results(hv_InferenceSegmentationThreshold, hv_InferenceClassificationThreshold, hv_DLResult);
            HOperatorSet.SetTposition(hWindow, RowY1, ColumX);
            hv_anomaly_score.Dispose();
            HOperatorSet.GetDictTuple(hv_DLResult, "anomaly_score", out hv_anomaly_score);
            if (hv_anomaly_score <= Parameter.specificationsCam1[i].DeepLearningRate)
            {
                result = true;
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.WriteString(hWindow, "OK" + hv_anomaly_score);
                }
            }
            else
            {
                result = false;
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetColor(hWindow, "red"); //red
                    HOperatorSet.WriteString(hWindow, "NG" + hv_anomaly_score);
                }
            }
            ho_Rectangle.Dispose();
            ho_ImagePart.Dispose();
            hv_DLDatasetInfo.Dispose();

            hv_DLSample.Dispose();
            hv_DLResult.Dispose();
            hv_anomaly_score.Dispose();
        }

        public static void DetectionHalconDeepLearning1(int i, HWindow hWindow, HObject hImage, HObject ho_Rectangle, HTuple hv_DLModelHandle, HTuple hv_DLPreprocessParam, HTuple hv_InferenceClassificationThreshold, HTuple hv_InferenceSegmentationThreshold, Rect1 rect1, ref bool result)
        {
            HObject ho_ImageReduced, ho_ImagePart;
            HTuple hv_DLDatasetInfo = new HTuple();
            HTuple hv_DLSample = new HTuple(), hv_DLResult = new HTuple();
            HTuple hv_anomaly_score = new HTuple();
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_ImagePart);
            HOperatorSet.SetDraw(hWindow, "margin");

            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(hImage, ho_Rectangle, out ho_ImageReduced);
            ho_ImagePart.Dispose();
            HOperatorSet.CropDomain(ho_ImageReduced, out ho_ImagePart);
            //write_image (ImagePart, 'bmp', 0, 'C:/Users/王印/Desktop/halconDPL/极耳/N1/N1训练图/OK/'+ Index +'.bmp')
            //HOperatorSet.WriteImage(ho_ImagePart, "bmp", 0, "C:/Users/王印/Desktop/halconDPL/极耳/N2/N2训练图/NG/0.bmp");
            //推理程序：调用model_best.hdl推理指定目录下测试图像
            //读取模型

            hv_DLSample.Dispose();
            HDevelopExport.gen_dl_samples_from_images(ho_ImagePart, out hv_DLSample);
            HDevelopExport.preprocess_dl_samples(hv_DLSample, hv_DLPreprocessParam);
            hv_DLResult.Dispose();
            HOperatorSet.ApplyDlModel(hv_DLModelHandle, hv_DLSample, new HTuple(), out hv_DLResult);
            HDevelopExport.threshold_dl_anomaly_results(hv_InferenceSegmentationThreshold, hv_InferenceClassificationThreshold, hv_DLResult);
            HOperatorSet.SetTposition(hWindow, rect1.Row1, rect1.Colum1);
            hv_anomaly_score.Dispose();
            HOperatorSet.GetDictTuple(hv_DLResult, "anomaly_score", out hv_anomaly_score);
            if (hv_anomaly_score <= Parameter.specificationsCam2[i].DeepLearningRate)
            {
                result = true;
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetColor(hWindow, "green");
                    HOperatorSet.WriteString(hWindow, "OK" + hv_anomaly_score);
                }
            }
            else
            {
                result = false;
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.SetColor(hWindow, "red");
                    HOperatorSet.WriteString(hWindow, "NG" + hv_anomaly_score);
                }
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                HOperatorSet.DispRectangle1(hWindow, rect1.Row1, rect1.Colum1, rect1.Row2, rect1.Colum2);
            }
            ho_Rectangle.Dispose();
            ho_ImagePart.Dispose();
            hv_DLDatasetInfo.Dispose();

            hv_DLSample.Dispose();
            hv_DLResult.Dispose();
            hv_anomaly_score.Dispose();
        }

    }
}
