using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.AnalysisTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GIS_ArcEngine_fisrtapp.function;

namespace GIS_ArcEngine_fisrtapp
{
    public partial class Clip_Window : Form
    {
        private AxMapControl axmapcontrol1; 
        public Clip_Window(AxMapControl axmapcontrol)
        {
            InitializeComponent();
            this.axmapcontrol1 = axmapcontrol; 
           
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "shp文件(*.shp)|*.shp|图层文件(*.lyr)|*.lyr|All Files(*.*)|*.**";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (OpenFile.FileName != null || OpenFile.FileName != "")
                {
                    textBox1.Text = OpenFile.FileName;
                }
            }
            else
            {
                MessageBox.Show("请选择文件");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "shp文件(*.shp)|*.shp|图层文件(*.lyr)|*.lyr|All Files(*.*)|*.**";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (OpenFile.FileName != null || OpenFile.FileName != "")
                {
                    textBox2.Text = OpenFile.FileName;
                }
            }
            else
            {
                MessageBox.Show("请选择文件");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "shp文件(*.shp)|*.shp";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                if (OpenFile.FileName != null || OpenFile.FileName != "")
                {
                    textBox3.Text = OpenFile.FileName;
                }
            }
            else
            {
                MessageBox.Show("请选择文件");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("输入要素不可以为空\n裁剪要素不可以为空\n输出要素不可以为空");
            }
            else if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("输入要素不可以为空\n裁剪要素不可以为空");
                return;
            }
            else if (textBox1.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("输入要素不可以为空\n输出要素不可以为空");
                return;
            }
            else if (textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("裁剪要素不可以为空\n输出要素不可以为空");
                return;
            }else if (textBox2.Text == "")
            {
                MessageBox.Show("裁剪要素不可以为空");
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("输入要素不可以为空");
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("输出要素不可以为空");
            }
            else
            {
                Geoprocessor gp = new Geoprocessor();
                ESRI.ArcGIS.AnalysisTools.Clip clipTool = new Clip(textBox1.Text, textBox2.Text, textBox3.Text);
                gp.OverwriteOutput = true;
                gp.Execute(clipTool, null);
                GeoUtil util = new GeoUtil(axmapcontrol1);
                util.loadShapefile(textBox3.Text); 
                this.Close(); 
            }







        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
