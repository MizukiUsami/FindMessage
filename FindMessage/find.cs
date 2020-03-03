using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace FindMessage
{
    class find
    {
        int unknown_number;
        List<int> record_unkown = new List<int>(); //获取*号位
        List<string> list = new List<string>();

        public void Findstart(string tel, string Ct)
        {
            for (int i = 0; i < 11; i++)
            {
                if (tel[i] == '*')
                {
                    unknown_number += 1;
                    record_unkown.Add(i);
                }
            }

            double maxtimes = Math.Pow(10, unknown_number);

            for (int j = 0; j < maxtimes; j++)
            {
                string fill_num = j.ToString().PadLeft(unknown_number, '0');
                char[] combine_num = tel.ToCharArray();
                for (int i = 0; i < record_unkown.ToArray().Length; i++)
                {
                    combine_num[record_unkown[i]] = fill_num[i];
                }

                string ss = HttpGet("http://mobsec-dianhua.baidu.com/dianhua_api/open/location?tel=" + new string(combine_num));

                /**/
                Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);
                MatchCollection mc = re.Matches(ss);
                foreach (Match ma in mc)
                {
                    list.Add(ma.Value);
                }

                //Console.WriteLine(list[2]);   //电话
                //Console.WriteLine(list[10]);  //城市

                /**/

                if (list.Count != 37)
                {
                    continue;
                }

                string number = list[2];
                string city = list[10];

                if (Ct.Length != 0)
                {
                    if (city == Ct)
                    {
                        LogWrite(number + " " + city + Environment.NewLine);
                    }
                }
                else
                {
                    LogWrite(number + " " + city + Environment.NewLine);
                }

                list.Clear();

                Form1.form1.richTextBox1.AppendText(ss + Environment.NewLine);
                Form1.form1.backgroundWorker1.ReportProgress((int)(j / maxtimes * 100));


            }
        }


        public string HttpGet(string url)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


        public void LogWrite(string str)
        {         //项目根目录
            string path = "ilogs.txt";

            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(fs);
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream fs = new FileStream(path, FileMode.Append);
                //文本写入
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
