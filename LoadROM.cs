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
        public string ROM_VERSION;
        public string romVer;
        public string ROM_Path2;
        public string romVer2;

        public string ROMLB3_3_Path;
        public string ROMLB_3_3_HardwareVer;
        public string ROMLB_3_3_SoftwareVer;

        public string ROMLB3_4_Path;
        public string ROMLB_3_4_HardwareVer;
        public string ROMLB_3_4_SoftwareVer;

        public string ROMLB4_4_Path;
        public string ROMLB_4_4_HardwareVer;
        public string ROMLB_4_4_SoftwareVer;


        int ROMLB_3_3_Bin_Size;
        BinaryReader ROMLB_3_3_bin_read;
        byte[] ROMLB_3_3_bytes;


        int ROMLB_3_4_Bin_Size;
        BinaryReader ROMLB_3_4_bin_read;
        byte[] ROMLB_3_4_bytes;

        int ROMLB_4_4_Bin_Size;
        BinaryReader ROMLB_4_4_bin_read;
        byte[] ROMLB_4_4_bytes;



        public LoadROM()
        {
            InitializeComponent();

            ROMLB3_3_Path = Properties.Settings.Default.LB3_3_FilePath;
            ROMLB3_4_Path = Properties.Settings.Default.LB3_4_FilePath;
            ROMLB4_4_Path = Properties.Settings.Default.LB4_4_FilePath;

            textBox1.Text = ROMLB3_3_Path;
            textBox3.Text = ROMLB3_4_Path;
            textBox4.Text = ROMLB4_4_Path;

            ROM_Path = ROMLB3_3_Path;

            listView1.Items[0].SubItems[1].Text = Path.GetFileName(ROMLB3_3_Path);
            listView1.Items[1].SubItems[1].Text = Path.GetFileName(ROMLB3_4_Path);
            listView1.Items[2].SubItems[1].Text = Path.GetFileName(ROMLB4_4_Path);


            check_ver();
            ROM_VERSION = ROMLB_3_3_SoftwareVer;
            textBox2.Text = ROMLB_3_3_SoftwareVer;
            romVer = textBox2.Text;

        }

        public void check_ver()
        {
            if (ROMLB3_3_Path != "")
            {
                FileInfo info1 = new FileInfo(ROMLB3_3_Path);
                int size1 = (int)info1.Length;
                ROMLB_3_3_Bin_Size = size1;
                FileStream fs1 = new FileStream(ROMLB3_3_Path, FileMode.Open, FileAccess.Read);
                ROMLB_3_3_bin_read = new BinaryReader(fs1);
                ROMLB_3_3_bytes = new byte[ROMLB_3_3_Bin_Size];
                ROMLB_3_3_bytes = ROMLB_3_3_bin_read.ReadBytes(ROMLB_3_3_Bin_Size);
                ROMLB_3_3_bin_read.Close();
                fs1.Close();
                string filename1 = Path.GetFileName(ROMLB3_3_Path);

                if (filename1.Length != 27)
                    MessageBox.Show("文件名长度不对");
                if (filename1.Length == 27)
                {
                    ROMLB_3_3_HardwareVer = filename1.Substring(13, 5);
                    ROMLB_3_3_SoftwareVer = filename1.Substring(19, 4);
                }
                if (ROMLB_3_3_HardwareVer != "LB3_3")
                    MessageBox.Show("固件加载错误！");

                listView1.Items[0].SubItems[2].Text = ROMLB_3_3_HardwareVer;
                listView1.Items[0].SubItems[3].Text = ROMLB_3_3_SoftwareVer;
                size1 = size1 / 1024;
                listView1.Items[0].SubItems[4].Text = size1.ToString() + "KB";
            }

            if (ROMLB3_4_Path != "")
            {
                FileInfo info2 = new FileInfo(ROMLB3_4_Path);
                int size2 = (int)info2.Length;
                ROMLB_3_4_Bin_Size = size2;
                FileStream fs2 = new FileStream(ROMLB3_4_Path, FileMode.Open, FileAccess.Read);
                ROMLB_3_4_bin_read = new BinaryReader(fs2);
                ROMLB_3_4_bytes = new byte[ROMLB_3_4_Bin_Size];
                ROMLB_3_4_bytes = ROMLB_3_4_bin_read.ReadBytes(ROMLB_3_4_Bin_Size);
                ROMLB_3_4_bin_read.Close();
                fs2.Close();
                string filename2 = Path.GetFileName(ROMLB3_4_Path);

                if (filename2.Length != 27)
                    MessageBox.Show("文件名长度不对");
                if (filename2.Length == 27)
                {
                    ROMLB_3_4_HardwareVer = filename2.Substring(13, 5);
                    ROMLB_3_4_SoftwareVer = filename2.Substring(19, 4);
                }
                if (ROMLB_3_4_HardwareVer != "LB3_4")
                    MessageBox.Show("固件加载错误！");

                listView1.Items[1].SubItems[2].Text = ROMLB_3_4_HardwareVer;
                listView1.Items[1].SubItems[3].Text = ROMLB_3_4_SoftwareVer; // 软件版本
                size2 = size2 / 1024;
                listView1.Items[1].SubItems[4].Text = size2.ToString() + "KB";
            }

            if (ROMLB4_4_Path != "")
            {
                FileInfo info3 = new FileInfo(ROMLB4_4_Path);
                int size3 = (int)info3.Length;
                ROMLB_4_4_Bin_Size = size3;
                FileStream fs3 = new FileStream(ROMLB4_4_Path, FileMode.Open, FileAccess.Read);
                ROMLB_4_4_bin_read = new BinaryReader(fs3);
                ROMLB_4_4_bytes = new byte[ROMLB_4_4_Bin_Size];
                ROMLB_4_4_bytes = ROMLB_4_4_bin_read.ReadBytes(ROMLB_4_4_Bin_Size);
                ROMLB_4_4_bin_read.Close();
                fs3.Close();
                string filename3 = Path.GetFileName(ROMLB4_4_Path);
                if (filename3.Length != 27)
                    MessageBox.Show("文件名长度不对");
                if (filename3.Length == 27)
                {
                    ROMLB_4_4_HardwareVer = filename3.Substring(13, 5);
                    ROMLB_4_4_SoftwareVer = filename3.Substring(19, 4);

                    if (ROMLB_4_4_HardwareVer != "LB4_4")
                        MessageBox.Show("固件加载错误！");
                }
                listView1.Items[2].SubItems[2].Text = ROMLB_4_4_HardwareVer;
                listView1.Items[2].SubItems[3].Text = ROMLB_4_4_SoftwareVer; // 软件版本
                size3 = size3 / 1024;
                listView1.Items[2].SubItems[4].Text = size3.ToString() + "KB";
            }


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

                ROMLB3_3_Path = openFileDialog.FileName;
                listView1.Items[0].SubItems[1].Text = Path.GetFileName(ROMLB3_3_Path);

                textBox1.Text = ROMLB3_3_Path;
                Properties.Settings.Default.LB3_3_FilePath = ROMLB3_3_Path;
                Properties.Settings.Default.Save();  // save 文件路径
                check_ver();

                ROM_Path = ROMLB3_3_Path;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text= ROMLB_3_3_SoftwareVer;

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

                ROMLB3_4_Path = openFileDialog.FileName;
                listView1.Items[1].SubItems[1].Text = Path.GetFileName(ROMLB3_4_Path);

                textBox3.Text = ROMLB3_4_Path;

                Properties.Settings.Default.LB3_4_FilePath = ROMLB3_4_Path;
                Properties.Settings.Default.Save();  // save 文件路径
                string tPath = Properties.Settings.Default.LB3_4_FilePath;
                check_ver();
                ROM_Path = ROMLB3_3_Path;

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E:\\";
            openFileDialog.Filter = "Bin文件(*.bin)|*.bin";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;//允许同时选择多个文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                ROMLB4_4_Path = openFileDialog.FileName;
                listView1.Items[2].SubItems[1].Text = Path.GetFileName(ROMLB4_4_Path);

                textBox4.Text = ROMLB4_4_Path;
                Properties.Settings.Default.LB4_4_FilePath = ROMLB4_4_Path;
                Properties.Settings.Default.Save();  // save 文件路径
                check_ver();

                ROM_Path = ROMLB4_4_Path;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
