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
using System.Threading;

namespace Multi_IAP_Application
{
    public partial class IAP_Form : Form
    {
        int isSysTimeRight = 0;

        int time_out = 0;
        int iap_send_state = 0;
        int Bin_Size;

        bool unlock_cmd=false;

        string time_display;
        string hardware_version;
        string software_version;

        int index = 0;
        int ready_end = 0;
        int rx_count = 0;
        int time_count = 0;

        int DownSpeed = 1;//1 = 500字节 2: = 1024， 3：2048  4: 2*2048
        bool autoDownMode = true;

        public byte[] DownBytes;//存储需要下载的数据

        string LocalROMVer;//串口检测到的ROM版本
        public string updateROMVer;//升级到的软件版本号

        bool readyUndate = true;//准备升级

        public IAP_Form()
        {
            InitializeComponent();

            foreach (string vPortName in SerialPort.GetPortNames())
            {
                this.comboBox1.Items.Add(vPortName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "打开串口")
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort1.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("串口错误");
                    return;
                }
                button1.Text = "关闭串口";
                comboBox1.Enabled = false;
               // comboBox1.Text = comboBox1.Items[0].ToString();
              // comboBox1.Text = comboBox1.SelectedIndex.ToString();
                // label3.Text = comboBox1.Text[3].ToString();
                int com_index = comboBox1.SelectedIndex+1;
                label3.Text = com_index.ToString();
                label3.BackColor = Color.Yellow;

                timer_sec.Start();
            }
            else
            {
                serialPort1.Close();
                button1.Text = "打开串口";
                comboBox1.Enabled = true;
                label3.Text = "N";
                label3.BackColor = Color.Transparent;
                timer_sec.Stop();
                timer1.Stop();
            }
        }
        private bool CheckCurrentTime(string time)
        {
            DateTime dt = DateTime.Now;
            string str_datetime = dt.ToShortDateString().ToString() + " " + time;
            DateTime c_dt = Convert.ToDateTime(str_datetime);

            if (dt > c_dt.AddSeconds(40))
            {
                return false;
            }
            else if (c_dt > dt.AddSeconds(40))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void process_iap_recv(string s)
        {
            //修正时间[ INIT_INFO][11:04:10] Encrypt ok! 
            if ((s.IndexOf("][") > 0) && (isSysTimeRight == 0))
            {
                int n1 = s.IndexOf("][");
                if (n1 < 0) return;
                string tmp = s.Substring(n1 + 2);
                int n2 = tmp.IndexOf("]");
                if (n2 < 0) return;
                string str = tmp.Substring(0, n2);
                time_display = tmp.Substring(0, n2); 
                bool b = CheckCurrentTime(str);
                if (b == false)
                {
                    isSysTimeRight = 1;
                    label2.ForeColor = Color.Red;
                    label2.Text = "时间错误--请更新时间 \r\n" + time_display;
                    
                    string setTimeStr = DateTime.Now.ToString("yyyy,MM,dd,HH,mm,ss");
                    SendToSerialPort("AT+SETTIME=" + setTimeStr);
                }
            }
            else if (s.IndexOf("Set System Time OK") >= 0)
            {
                if (s.IndexOf("][") > 0)
                {
                    int n1 = s.IndexOf("][");
                    if (n1 < 0) return;
                    string tmp = s.Substring(n1 + 2);
                    int n2 = tmp.IndexOf("]");
                    if (n2 < 0) return;
                    time_display = tmp.Substring(0, n2);
                 }

                    isSysTimeRight = 0;
                    label2.ForeColor = Color.Black;
                    label2.Text = "系统时间已更新\r\n" + time_display; ;
               
            }
            else if (s.IndexOf("MB_LB") >= 0)
            {
                int n1 = s.IndexOf("MB_LB");
                //string tmp = s.Substring(n1 + 7);
                //int n2 = tmp.IndexOf(")");
                hardware_version = s.Substring(n1 + 3, 5);
                label5.Text = "硬件版本: " + hardware_version + "\r\n" + "软件版本: " + software_version;
            }
            else if (s.IndexOf("Verson") >= 0)
            {
                int m1 = s.IndexOf("Verson");
                string tmp1 = s.Substring(m1 + 7);
                int m2 = tmp1.IndexOf(")");
                //richTextBox2.AppendText("固件版本:" + tmp.Substring(0, n2) + "\r\n");
                software_version = tmp1.Substring(0, m2);
                label5.Text = "硬件版本: " + hardware_version + "\r\n" + "软件版本: " + software_version;

                readyUndate = false;
                int n1 = s.IndexOf("Verson");
                string tmp = s.Substring(n1 + 7);
                int n2 = tmp.IndexOf(")");
                if (n2 < 0) return;
                LocalROMVer = tmp.Substring(0, n2).Trim();
                richTextBox1.AppendText("#################################\r\n");
                richTextBox1.AppendText("检测到的软件版本号为：" + LocalROMVer + "\r\n");
                richTextBox1.AppendText("#################################\r\n");

                if (autoDownMode == false)
                {
                    return;
                }

                if (updateROMVer == "")
                {
                    return;
                }


                if (LocalROMVer == updateROMVer)
                {

                    if (unlock_cmd == false)
                    {
                        unlock_cmd = true;
                        label2.Text = "版本相同，发送开锁命令";
                        serialPort1.Write("AT+MOTOR\r\n");
                        serialPort1.Write("AT+MOTOR\r\n");
                        serialPort1.Write("AT+MOTOR\r\n");
                        readyUndate = true;
                        richTextBox1.Clear();
                    }
                }
                else
                {
                    label5.Text = "硬件版本: " + "\r\n" + "软件版本: " ;
                    startIAP();
                }
                }
           
            if (s.IndexOf("AD+IAPACK=1") >= 0)
            {
                DownSpeed = 1;
                iap_send_state = 1;
                time_out = 0;
                label2.Text = "接收下载命令成功 500字节格式";
                progressBar1.Maximum = Bin_Size / 500;
            }
            else if (s.IndexOf("AD+IAPACK=2") >= 0)
            {
                DownSpeed = 2;
                iap_send_state = 1;
                time_out = 0;
                label2.Text = "接收下载命令成功 1k格式";
                progressBar1.Maximum = Bin_Size / 1024;
            }
            else if (s.IndexOf("AD+IAPACK=3") >= 0)
            {
                DownSpeed = 3;
                iap_send_state = 1;
                time_out = 0;
                label2.Text = "接收下载命令成功 2k格式";
                progressBar1.Maximum = Bin_Size / 2048;
            }
            else if (s.IndexOf("AD+IAPACK=4") >= 0)
            {
                DownSpeed = 4;
                iap_send_state = 1;
                time_out = 0;
                label2.Text = "接收下载命令成功 4k格式";
                progressBar1.Maximum = Bin_Size / 4096;
            }
            else if (s.IndexOf("AD+IAPSIZEACK") >= 0)
            {
                iap_send_state = 2;
                time_out = 0;
                label2.Text = "下载文件长度命令成功";
            }
            else if (s.IndexOf("AD+IAPDOWNACK=1") >= 0)
            {
                index++;

                
                if (index > progressBar1.Maximum)
                {
                    progressBar1.Value = progressBar1.Maximum;
                    label2.Text = "下载中 进度：" + progressBar1.Value + " / " + progressBar1.Maximum +
                        "  耗时：" + time_count + " 秒";
                }
                else
                {
                    progressBar1.Value = index;
                    label2.Text = "下载中 进度：" + progressBar1.Value + " / " + progressBar1.Maximum +
                        "  耗时：" + time_count + " 秒";
                }

                iap_send_state = 2;
                time_out = 0;
                if (ready_end == 1)
                {
                    iap_send_state = 3;
                }
            }
            else if (s.IndexOf("AD+IAPENDACK=1") >= 0)
            {
                iap_send_state = 4;
                time_out = 0;
                label2.Text = "固件下载成功" + "  总耗时：" + time_count + " 秒"; ;
            }
            else if (s.IndexOf("AD+IAPENDACK=2") >= 0)
            {
                iap_send_state = 5;
                time_out = 0;
                label2.Text = "固件下载失败";
            }
        }

        private void process_iap_send()
        {
            switch (iap_send_state)
            {
                case 1:
                    if (time_out <= 0)
                    {
                        uint crc = CRC.calcCRC(DownBytes);
                        string str = "AD+IAPSIZE=" + Bin_Size + "," + crc;
                        SendToSerialPort(str);
                        time_out = 500;
                    }
                    time_out--;
                    break;

                case 2:
                    if (time_out <= 0)
                    {
                        string str2 = "";
                        if (DownSpeed == 1)
                        {
                            if ((index + 1) * 500 < Bin_Size)
                            {
                                byte[] buff = new byte[500];
                                Array.Copy(DownBytes, index * 500, buff, 0, 500);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + "500" + "," + crc + "," + s;
                                string time = DateTime.Now.ToString("hh:mm:ss fff");
                                richTextBox1.AppendText("Send " + time + " " + "500字节" + "\r\n");
                            }
                            else
                            {
                                int len = Bin_Size - 500 * index;
                                if (len <= 0) break;
                                byte[] buff = new byte[len];
                                Array.Copy(DownBytes, index * 500, buff, 0, len);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + len + "," + crc + "," + s;
                                ready_end = 1;
                            }
                        }
                        else if (DownSpeed == 2)
                        {
                            if ((index + 1) * 1024 < Bin_Size)
                            {
                                byte[] buff = new byte[1024];
                                Array.Copy(DownBytes, index * 1024, buff, 0, 1024);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + "1024" + "," + crc + "," + s;
                                string time = DateTime.Now.ToString("hh:mm:ss fff");
                                richTextBox1.AppendText("Send " + time + " " + "1024字节" + "\r\n");
                            }
                            else
                            {
                                int len = Bin_Size - 1024 * index;
                                if (len <= 0) break;
                                byte[] buff = new byte[len];
                                Array.Copy(DownBytes, index * 1024, buff, 0, len);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + len + "," + crc + "," + s;
                                ready_end = 1;
                            }
                        }
                        else if (DownSpeed == 3)
                        {
                            if ((index + 1) * 2048 < Bin_Size)
                            {
                                byte[] buff = new byte[2048];
                                Array.Copy(DownBytes, index * 2048, buff, 0, 2048);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + "2048" + "," + crc + "," + s;
                                string time = DateTime.Now.ToString("hh:mm:ss fff");
                                richTextBox1.AppendText("Send " + time + " " + "2048字节" + "\r\n");
                            }
                            else
                            {
                                int len = Bin_Size - 2048 * index;
                                if (len <= 0) break;
                                byte[] buff = new byte[len];
                                Array.Copy(DownBytes, index * 2048, buff, 0, len);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + len + "," + crc + "," + s;
                                ready_end = 1;
                            }
                        }
                        else
                        {
                            if ((index + 1) * 4096 < Bin_Size)
                            {
                                byte[] buff = new byte[4096];
                                Array.Copy(DownBytes, index * 4096, buff, 0, 4096);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + "4096" + "," + crc + "," + s;
                                string time = DateTime.Now.ToString("hh:mm:ss fff");
                                richTextBox1.AppendText("Send " + time + " " + "4096字节" + "\r\n");
                            }
                            else
                            {
                                int len = Bin_Size - 4096 * index;
                                if (len <= 0) break;
                                byte[] buff = new byte[len];
                                Array.Copy(DownBytes, index * 4096, buff, 0, len);
                                uint crc = CRC.calcCRC(buff);
                                string s = BitConverter.ToString(buff).Replace("-", string.Empty);
                                str2 = "AD+IAPDOWN=" + index + "," + len + "," + crc + "," + s;
                                ready_end = 1;
                            }
                        }
                        serialPort1.Write(str2 + "\r\n");
                        time_out = 100;
                    }
                    time_out--;
                    break;

                case 3:
                    if (time_out <= 0)
                    {
                        string str = "AD+IAPEND=1";
                        SendToSerialPort(str);
                        time_out = 100;
                    }
                    time_out--;
                    break;

                case 4:

                    richTextBox1.AppendText("固件下载结束OK\r\n");
                    richTextBox1.AppendText("=========================\r\n");
                    richTextBox1.AppendText("============Ok===========\r\n");
                    richTextBox1.AppendText("=========================\r\n");
                    timer1.Stop();
                    timer_sec.Stop();
                    label2.BackColor = Color.LightGreen;
                    break;

                case 5:
                    timer1.Stop();
                    richTextBox1.AppendText("固件下载结束ERROR\r\n");
                    label2.BackColor = Color.Red;
                    break;

            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int i = serialPort1.BytesToRead;
                byte[] buf = new byte[i];
                string time = DateTime.Now.ToString("hh:mm:ss fff");
                string s = serialPort1.ReadLine();

                if (s.Length <= 0) return;

                this.BeginInvoke((EventHandler)delegate
                {
                    try
                    {
                        if (readyUndate == true)//准备好升级，当检测到有数据输入，则发生获取版本请求
                        {
                            if (updateROMVer != null)
                            {
                                serialPort1.Write("AT+WHO\r\n");
                            }
                            
                        }
                        process_iap_recv(s);
                        richTextBox1.AppendText(s);

                    }
                    catch (System.FormatException)
                    {
                        System.Console.WriteLine("System.FormatException");
                        System.Console.WriteLine(s);
                    }
                }
                );
            }
            catch (IOException)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            autoDownMode = true;
            startIAP();
        }

        void startIAP()
        {
            if (serialPort1.IsOpen == false)
            {
                MessageBox.Show("请打开串口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DownBytes == null)
            {
                MessageBox.Show("请添加固件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Bin_Size = DownBytes.Length;
            progressBar1.Maximum = Bin_Size / 500;

            time_count = 0;
            //richTextBox1.Clear();
            label2.BackColor = Color.Transparent;
            label2.Text = "发送下载命令";
            timer1.Start();
            SendToSerialPort("AT+PWD=6789");
            string str = "AT+IAP=1";
            SendToSerialPort(str);
            label5.Text = "硬件版本: " + "\r\n" + "软件版本: ";
            iap_send_state = 0;
            index = 0;
            ready_end = 0;
            //timer_sec.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            autoDownMode = false;
            label2.Text = "下载终止";
            progressBar1.Value = 0;
            timer1.Stop();
            string str = "AD+IAPEND=0";
            SendToSerialPort(str);
            iap_send_state = 0;
            //timer_sec.Stop();
        }
        private void SendToSerialPort(string data)
        {
            if (serialPort1.IsOpen)
            {
                string time = DateTime.Now.ToString("hh:mm:ss fff");
                serialPort1.Write(data + "\r\n");

                richTextBox1.AppendText("Send " + time + " " + data + "\r\n");
            }
        }

        private void timer_sec_Tick(object sender, EventArgs e)
        {
            if (readyUndate == true)
            {
                serialPort1.Write("AT+WHO\r\n");
                richTextBox1.AppendText(serialPort1.PortName + "  AT+WHO\r\n");
            }
            else
            {
                time_count++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            process_iap_send();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Write("AT+INFO\r\n");
            serialPort1.Write("AT+WHO\r\n");
        }
    }
}
