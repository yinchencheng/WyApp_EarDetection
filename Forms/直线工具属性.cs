using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WY_App.Utility;

namespace WY_App
{
    public partial class 直线工具属性 : Form
    {
        public 直线工具属性()
        {
            InitializeComponent();           
        }

        private void btn_Close_System_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (MainForm.formloadIndex == 3 || MainForm.formloadIndex == 4)
            {
                if (MainForm.LineIndex < 3)
                {
                    Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].阈值  = num_yuzhi.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].simga = num_sigma.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].Index = (int)num_Index.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].LineLength = (int)Linelength.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].极性 = cmb_极性.Text;

                    XMLHelper.serialize<Parameters.SpecificationsCam1>(Parameters.specificationsCam1[0], "Parameter/Cam1Specifications" + 0 + ".xml");
                }
                else
                {
                    Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex - 3].阈值 = num_yuzhi.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex - 3].simga = num_sigma.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].Index = (int)num_Index.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].LineLength = (int)Linelength.Value;
                    Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].极性 = cmb_极性.Text;
                    XMLHelper.serialize<Parameters.SpecificationsCam1>(Parameters.specificationsCam1[1], "Parameter/Cam1Specifications" + 1 + ".xml");
                }
            }

            if (MainForm.formloadIndex == 5 || MainForm.formloadIndex == 6)
            {
                if (MainForm.LineIndex < 3)
                {
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].阈值 = num_yuzhi.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].simga = num_sigma.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].Index = (int)num_Index.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].LineLength = (int)Linelength.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].极性 = cmb_极性.Text;
                    XMLHelper.serialize<Parameters.SpecificationsCam2>(Parameters.specificationsCam2[0], "Parameter/Cam2Specifications" + 0 + ".xml");
                }
                else
                {
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].阈值 = num_yuzhi.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].simga = num_sigma.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].Index = (int)num_Index.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].LineLength = (int)Linelength.Value;
                    Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].极性 = cmb_极性.Text;
                    XMLHelper.serialize<Parameters.SpecificationsCam2>(Parameters.specificationsCam2[1], "Parameter/Cam2Specifications" + 1 + ".xml");
                }
            }
            this.Close();
        }

        private void 直线工具属性_Load(object sender, EventArgs e)
        {
            if (MainForm.formloadIndex == 3|| MainForm.formloadIndex == 4)
            {
                if(MainForm.LineIndex<3)
                {
                    num_yuzhi.Value = Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].阈值;
                    num_sigma.Value = Parameters.specificationsCam1[MainForm.formloadIndex-3].基准[MainForm.LineIndex].simga;
                    num_Index.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].基准[MainForm.LineIndex].Index;
                    Linelength.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].基准[MainForm.LineIndex].LineLength;
                    cmb_极性.Text = Parameters.specificationsCam1[MainForm.formloadIndex - 3].基准[MainForm.LineIndex].极性;

                }
                else
                {
                    num_yuzhi.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].阈值;
                    num_sigma.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].simga;
                    num_Index.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex - 3].Index;
                    Linelength.Value = Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex-3].LineLength;
                    cmb_极性.Text = Parameters.specificationsCam1[MainForm.formloadIndex - 3].模板区域[MainForm.LineIndex - 3].极性;
                }
            }
            
            if(MainForm.formloadIndex == 5|| MainForm.formloadIndex == 6)
            {
                if (MainForm.LineIndex < 3)
                {
                    num_yuzhi.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].阈值;
                    num_sigma.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].simga;
                    num_Index.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].Index;
                    Linelength.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].LineLength;
                    cmb_极性.Text = Parameters.specificationsCam2[MainForm.formloadIndex - 5].基准[MainForm.LineIndex].极性;
                }
                else
                {
                    num_yuzhi.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].阈值;
                    num_sigma.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].simga;
                    num_Index.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex-3].Index;
                    Linelength.Value = Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex-3].LineLength;
                    cmb_极性.Text = Parameters.specificationsCam2[MainForm.formloadIndex - 5].模板区域[MainForm.LineIndex - 3].极性;
                }
            }
            
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            num_yuzhi.Enabled = true;
            num_sigma.Enabled = true;
            num_Index.Enabled = true;
            cmb_极性.Enabled = true;
            Linelength.Enabled = true;
        }
    }
}
