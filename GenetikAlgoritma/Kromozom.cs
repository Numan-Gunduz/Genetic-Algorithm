
using System;
using System.Collections.Generic;

namespace GenetikAlgoritma
{
    public class Kromozom
    {
        private readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public List<double> X { get; } = new List<double>();

        public Kromozom(int genBoyutu)
        {
            for (int i = 0; i < genBoyutu; i++)
            {
                X.Add(rnd.NextDouble() * 20 - 10); 
            }
        }
        public Kromozom(List<double> x)
        {
            X.AddRange(x);
        }
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
