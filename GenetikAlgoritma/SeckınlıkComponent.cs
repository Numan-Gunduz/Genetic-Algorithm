
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace GenetikAlgoritma
{
    public partial class SeckınlıkComponent : UserControl
    {
        public Birey Canli { get; set; }

        public SeckınlıkComponent()
        {
            InitializeComponent();
        }
        public SeckınlıkComponent(Birey c, int no) : this()
        {
            this.Canli = c;
            label5.Text = no.ToString();
            label8.Text = c.kromozom.X[0].ToString();
            label9.Text = c.kromozom.X[1].ToString();
            sonuc3.Text = c.kromozom.X[2].ToString();
            label3.Text = c.SphereFonksiyonSkor().ToString();

        }
    }
}
