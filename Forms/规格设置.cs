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
using static WY_App.Utility.Parameter;
using Parameter = WY_App.Utility.Parameter;

namespace WY_App
{
    public partial class 规格设置 : Form
    {
        public 规格设置()
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
            num_胶宽value.Enabled = true;
            num_胶宽min.Enabled = true;
            num_胶宽max.Enabled = true;
            num_胶宽adjustN1.Enabled = true;
            num_胶宽adjustN2.Enabled = true;

            num_胶高value.Enabled = true;
            num_胶高min.Enabled = true;
            num_胶高max.Enabled = true;
            num_胶高adjustN1.Enabled = true;
            num_胶高adjustN2.Enabled = true;

            num_胶线value.Enabled = true;
            num_胶线min.Enabled = true;
            num_胶线max.Enabled = true;
            num_胶线adjustN1.Enabled = true;
            num_胶线adjustN2.Enabled = true;

            uiDouble00.Enabled = true;
            uiDouble01.Enabled = true;
            uiDouble02.Enabled = true;
            uiDouble03.Enabled = true;
            uiDouble04.Enabled = true;

            uiDouble10.Enabled = true;
            uiDouble11.Enabled = true;
            uiDouble12.Enabled = true;
            uiDouble13.Enabled = true;
            uiDouble14.Enabled = true;

            uiDouble20.Enabled = true;
            uiDouble21.Enabled = true;
            uiDouble22.Enabled = true;
            uiDouble23.Enabled = true;
            uiDouble24.Enabled = true;

            uiDouble30.Enabled = true;
            uiDouble31.Enabled = true;
            uiDouble32.Enabled = true;
            uiDouble33.Enabled = true;
            uiDouble34.Enabled = true;

            uiDouble40.Enabled = true;
            uiDouble41.Enabled = true;
            uiDouble42.Enabled = true;
            uiDouble43.Enabled = true;
            uiDouble44.Enabled = true;

			uiDouble45.Enabled = true;
			uiDouble46.Enabled = true;
			uiDouble47.Enabled = true;
			uiDouble48.Enabled = true;
			uiDouble49.Enabled = true;

            uiDouble50.Enabled = true;
            uiDouble51.Enabled = true;
            uiDouble52.Enabled = true;
            uiDouble53.Enabled = true;
            uiDouble54.Enabled = true;

            btn_Save.Enabled = true;

        }

        private void ParamSettings_Load(object sender, EventArgs e)
        {
            num_胶宽value.Value = Parameter.specificationsCam1[0].肩高.value;
            num_胶宽min.Value = Parameter.specificationsCam1[0].肩高.min;
            num_胶宽max.Value = Parameter.specificationsCam1[0].肩高.max;
            num_胶宽adjustN1.Value = Parameter.specificationsCam1[0].肩高.adjust;
            num_胶高adjustN2.Value = Parameter.specificationsCam1[1].肩高.adjust;

            num_胶高value.Value = Parameter.specificationsCam1[0].肩宽.value;
            num_胶高min.Value = Parameter.specificationsCam1[0].肩宽.min;
            num_胶高max.Value = Parameter.specificationsCam1[0].肩宽.max;
            num_胶高adjustN1.Value = Parameter.specificationsCam1[0].肩宽.adjust;
            num_胶高adjustN2.Value = Parameter.specificationsCam1[1].肩宽.adjust;

            num_胶线value.Value = Parameter.specificationsCam1[0].胶线.value;
            num_胶线min.Value = Parameter.specificationsCam1[0].胶线.min;
            num_胶线max.Value = Parameter.specificationsCam1[0].胶线.max;
            num_胶线adjustN1.Value = Parameter.specificationsCam1[0].胶线.adjust;
            num_胶线adjustN2.Value = Parameter.specificationsCam1[1].胶线.adjust;

            uiDouble00.Value = Parameter.specificationsCam2[0].检测规格[0].min;
            uiDouble01.Value = Parameter.specificationsCam2[0].检测规格[0].value;
            uiDouble02.Value = Parameter.specificationsCam2[0].检测规格[0].max;
            uiDouble03.Value = Parameter.specificationsCam2[0].检测规格[0].adjust;
            uiDouble04.Value = Parameter.specificationsCam2[1].检测规格[0].adjust;

            uiDouble10.Value = Parameter.specificationsCam2[0].检测规格[1].min;
			uiDouble11.Value = Parameter.specificationsCam2[0].检测规格[1].value;
			uiDouble12.Value = Parameter.specificationsCam2[0].检测规格[1].max;
			uiDouble13.Value = Parameter.specificationsCam2[0].检测规格[1].adjust;
			uiDouble14.Value = Parameter.specificationsCam2[1].检测规格[1].adjust;

            uiDouble20.Value = Parameter.specificationsCam2[0].检测规格[2].min;
			uiDouble21.Value = Parameter.specificationsCam2[0].检测规格[2].value;
			uiDouble22.Value = Parameter.specificationsCam2[0].检测规格[2].max;
			uiDouble23.Value = Parameter.specificationsCam2[0].检测规格[2].adjust;
			uiDouble24.Value = Parameter.specificationsCam2[1].检测规格[2].adjust;

            uiDouble30.Value = Parameter.specificationsCam2[0].检测规格[3].min;
			uiDouble31.Value = Parameter.specificationsCam2[0].检测规格[3].value;
			uiDouble32.Value = Parameter.specificationsCam2[0].检测规格[3].max;
			uiDouble33.Value = Parameter.specificationsCam2[0].检测规格[3].adjust;
			uiDouble34.Value = Parameter.specificationsCam2[1].检测规格[3].adjust;

            uiDouble40.Value = Parameter.specificationsCam2[0].检测规格[4].min;
			uiDouble41.Value = Parameter.specificationsCam2[0].检测规格[4].value;
			uiDouble42.Value = Parameter.specificationsCam2[0].检测规格[4].max;
			uiDouble43.Value = Parameter.specificationsCam2[0].检测规格[4].adjust;
			uiDouble44.Value = Parameter.specificationsCam2[1].检测规格[4].adjust;

			uiDouble45.Value = Parameter.specificationsCam2[0].检测规格[5].min;
			uiDouble46.Value = Parameter.specificationsCam2[0].检测规格[5].value;
			uiDouble47.Value = Parameter.specificationsCam2[0].检测规格[5].max;
			uiDouble48.Value = Parameter.specificationsCam2[0].检测规格[5].adjust;
			uiDouble49.Value = Parameter.specificationsCam2[1].检测规格[5].adjust;

            uiDouble50.Value = Parameter.specificationsCam2[0].检测规格[6].min;
            uiDouble51.Value = Parameter.specificationsCam2[0].检测规格[6].value;
            uiDouble52.Value = Parameter.specificationsCam2[0].检测规格[6].max;
            uiDouble53.Value = Parameter.specificationsCam2[0].检测规格[6].adjust;
            uiDouble54.Value = Parameter.specificationsCam2[1].检测规格[6].adjust;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Parameter.specificationsCam1[0].肩高.value =  num_胶宽value.Value;
            Parameter.specificationsCam1[0].肩高.min =    num_胶宽min.Value;
            Parameter.specificationsCam1[0].肩高.max =    num_胶宽max.Value;
            Parameter.specificationsCam1[0].肩高.adjust = num_胶宽adjustN1.Value;
            Parameter.specificationsCam1[1].肩高.adjust = num_胶宽adjustN2.Value;

            Parameter.specificationsCam1[0].肩宽.value =  num_胶高value.Value;
            Parameter.specificationsCam1[0].肩宽.min =    num_胶高min.Value;
            Parameter.specificationsCam1[0].肩宽.max =    num_胶高max.Value;
            Parameter.specificationsCam1[0].肩宽.adjust = num_胶高adjustN1.Value;
            Parameter.specificationsCam1[1].肩宽.adjust = num_胶高adjustN2.Value;

            Parameter.specificationsCam1[0].胶线.value =  num_胶线value.Value;
            Parameter.specificationsCam1[0].胶线.min =    num_胶线min.Value;
            Parameter.specificationsCam1[0].胶线.max =    num_胶线max.Value;
            Parameter.specificationsCam1[0].胶线.adjust = num_胶线adjustN1.Value;
            Parameter.specificationsCam1[1].胶线.adjust = num_胶线adjustN2.Value;

            Parameter.specificationsCam2[0].检测规格[0].min = uiDouble00.Value;
			Parameter.specificationsCam2[0].检测规格[0].value = uiDouble01.Value;
			Parameter.specificationsCam2[0].检测规格[0].max = uiDouble02.Value;
			Parameter.specificationsCam2[0].检测规格[0].adjust = uiDouble03.Value;
			Parameter.specificationsCam2[1].检测规格[0].adjust = uiDouble04.Value;

            Parameter.specificationsCam2[0].检测规格[1].min = uiDouble10.Value;
			Parameter.specificationsCam2[0].检测规格[1].value = uiDouble11.Value;
			Parameter.specificationsCam2[0].检测规格[1].max = uiDouble12.Value;
			Parameter.specificationsCam2[0].检测规格[1].adjust = uiDouble13.Value;
			Parameter.specificationsCam2[1].检测规格[1].adjust = uiDouble14.Value;

            Parameter.specificationsCam2[0].检测规格[2].min = uiDouble20.Value;
			Parameter.specificationsCam2[0].检测规格[2].value = uiDouble21.Value;
			Parameter.specificationsCam2[0].检测规格[2].max = uiDouble22.Value;
			Parameter.specificationsCam2[0].检测规格[2].adjust = uiDouble23.Value;
			Parameter.specificationsCam2[1].检测规格[2].adjust = uiDouble24.Value;

            Parameter.specificationsCam2[0].检测规格[3].min = uiDouble30.Value;
			Parameter.specificationsCam2[0].检测规格[3].value = uiDouble31.Value;
			Parameter.specificationsCam2[0].检测规格[3].max = uiDouble32.Value;
			Parameter.specificationsCam2[0].检测规格[3].adjust = uiDouble33.Value;
			Parameter.specificationsCam2[1].检测规格[3].adjust = uiDouble34.Value;

            Parameter.specificationsCam2[0].检测规格[4].min = uiDouble40.Value;
			Parameter.specificationsCam2[0].检测规格[4].value = uiDouble41.Value;
			Parameter.specificationsCam2[0].检测规格[4].max = uiDouble42.Value;
			Parameter.specificationsCam2[0].检测规格[4].adjust = uiDouble43.Value;
			Parameter.specificationsCam2[1].检测规格[4].adjust = uiDouble44.Value;

			Parameter.specificationsCam2[0].检测规格[5].min = uiDouble45.Value;
			Parameter.specificationsCam2[0].检测规格[5].value = uiDouble46.Value;
			Parameter.specificationsCam2[0].检测规格[5].max = uiDouble47.Value;
			Parameter.specificationsCam2[0].检测规格[5].adjust = uiDouble48.Value;
			Parameter.specificationsCam2[1].检测规格[5].adjust = uiDouble49.Value;

            Parameter.specificationsCam2[0].检测规格[6].min = uiDouble50.Value;
            Parameter.specificationsCam2[0].检测规格[6].value = uiDouble51.Value;
            Parameter.specificationsCam2[0].检测规格[6].max = uiDouble52.Value;
            Parameter.specificationsCam2[0].检测规格[6].adjust = uiDouble53.Value;
            Parameter.specificationsCam2[1].检测规格[6].adjust = uiDouble54.Value;


            XMLHelper.serialize<Parameter.SpecificationsCam1>(Parameter.specificationsCam1[0], Parameter.commministion.productName + "/Cam1Specifications0.xml");
            XMLHelper.serialize<Parameter.SpecificationsCam1>(Parameter.specificationsCam1[1], Parameter.commministion.productName + "/Cam1Specifications1.xml");
            XMLHelper.serialize<Parameter.SpecificationsCam2>(Parameter.specificationsCam2[0], Parameter.commministion.productName + "/Cam2Specifications0.xml");
            XMLHelper.serialize<Parameter.SpecificationsCam2>(Parameter.specificationsCam2[1], Parameter.commministion.productName + "/Cam2Specifications1.xml");
            MessageBox.Show("系统参数修改，请重启软件");
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
