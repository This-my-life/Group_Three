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
    public partial class Form_erase : Form
    {
        private AxMapControl axMapControl1 = null;
        private IFeatureLayer featureLayer1 = null;
        private IFeatureLayer featureLayer2 = null;
        private string path = null;
        public Form_erase()
        {
            InitializeComponent();
        }

        public Form_erase(AxMapControl axMapControl1)
        {
            InitializeComponent();
            this.axMapControl1 = axMapControl1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            featureLayer1 = this.axMapControl1.get_Layer(comboBox1.SelectedIndex) as FeatureLayer;

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            featureLayer2 = this.axMapControl1.get_Layer(comboBox2.SelectedIndex) as FeatureLayer;

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


        private void comboBox2_Click(object sender, EventArgs e)
        {
            if (this.axMapControl1.LayerCount > 0)
            {
                for (int i = 0; i < this.axMapControl1.LayerCount; i++)
                {
                    comboBox2.Items.Add(this.axMapControl1.get_Layer(i).Name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Erase erase = new ESRI.ArcGIS.AnalysisTools.Erase(featureLayer1, featureLayer2, path);
            try
            {
                gp.Execute(erase, null);
                MessageBox.Show("擦除成功");
            }
            catch
            {
                MessageBox.Show("擦除失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
