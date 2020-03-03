using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;


namespace FindMessage
{
    public partial class Form1 : Form
    {
        public static Form1 form1;

        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool right_num = true;
            foreach (var item in textBox1.Text)
            {
                if (item > 57 || item < 48 && item != 42)
                {
                    right_num = false;
                }
            }

            if (textBox1.Text.Length != 11 || !right_num)
            {
                MessageBox.Show("请输入正确电话号码");
            }
            else
            {
                richTextBox1.Clear();
                progressBar1.Value = 0;
                backgroundWorker1.RunWorkerAsync();              //调用backgroundWorker1_DoWork
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Focus();//获取焦点
            richTextBox1.Select(richTextBox1.TextLength, 0);//光标定位到文本最后
            richTextBox1.ScrollToCaret();//滚动到光标处
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //子线程中
            MessageBox.Show("即将开始检索");
            find f = new find();
            f.Findstart(textBox1.Text, textBox2.Text);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //主线程中
            progressBar1.Value = e.ProgressPercentage;
            label3.Text = progressBar1.Value.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
            label3.Text = "100%";
            MessageBox.Show("检索完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("ilogs.txt");
        }
    }
}
