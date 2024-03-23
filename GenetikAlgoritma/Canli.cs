
using System;
using System.Collections.Generic;

namespace GenetikAlgoritma
{
    public class Canli
    {
        private readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Canli TurnuvaCifti { get; set; }
        public Gen Gen { get; set; }

        // Canli sınıfını oluştururken Gen sınıfını Matyas fonksiyonuna uygun olarak oluştur
        public List<Canli> Olustur(int populasyon, int genBoyutu)
        {
            List<Canli> pop = new List<Canli>();
            for (int i = 0; i < populasyon; i++)
            {
                List<double> xValues = new List<double>();
                for (int j = 0; j < genBoyutu; j++)
                {
                    xValues.Add(rnd.NextDouble() * 20 - 10); // [-10, 10] aralığında rastgele bir değer
                }
                pop.Add(new Canli(xValues));
            }
            return pop;
        }

        // Canli sınıfını oluştururken Gen sınıfını belirtilen X değerleriyle oluştur
        public Canli(List<double> x)
        {
            Gen = new Gen(x);
        }

        public Canli()
        {
        }
    }
}
