using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Multi_IAP_Application
{
    public partial class LoadROM : Form
    {
        public string ROM_Path;
        public string romVer;
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
                ROM_Path = openFileDialog.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                romVer = textBox2.Text;
        }
    }
}
