
using System;
using System.Collections.Generic;

namespace GenetikAlgoritma
{
    public class Birey
    {
        private readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public Birey TurnuvaCifti { get; set; }
        public Kromozom kromozom { get; set; }

     
        public List<Birey> Olustur(int populasyon, int genBoyutu)
        {
            List<Birey> pop = new List<Birey>();
            for (int i = 0; i < populasyon; i++)
            {
                List<double> xValues = new List<double>();
                for (int j = 0; j < genBoyutu; j++)
                {
                    xValues.Add(rnd.NextDouble() * 20 - 10); 
                }
                pop.Add(new Birey(xValues));
            }
            return pop;
        }

        public Birey(List<double> x)
        {
            kromozom = new Kromozom(x);
        }

        public Birey()
        {
        }
        public double SphereFonksiyonSkor()
        {
            return kromozom.FonksiyonSkor;
        }
    }
}
