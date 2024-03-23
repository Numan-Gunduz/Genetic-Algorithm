
using System;
using System.Collections.Generic;

namespace GenetikAlgoritma
{
    public class Gen
    {
        private readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public List<double> X { get; } = new List<double>();

        // Gen sınıfı oluşturulurken X değerlerini rastgele belirle
        public Gen(int genBoyutu)
        {
            for (int i = 0; i < genBoyutu; i++)
            {
                X.Add(rnd.NextDouble() * 20 - 10); // [-10, 10] aralığında rastgele bir değer
            }
        }

        // Özel olarak X değerlerini atanmış Gen sınıfı oluştur
        public Gen(List<double> x)
        {
            X.AddRange(x);
        }

        // Matyas fonksiyonunun skorunu hesapla
        public double FonksiyonSkor
        {
            get
            {
                double sum = 0;
                foreach (var xi in X)
                {
                    sum += xi * xi;
                }
                return sum;
            }
        }
    }
}
