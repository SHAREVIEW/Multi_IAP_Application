using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Multi_IAP_Application
{
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();

            this.txtHardware.Text = softReg.GetMNum();
        }

        public static bool state = true;  //软件是否为可用状态
        SoftReg softReg = new SoftReg();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtLicence.Text !="") && (txtLicence.Text == softReg.GetRNum()))
                {
                    MessageBox.Show("注册成功！重启软件后生效！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Mobike").CreateSubKey("Register.INI").CreateSubKey(txtLicence.Text);
                    retkey.SetValue("UserName", "Rsoft");
                    this.Close();
               
                    Application.Exit();     // 退出当前程序

                    // 重新打开进程
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    MessageBox.Show("注册码错误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLicence.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
