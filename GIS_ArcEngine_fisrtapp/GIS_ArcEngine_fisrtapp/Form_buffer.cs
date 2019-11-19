using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;

namespace GIS_ArcEngine_fisrtapp
{
    public partial class Form_buffer : Form
    {
        private AxMapControl axMapControl1 = null;
        private IFeatureLayer featureLaer = null;
        string path = null;
        string distance = null;
        public Form_buffer()
        {
            InitializeComponent();
        }

        public Form_buffer(AxMapControl axMapControl1)
        {
            InitializeComponent();
            this.axMapControl1 = axMapControl1;
        }
        private void comboBox1_Click(object sender, EventArgs e)
        {
            if (this.axMapControl1.LayerCount > 0)
            {
                for (int i = 0; i < this.axMapControl1.LayerCount; i++)
                {

                    comboBox1.Items.Add(this.axMapControl1.get_Layer(i).Name);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            distance = textBox2.Text + " " + comboBox2.Text;

            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(featureLaer, path, distance);
            Console.WriteLine(buffer);
            try
            {
                gp.Execute(buffer, null);
                MessageBox.Show("缓冲区建立成功");
            }
            catch
            {
                MessageBox.Show("缓冲区建立失败！");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            featureLaer = this.axMapControl1.get_Layer(comboBox1.SelectedIndex) as IFeatureLayer;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
