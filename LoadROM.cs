using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Multi_IAP_Application
{
    public partial class LoadROM : Form
    {
        public string ROM_Path;
        public string romVer;
        public string ROM_Path2;
        public string romVer2;

        public LoadROM()
        {
            InitializeComponent();
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

                textBox1.Text = openFileDialog.FileName;
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
                ROM_Path2 = openFileDialog.FileName;

            }
        }
    }
}
