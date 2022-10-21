using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogAnal
{
    public partial class Form1 : Form
    {
        Dictionary<string, int> sensor_ON_real = new Dictionary<string, int>();
        Dictionary<string, int> sensor_OFF_real = new Dictionary<string, int>();
        Dictionary<string, int> sensor_ON_test = new Dictionary<string, int>();
        Dictionary<string, int> sensor_OFF_test = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path_real = @"C:\Users\USER\Desktop\새 폴더 (7)\Real_Log.txt";
            LogToDictionary(path_real, sensor_ON_real, sensor_OFF_real);
            string path_test = @"C:\Users\USER\Desktop\새 폴더 (7)\Test_Log.txt";
            LogToDictionary(path_test, sensor_ON_test, sensor_OFF_test);
                      
            foreach(string s in sensor_ON_real.Keys)
            {
                if(sensor_ON_test.ContainsKey(s))
                {
                    if(sensor_ON_real[s] != sensor_ON_test[s])
                    {
                        textBox1.Text = "불일치";
                        richTextBox1.AppendText($"MAC address : {s}  //  realLog : {sensor_ON_real[s]}  //  testLog : {sensor_ON_test[s]}  //  [ON]");
                        richTextBox1.AppendText("\r\n");
                    }
                        
                }
                else
                {
                    textBox1.Text = "불일치";
                    richTextBox1.AppendText($"{s}가 불일치");
                    richTextBox1.AppendText("\r\n");
                }               
            }
            if (textBox1.Text != "불일치")
                textBox1.Text = "일치";

            foreach (string s in sensor_OFF_real.Keys)
            {
                if (sensor_OFF_test.ContainsKey(s))
                {
                    if (sensor_OFF_real[s] != sensor_OFF_test[s])
                    {
                        textBox2.Text = "불일치";
                        richTextBox1.AppendText($"MAC address : {s}  //  realLog : {sensor_OFF_real[s]}  //  testLog : {sensor_OFF_test[s]}  //  [OFF]");
                        richTextBox1.AppendText("\r\n");
                    }

                }
                else
                {
                    textBox2.Text = "불일치";
                    richTextBox1.AppendText($"{s}가 불일치");
                    richTextBox1.AppendText("\r\n");
                }
            }
            if (textBox2.Text != "불일치")
                textBox2.Text = "일치";
        }

        public static string LoadFile(string FilePath)
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private void LogToDictionary(string filePath, Dictionary<string, int> sensor_on, Dictionary<string, int> sensor_off)
        {
            string totalSensorLog = LoadFile(filePath);
            string[] sensorLogArray = totalSensorLog.Split('\n');
            List<string> undatedSensorArray = new List<string>();
            foreach (string sensorLog in sensorLogArray)
            {
                if (sensorLog.Contains("[UpdateSensor]"))
                {
                    //[UpdateSensor]가 포함된 로그 한 줄 List추가
                    undatedSensorArray.Add(sensorLog);

                    // [UpdateSensor]의 센서 MAC
                    string sensor_MAC = sensorLog.Substring(58, 12);

                    // true와 false여부에 따른 두개의 딕셔너리에 추가하는 구문
                    if (sensorLog[71] == 'T')
                    {
                        if (!sensor_on.ContainsKey(sensor_MAC))
                        {
                            sensor_on.Add(sensor_MAC, 1);
                        }
                        else
                        {
                            sensor_on[sensor_MAC]++;
                        }
                    }
                    else
                    {
                        if (!sensor_off.ContainsKey(sensor_MAC))
                        {
                            sensor_off.Add(sensor_MAC, 1);
                        }
                        else
                        {
                            sensor_off[sensor_MAC]++;
                        }
                    }

                }
            }         
        }

    }
}
