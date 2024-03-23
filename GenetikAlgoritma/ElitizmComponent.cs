
using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace GenetikAlgoritma
{
    public partial class ElitizmComponent : UserControl
    {
        public Canli Canli { get; set; }

        public ElitizmComponent()
        {
            InitializeComponent();
        }

        public ElitizmComponent(Canli c, int no) : this()
        {
            this.Canli = c;
            label5.Text = no.ToString();
            label8.Text = c.Gen.X[0].ToString(); // X değerleri listesi olarak tutulduğu için indeksle erişmek gerekiyor
            label9.Text = c.Gen.X[1].ToString(); // X değerleri listesi olarak tutulduğu için indeksle erişmek gerekiyor
            label3.Text = c.Gen.FonksiyonSkor.ToString(); // Matyas fonksiyonu için Gen sınıfında tanımlanan FonksiyonSkor özelliği kullanıldı
            Renklendir();
        }

        private void Renklendir()
        {
            double d = Canli.Gen.FonksiyonSkor; // Matyas fonksiyonu için Gen sınıfında tanımlanan FonksiyonSkor özelliği kullanıldı
            int ratio = (int)(d * 1700), r = 100, g = 0;
            if (ratio <= 100)
            {
                r = (int)(255 * ((double)ratio / (double)100));
                g = (int)(255 * ((double)(100 - ratio) / (double)101));
            }

            pictureBox2.BackColor = Color.FromArgb(r, g, 0);
        }

        private void ElitizmComponent_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
