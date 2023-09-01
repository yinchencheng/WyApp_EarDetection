using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WY_App.Utility;

namespace WY_App
{
    public partial class 相机配置 : Form
    {
        public 相机配置()
        {
            InitializeComponent();
        }

        private void 修改_Click(object sender, EventArgs e)
        {

            Column.Enabled = true;
            Phi.Enabled = true;
            Length1.Enabled = true;
            Length2.Enabled = true;

        }

        private void 保存_Click(object sender, EventArgs e)
        {
            try
            {
                //if (FormCamera.Round_Edge == 0)
                //{
                //    Halcon.Xml_to_base(this, Data.constructor);
                //    XMLHelper.serialize<Constructor.Rent2>(Data.constructor[Data.select_image], Application.StartupPath + "/Parameter/Data.select_image.xml");
                //}
                //if (FormCamera.Round_Edge == 1)
                //{
                //    Halcon.Xml_to_base(this, Data.Base_c1_y1[Data.select_image]);
                //    XMLHelper.serialize<Constructor.Rent2>(Data.Base_c1_y1[Data.select_image], Application.StartupPath + "/Parameter/Base_c1_y1.xml");
                //}
                //if (FormCamera.Round_Edge == 2)
                //{
                //    Halcon.Xml_to_base(this, Data.Base_c1_y2[Data.select_image]);
                //    XMLHelper.serialize<Constructor.Rent2>(Data.Base_c1_y2[Data.select_image], Application.StartupPath + "/Parameter/Base_c1_y2.xml");
                //}

               
            }
            catch
            {
                MessageBox.Show("数据保存失败！");
            }
            this.Close();
        }
    }
}
