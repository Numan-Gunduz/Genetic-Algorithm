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
        private bool ToggleKontrol()
        {
            if (isRunning)
            {
                isRunning = false;
                button1.Text = "Başlat";
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
            label11.Text = "Toplam:";
            pictureBox1.Image = Properties.Resources.Sheper;
            this.chart1.Series.Clear();
            Series series = this.chart1.Series.Add("Sonuclar");
            chart1.IsSoftShadows = false;

            series.ChartType = SeriesChartType.Area;
            series.BorderWidth = 3;
            series.Color = Color.Teal;
            return series;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (!ToggleKontrol()) return;

            System.Windows.Forms.DataVisualization.Charting.Series series = GenSeries();

            int popSayi = (int)popülasyonBoyut.Value;
            int seckınlıkPop = (int)seckınlık.Value;
            int iterasyon = (int)numericUpDown4.Value;
            double carazlamaOran = (double)caprazlama.Value / 100;
            double mutasyonOran = (double)mutasyonoranı.Value / 100;
            int ms = 5; 

            GenetikIslemler GenDrv = new GenetikIslemler(popSayi);
            GenDrv.seckınlıkPop = seckınlıkPop;

            Image img = Properties.Resources.Sheper;
            for (int j = 0; j < iterasyon; j++)
            {
                await Task.Delay(ms);
                GenDrv.Seckınlık();
                GenDrv.TurnuvaCiftiOlustur();
                GenDrv.Caprazla(carazlamaOran);
                GenDrv.Mutasyon(mutasyonOran);

                SeckınlıkFlowLayoutEkle(GenDrv.BestCanli());
                pictureBox1.Image = img;

                var eniyiSkor = GenDrv.BestCanli().SphereFonksiyonSkor() * 10000;
                series.Points.AddXY(j, eniyiSkor);
                string xiListString = string.Join(", ", GenDrv.BestCanli().kromozom.X[0]);
                string xjListString = string.Join(", ", GenDrv.BestCanli().kromozom.X[1]);
                string njListString = string.Join(", ", GenDrv.BestCanli().kromozom.X[2]);
                sonuc1.Text = xiListString;
                sonuc2.Text = xjListString;
                sonuc3.Text = njListString;
                var eniyiSkorr = GenDrv.BestCanli().SphereFonksiyonSkor();
                label7.Text = "En iyi Fonksiyon: " + eniyiSkorr.ToString();

                if (!isRunning) break;
                if (j == iterasyon - 1) ToggleKontrol();
            }
        }
        public bool SeckınlıkFlowLayoutEkle(Birey c)
        {

            foreach (var Seckınlık in flowLayoutPanel1.Controls.OfType<SeckınlıkComponent>())
                if (c.kromozom.FonksiyonSkor == Seckınlık.Canli.SphereFonksiyonSkor())
                    return false;
            label11.Text="Toplam:"+(flowLayoutPanel1.Controls.Count+1);
            var comp = new SeckınlıkComponent(c, flowLayoutPanel1.Controls.Count + 1);
            flowLayoutPanel1.Controls.Add(comp);
            return true;
        }

    }

}

