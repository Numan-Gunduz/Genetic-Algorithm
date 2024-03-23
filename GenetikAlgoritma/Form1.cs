using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GenetikAlgoritma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isRunning = false;

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void TabloRender(List<Canli> c, int cap = 10, Color? color = null, Image img = null)
        {
            bool check = img == null;

            if (check)
                img = Properties.Resources.matyas;

            foreach (Canli canli in c)
            {
                for (int i = 0; i < canli.Gen.X.Count; i++)
                {
                    double xi = canli.Gen.X[i];
                    int x = (int)((double)((xi + 10) / 20) * (img.Width - 50));
                    int y = (int)((double)((xi + 10) / 20) * (img.Height - 60));
                    drawPoint(x + 25, y + 30, img, cap, color);
                };
            }
            if (check)
                pictureBox1.Image = img;

        }

        public void drawPoint(int x, int y, Image img, int radius = 10, Color? color = null)
        {
            Graphics g = Graphics.FromImage(img);
            int alpha = (int)numericUpDown7.Value;
            Color colorTemp = Color.Empty;

            Point dPoint = new Point(x, (img.Height - y));
            dPoint.X = dPoint.X - 2;
            dPoint.Y = dPoint.Y - 2;
            if (color.HasValue)
            {
                colorTemp = Color.FromArgb(alpha, color.Value.R, color.Value.G, color.Value.B);
                g.FillCircle(new SolidBrush(Color.FromArgb(alpha, color.Value.R, color.Value.G, color.Value.B)), dPoint.X, dPoint.Y, 2);
                g.DrawCircle(new Pen(colorTemp), dPoint.X, dPoint.Y, radius); //çember
            }
            else
            {
                g.FillCircle(new SolidBrush(Color.FromArgb(255, 0, 50, 255)), dPoint.X, dPoint.Y, 2);
                g.DrawCircle(new Pen(Color.DarkBlue), dPoint.X, dPoint.Y, radius); //çember
            }

            g.Dispose();
        }

        private bool ToggleKontrol()
        {
            if (isRunning)
            {
                isRunning = false;
                button1.Text = "HESAPLA";
            }
            else
            {
                button1.Text = "Durdur";
                isRunning = true;
            }

            return isRunning;
        }

        private Series GenSeries()
        {

            flowLayoutPanel1.Controls.Clear();
            label11.Text = "Toplam:0";
            pictureBox1.Image = Properties.Resources.matyas;
            this.chart1.Series.Clear();
            Series series = this.chart1.Series.Add("Sonuclar");
            chart1.IsSoftShadows = false;

            series.ChartType = SeriesChartType.Area;
            series.BorderWidth = 3;
            series.Color = Color.IndianRed;
            return series;
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            if (!ToggleKontrol()) return;

            System.Windows.Forms.DataVisualization.Charting.Series series = GenSeries();

            int popSayi = (int)numericUpDown1.Value;
            int elitPop = (int)numericUpDown5.Value;
            int iterasyon = (int)numericUpDown4.Value;
            double carazlamaOran = (double)numericUpDown2.Value / 100;
            double mutasyonOran = (double)numericUpDown3.Value / 100;
            int ms = (int)numericUpDown6.Value;

            GenetikDriver GenDrv = new GenetikDriver(popSayi);
            GenDrv.elitPop = elitPop;

            Image img = Properties.Resources.matyas;
            for (int j = 0; j < iterasyon; j++)
            {
                await Task.Delay(ms);
                GenDrv.Elitizm();
                GenDrv.TurnuvaCiftiOlustur();
                GenDrv.Caprazla(carazlamaOran);
                GenDrv.Mutasyon(mutasyonOran);

                ElitizmFlowLayoutEkle(GenDrv.BestCanli());

                TabloRender(GenDrv.canliList, 10, colorDialog1.Color, img);
                TabloRender(GenDrv.elitList, 10, colorDialog2.Color, img);
                pictureBox1.Image = img;

                var eniyiSkor = GenDrv.BestCanli().Gen.FonksiyonSkor * 10000;
                series.Points.AddXY(j, eniyiSkor);
                string xiListString = string.Join(", ", GenDrv.BestCanli().Gen.X);
                label8.Text = xiListString;

                var eniyiSkorr = GenDrv.BestCanli().Gen.FonksiyonSkor;
                label7.Text = "Fonksiyon Skoru: " + eniyiSkorr.ToString();

                if (!isRunning) break;
                if (j == iterasyon - 1) ToggleKontrol();
            }
        }


        public bool ElitizmFlowLayoutEkle(Canli c)
        {
            foreach (var elitizm in flowLayoutPanel1.Controls.OfType<ElitizmComponent>())
                if (c.Gen.FonksiyonSkor == elitizm.Canli.Gen.FonksiyonSkor)
                    return false;

            label11.Text = "Toplam:" + (flowLayoutPanel1.Controls.Count + 1);
            var comp = new ElitizmComponent(c, flowLayoutPanel1.Controls.Count + 1);

            comp.pictureBox2.Click += (s, arg) =>
            {
            var canli = ((s as Control).Parent.Parent.Parent as ElitizmComponent).Canli;
            var list = new List<Canli>();
            list.Add(canli);
                TabloRender(list, 20);
            };
            flowLayoutPanel1.Controls.Add(comp);
            return true;
        }


        private void buttonClr1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btn.BackColor = colorDialog1.Color;
            }
        }

        private void buttonClr2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                btn.BackColor = colorDialog2.Color;
            }

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
    }

    public static class GraphicsExtensions
    {
        public static void DrawCircle(this Graphics g, Pen pen,
            float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush,
            float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush, centerX - radius, centerY - radius,
                radius + radius, radius + radius);
        }

    }

}

