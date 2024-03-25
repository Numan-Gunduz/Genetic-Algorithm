
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenetikAlgoritma
{
    class GenetikIslemler
    {
        public List<Birey> bıreyList { get; set; }
        public List<Birey> seckınlıkList { get; set; }
        public int seckınlıkPop { get; set; }

        public List<Birey> populasyonList
        {
            get
            {
                List<Birey> list = new List<Birey>();
                list.AddRange(bıreyList);
                if (seckınlıkList != null)
                    list.AddRange(seckınlıkList);
                return list;
            }
        }

        public GenetikIslemler(int pop)
        {
            PopulasyonOlustur(pop);
        }

        private Birey Kiyasla(Birey c1, Birey c2)
        {
            Birey c = new Birey();
            c = c1.SphereFonksiyonSkor() > c2.SphereFonksiyonSkor() ? c1 : c2;
            return c;
        }

        public List<Birey> PopulasyonOlustur(int pop)
        {
            List<Birey> liste = new Birey().Olustur(pop, 3);
            bıreyList = liste;
            return liste;
        }

        public List<Birey> TurnuvaCiftiOlustur()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Birey> TurnuvaList = new List<Birey>();
            for (int i = 0; i < bıreyList.Count; i++)
            {
                int rndIndis1, rndIndis2;
                rndIndis1 = rnd.Next(0, bıreyList.Count);
                rndIndis2 = rnd.Next(0, bıreyList.Count);
                var v1 = bıreyList[rndIndis1];
                var v2 = bıreyList[rndIndis2];
                TurnuvaList.Add(Kiyasla(v1, v2));

                rndIndis1 = rnd.Next(0, bıreyList.Count);
                rndIndis2 = rnd.Next(0, bıreyList.Count);
                v1 = bıreyList[rndIndis1];
                v2 = bıreyList[rndIndis2];
                TurnuvaList[i].TurnuvaCifti = Kiyasla(v1, v2);
            }
            bıreyList = TurnuvaList;
            return TurnuvaList;
        }


        public List<Birey> Caprazla(double ihtimal)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Birey> caprazlanmisList = new List<Birey>();

            foreach (var canli in bıreyList)
            {
                if (rnd.NextDouble() > ihtimal)
                {
                    caprazlanmisList.Add(canli);
                    continue;
                }

                List<double> offspring1 = new List<double>();
                List<double> offspring2 = new List<double>();

                for (int i = 0; i < canli.kromozom.X.Count; i++)
                {
                    double rndDouble = rnd.NextDouble();
                    double off1 = rndDouble * canli.kromozom.X[i] + (1 - rndDouble) * canli.TurnuvaCifti.kromozom.X[i];
                    double off2 = (1 - rndDouble) * canli.kromozom.X[i] + rndDouble * canli.TurnuvaCifti.kromozom.X[i];
                    offspring1.Add(off1);
                    offspring2.Add(off2);
                }

                caprazlanmisList.Add(new Birey(offspring1));
                caprazlanmisList.Add(new Birey(offspring2));
            }
            bıreyList = caprazlanmisList;
            return caprazlanmisList;
        }

        public List<Birey> Mutasyon(double ihtimal)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Birey> mutasyonList = new List<Birey>();

            foreach (var canli in bıreyList)
            {
                if (rnd.NextDouble() > ihtimal)
                {
                    mutasyonList.Add(canli);
                    continue;
                }
                List<double> mutatedGen = new List<double>();
                foreach (var xi in canli.kromozom.X)
                {
                    double mutation = xi + (rnd.NextDouble() * 2 - 1);
                    mutatedGen.Add(mutation);
                }
                mutasyonList.Add(new Birey(mutatedGen));
            }
            bıreyList = mutasyonList;
            return mutasyonList;
        }

        public Birey BestCanli()
        {
            var c = populasyonList.OrderBy(a => a.SphereFonksiyonSkor()).FirstOrDefault();
            Console.WriteLine("En iyi Sonuç Veren :" + c.SphereFonksiyonSkor());
            return c;
        }
        public List<Birey> Seckınlık(int seckınlıkPop)
        {
            List<Birey> Seckınlık = populasyonList.OrderBy(a => a.SphereFonksiyonSkor()).Take(seckınlıkPop).ToList();
            bıreyList = populasyonList.OrderBy(a => a.SphereFonksiyonSkor()).Reverse().Take(populasyonList.Count() - seckınlıkPop).ToList();
            seckınlıkList = Seckınlık;
            Console.WriteLine("En iyi Fonsiyon:" + populasyonList.OrderBy(a => a.SphereFonksiyonSkor()).FirstOrDefault().SphereFonksiyonSkor());
            return Seckınlık;
        }
        public List<Birey> Seckınlık()
        {
            return Seckınlık(seckınlıkPop);
        }
    }
}
