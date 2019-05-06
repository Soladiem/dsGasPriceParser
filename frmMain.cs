using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace dsGasPriceParser
{
    public partial class frmMain : Form
    {
        // Файл для хранения данных
        private static string filename = AppDomain.CurrentDomain.BaseDirectory + @"\dsGasPriceParser";

        // Ссылка для парсинга данных
        private static string sourceUrl = "https://www.petrolplus.ru/fuelindex/tatarstan_republic/";

        public frmMain()
        {
            InitializeComponent();
        }

        private static void getData(string url)
        {
            List<string> list = new List<string>();

            string html = getRequest(url);

            string pattern = @"(('АИ-\d{2}')|('Дизельное топливо'))+" +
                @"(,data:\s)+" +
                @"\[(.*)\]+";

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(html);

            foreach (Match math in matches)
            {
                string Prices = math.Groups[5].ToString();
                string[] Price = Prices.Split(',');

                double newPrice = double.Parse(Price[6], CultureInfo.InvariantCulture);

                list.Add(String.Format("{0:0.00}", newPrice));
            }

            try
            {
                // Запись в файл
                File.WriteAllLines(filename, list);
            }
            catch
            {
                MessageBox.Show("Неудалось записать файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string getRequest(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = false;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(response.CharacterSet)))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                DateTime creationTime = File.GetLastWriteTime(filename);
                if (DateTime.Now.ToShortDateString() != creationTime.ToShortDateString())
                {
                    getData(sourceUrl);
                }
            }
            else
            {
                getData(sourceUrl);
            }

            // Считывание файла
            string[] lines = File.ReadAllLines(filename);

            lbl95.Text += ": " + lines[0] + "р.";
            lbl92.Text += ": " + lines[1] + "р.";
            lblDt.Text += ": " + lines[2] + "р.";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(
                e.Graphics, e.ClipRectangle, 
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDark), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDarkDark), 1, ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(
                e.Graphics, e.ClipRectangle,
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDark), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDarkDark), 1, ButtonBorderStyle.Solid);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(
                e.Graphics, e.ClipRectangle,
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlLightLight), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDark), 1, ButtonBorderStyle.Solid,
                Color.FromKnownColor(KnownColor.ControlDarkDark), 1, ButtonBorderStyle.Solid);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sitkodenis.ru");
        }
    }
}
