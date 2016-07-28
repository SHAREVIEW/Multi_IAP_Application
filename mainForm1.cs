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

namespace Multi_IAP_Application
{
    public partial class mainForm1 : Form
    {
        string ROM_Path;
        IAP_Form iap_form1 ;
        IAP_Form iap_form2 ;
        IAP_Form iap_form3 ;
        IAP_Form iap_form4 ;

        int Bin_Size;
        BinaryReader bin_read;
        byte[] bytes;
        int index = 0;

        public mainForm1()
        {
            InitializeComponent();
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

           // this.LayoutMdi(MdiLayout.TileHorizontal);
            //this.LayoutMdi(MdiLayout.TileHorizontal);
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
                FileInfo info = new FileInfo(ROM_Path);
                int size = (int)info.Length;
                toolStripStatusLabel2.Text = ("文件大小 = " + size);
                Bin_Size = size;

                FileStream fs = new FileStream(ROM_Path, FileMode.Open, FileAccess.Read);
                bin_read = new BinaryReader(fs);
                bytes = new byte[size];
                bytes = bin_read.ReadBytes(size);
                //button1.Enabled = false;
                bin_read.Close();
                fs.Close();

                iap_form1.DownBytes = bytes;
                iap_form2.DownBytes = bytes;
                iap_form3.DownBytes = bytes;
                iap_form4.DownBytes = bytes;

                iap_form1.updateROMVer = loadRomForm.romVer;
                iap_form2.updateROMVer = loadRomForm.romVer;
                iap_form3.updateROMVer = loadRomForm.romVer;
                iap_form4.updateROMVer = loadRomForm.romVer;
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
