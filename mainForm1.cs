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


using System.Management;
using System.Runtime.InteropServices;


namespace Multi_IAP_Application
{
    public partial class mainForm1 : Form
    {
        string ROM_Path;
        //string ROM_VERSION;
        IAP_Form iap_form1 ;
        IAP_Form iap_form2 ;
        IAP_Form iap_form3 ;
        IAP_Form iap_form4 ;

        int Bin_Size;
        BinaryReader bin_read;
        byte[] bytes;
        //int index = 0;



        public mainForm1()
        {
            InitializeComponent();
            CheckRegState();

            iap_form1 = new IAP_Form();
            iap_form2 = new IAP_Form();
            iap_form3 = new IAP_Form();
            iap_form4 = new IAP_Form();


            iap_form1.MdiParent = this;
            iap_form2.MdiParent = this;
            iap_form3.MdiParent = this;
            iap_form4.MdiParent = this;

                    

            iap_form1.Show();
            iap_form2.Show();
            iap_form3.Show();
            iap_form4.Show();



            load_bin_file();
        }


        private bool isReg()
        {
            //判断软件是否注册
            bool isReg = false;
            SoftReg softReg = new SoftReg();

            RegistryKey retkey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Mobike").CreateSubKey("Register.INI");         // 新版本注册版的存放
            RegistryKey retkey1 = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("mySoftWare").CreateSubKey("Register.INI");  // 老版本注册版的存放


            foreach (string strRNum in retkey.GetSubKeyNames())
            {
                if (strRNum == softReg.GetRNum())
                {
                    isReg = true;
                }
            }

            foreach (string strRNum in retkey1.GetSubKeyNames())
            {
                if (strRNum == softReg.GetRNum())
                {
                    isReg = true;
                }
            }
            return isReg;
        }


        public void CheckRegState()
        {
            //检测注册
            if (isReg() == false)
            {
                this.Text += " 【未注册】";
                RegForm regform = new RegForm();
                regform.ShowDialog();

                //Application.Exit();
                //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return;
            }
            else
            {
                this.Text += " 【已注册】";
            }

        }



        public void load_bin_file()
        {
            LoadROM loadRomForm = new LoadROM();

            //loadRomForm.Show();
           
        }

        private void 加载固件ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadROM loadRomForm = new LoadROM();
            loadRomForm.ShowDialog();
            if (loadRomForm.DialogResult == DialogResult.OK)
            {
                ROM_Path = loadRomForm.ROM_Path;
                Console.WriteLine(ROM_Path);
                toolStripStatusLabel1.Text = "固件路径:" + ROM_Path + " " + "固件版本:" + loadRomForm.romVer;
                toolStripStatusLabel1.BackColor = Color.Transparent;
                //加载bin文件到数组


                //FileInfo info = new FileInfo(ROM_Path);
                //int size = (int)info.Length;
                //toolStripStatusLabel2.Text = ("文件大小 = " + size);
                //Bin_Size = size;

                //FileStream fs = new FileStream(ROM_Path, FileMode.Open, FileAccess.Read);
                //bin_read = new BinaryReader(fs);
                //bytes = new byte[size];
                //bytes = bin_read.ReadBytes(size);
                ////button1.Enabled = false;
                //bin_read.Close();
                //fs.Close();

                //iap_form1.DownBytes = bytes;
                //iap_form2.DownBytes = bytes;
                //iap_form3.DownBytes = bytes;
                //iap_form4.DownBytes = bytes;

                //iap_form1.updateROMVer = loadRomForm.romVer;
                //iap_form2.updateROMVer = loadRomForm.romVer;
                //iap_form3.updateROMVer = loadRomForm.romVer;
                //iap_form4.updateROMVer = loadRomForm.romVer;
            }
        }

        private void 横向排序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 层叠排列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void 垂直平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void 页面排列ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
