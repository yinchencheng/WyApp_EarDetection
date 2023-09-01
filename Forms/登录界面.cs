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

namespace WY_App
{
    public partial class 登录界面 : Form
    {
        public 登录界面()
        {
            InitializeComponent();           
        }

        private void btn_Close_System_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            this.Close();
            if (bunifuCustomTextbox2.Text.Contains("toptics888")|| bunifuCustomTextbox2.Text.Contains(""))
            {
                if (MainForm.formloadIndex == 1)
                {
					通讯设置 paramSettings = new 通讯设置();
					paramSettings.ShowDialog();
				}
                else if (MainForm.formloadIndex == 2)
                {
					规格设置 flg = new 规格设置();
					flg.ShowDialog();
				}
                else if (MainForm.formloadIndex == 3)
                {
                    检测1 flg = new 检测1();
                    flg.ShowDialog();
                }
                else if (MainForm.formloadIndex == 5)
                {
                    检测2 flg = new 检测2();
                    flg.ShowDialog();
                }
            }
            else
            {               
                MessageBox.Show("密码错误");
            }
            
        }

        private void bunifuCustomTextbox2_TextChanged(object sender, EventArgs e)
        {
            if(this.bunifuCustomTextbox2.Text.Length==10)
            {
                btn_login.Focus();
            }
        }
    }
}
