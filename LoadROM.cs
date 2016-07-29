using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.IO.Ports;
using Microsoft.Win32;

namespace Multi_IAP_Application
{
    public partial class LoadROM : Form
    {
        public string ROM_Path;
        public string romVer;
        public string ROM_Path2;
        public string romVer2;

        public string ROMLB3_3_Path;
        public string ROMLB_3_3_Ver;
        public string ROMLB3_4_Path;
        public string ROMLB_3_4_Ver;


        public LoadROM()
        {
            InitializeComponent();

            ROMLB3_3_Path = Properties.Settings.Default.LB3_3_FilePath;
            ROMLB3_4_Path = Properties.Settings.Default.LB3_4_FilePath;


            textBox1.Text = ROMLB3_3_Path;
            textBox3.Text = ROMLB3_4_Path;


            listView1.Items[0].SubItems[1].Text = Path.GetFileName(ROMLB3_3_Path);
            listView1.Items[1].SubItems[1].Text = Path.GetFileName(ROMLB3_4_Path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E:\\";
            openFileDialog.Filter = "Bin文件(*.bin)|*.bin";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;//允许同时选择多个文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string strFileName = openFileDialog.FileName;
                listView1.Items[0].SubItems[1].Text = Path.GetFileName(strFileName);

                string tPath = Properties.Settings.Default.LB3_3_FilePath;
            
                textBox1.Text = openFileDialog.FileName;
                Properties.Settings.Default.LB3_3_FilePath = textBox1.Text;
                Properties.Settings.Default.Save();  // save 文件路径

                ROM_Path = openFileDialog.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                romVer = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E:\\";
            openFileDialog.Filter = "Bin文件(*.bin)|*.bin";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;//允许同时选择多个文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
   
                string strFileName = openFileDialog.FileName;
                listView1.Items[1].SubItems[1].Text = Path.GetFileName(strFileName);

                textBox3.Text = openFileDialog.FileName;

                Properties.Settings.Default.LB3_4_FilePath = textBox3.Text;
                Properties.Settings.Default.Save();  // save 文件路径
                string tPath = Properties.Settings.Default.LB3_4_FilePath;
            


                ROM_Path2 = openFileDialog.FileName;

            }
        }
    }
}
