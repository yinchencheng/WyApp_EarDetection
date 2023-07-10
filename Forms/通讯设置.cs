using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using WY_App.Utility;
using Parameters = WY_App.Utility.Parameters;

namespace WY_App
{
    public partial class 通讯设置 : Form
    {
        public 通讯设置()
        {
            InitializeComponent();
        }

        private void btn_Close_System_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        Point downPoint;
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - downPoint.X,
                    this.Location.Y + e.Y - downPoint.Y);
            }
        }

        private void btn_Change_Click(object sender, EventArgs e)
        {
            txt_PlcIpAddress.Enabled = true;
            num_PLCPort.Enabled = true;
            txt_PlcDevice.Enabled = true;
            txt_PlcType.Enabled = true;
            chk_PLCEnabled.Enabled = true;

            txt_ServerIP.Enabled = true;
            num_ServerPort.Enabled = true;
            chk_ServerEnabled.Enabled = true;

            txt_ClientIP.Enabled = true;
            num_ClientPort.Enabled = true;
            chk_ClientEnabled.Enabled = true;

            txt_Trigger_Detection1.Enabled = true;
            txt_Trigger_Detection2.Enabled = true;
            txt_StartAdd.Enabled = true;
            txt_Completion_Add1.Enabled = true;
            txt_Completion_Add2.Enabled = true;
            num_LogSaveDays.Enabled = true;

            txt_ImagePath.Enabled = true;
            txt_ImageSavePath.Enabled = true;
            btn_Save.Enabled = true;

        }

        private void ParamSettings_Load(object sender, EventArgs e)
        {
            txt_PlcIpAddress.Text = Parameters.commministion.PlcIpAddress;
            num_PLCPort.Value = Parameters.commministion.PlcIpPort;
            txt_PlcDevice.Text = Parameters.commministion.PlcDevice;
            txt_PlcType.Text = Parameters.commministion.PlcType;
            chk_PLCEnabled.Checked = Parameters.commministion.PlcEnable;

            txt_ImagePath.Text = Parameters.commministion.ImagePath;
            txt_ImageSavePath.Text = Parameters.commministion.ImageSavePath;

            txt_ServerIP.Text = Parameters.commministion.TcpServerIpAddress;
            num_ServerPort.Value = Parameters.commministion.TcpServerIpPort;
            chk_ServerEnabled.Checked = Parameters.commministion.TcpServerEnable;

            txt_ClientIP.Text = Parameters.commministion.TcpClientIpAddress;
            num_ClientPort.Value = Parameters.commministion.TcpClientIpPort;
            chk_ClientEnabled.Checked = Parameters.commministion.TcpClientEnable;

            num_LogSaveDays.Value = Parameters.commministion.LogFileExistDay;

            txt_Trigger_Detection1.Text = Parameters.plcParams.Trigger_Detection1;
            txt_Trigger_Detection2.Text = Parameters.plcParams.Trigger_Detection2;
            txt_StartAdd.Text = Parameters.plcParams.StartAdd;
            txt_Completion_Add1.Text = Parameters.plcParams.Completion1;
            txt_Completion_Add2.Text = Parameters.plcParams.Completion2;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Parameters.commministion.PlcIpAddress = txt_PlcIpAddress.Text;
            Parameters.commministion.PlcIpPort = (int)num_PLCPort.Value;
            Parameters.commministion.PlcDevice = txt_PlcDevice.Text;
            Parameters.commministion.PlcType = txt_PlcType.Text;
            Parameters.commministion.PlcEnable = chk_PLCEnabled.Checked;

            Parameters.commministion.ImagePath = txt_ImagePath.Text;
            Parameters.commministion.ImageSavePath = txt_ImageSavePath.Text; ;

            Parameters.commministion.TcpServerIpAddress = txt_ServerIP.Text;
            Parameters.commministion.TcpServerIpPort = (int)num_ServerPort.Value;
            Parameters.commministion.TcpServerEnable = chk_ServerEnabled.Checked;

            Parameters.commministion.TcpClientIpAddress = txt_ClientIP.Text;
            Parameters.commministion.TcpClientIpPort = (int)num_ClientPort.Value;
            Parameters.commministion.TcpClientEnable = chk_ClientEnabled.Checked;

            Parameters.commministion.LogFileExistDay = (int)num_LogSaveDays.Value;

            XMLHelper.serialize<Parameters.Commministion>(Parameters.commministion, "Parameter/Commministion.xml");

            Parameters.plcParams.Trigger_Detection1 = txt_Trigger_Detection1.Text;
            Parameters.plcParams.Trigger_Detection2 = txt_Trigger_Detection2.Text;
            Parameters.plcParams.StartAdd= txt_StartAdd.Text;
            Parameters.plcParams.Completion1 = txt_Completion_Add1.Text;
            Parameters.plcParams.Completion2 = txt_Completion_Add2.Text;
            XMLHelper.serialize<Parameters.PLCParams>(Parameters.plcParams, "Parameter/PLCParams.xml");
            MessageBox.Show("系统参数修改，请重启软件");
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
