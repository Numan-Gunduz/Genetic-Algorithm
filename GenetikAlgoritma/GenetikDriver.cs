
using System;
using System.Collections.Generic;
using System.Linq;

namespace GenetikAlgoritma
{
    class GenetikDriver
    {
        public List<Canli> canliList { get; set; }
        public List<Canli> elitList { get; set; }
        public int elitPop { get; set; }

        public List<Canli> populasyonList
        {
            get
            {
                List<Canli> list = new List<Canli>();
                list.AddRange(canliList);
                if (elitList != null)
                    list.AddRange(elitList);
                return list;
            }
        }

        public GenetikDriver(int pop)
        {
            PopulasyonOlustur(pop);
        }

        private Canli Kiyasla(Canli c1, Canli c2)
        {
            return c1.Gen.FonksiyonSkor > c2.Gen.FonksiyonSkor ? c1 : c2;
        }

        public List<Canli> PopulasyonOlustur(int pop)
        {
            List<Canli> liste = new Canli().Olustur(pop, 2); // 2, Gen boyutu olarak kabul edildi
            canliList = liste;
            return liste;
        }

        public List<Canli> TurnuvaCiftiOlustur()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Canli> TurnuvaList = new List<Canli>();
            for (int i = 0; i < canliList.Count; i++)
            {
                int rndIndis1, rndIndis2;
                rndIndis1 = rnd.Next(0, canliList.Count);
                rndIndis2 = rnd.Next(0, canliList.Count);
                var v1 = canliList[rndIndis1];
                var v2 = canliList[rndIndis2];
                TurnuvaList.Add(Kiyasla(v1, v2));

                rndIndis1 = rnd.Next(0, canliList.Count);
                rndIndis2 = rnd.Next(0, canliList.Count);
                v1 = canliList[rndIndis1];
                v2 = canliList[rndIndis2];
                TurnuvaList[i].TurnuvaCifti = Kiyasla(v1, v2);
            }
            canliList = TurnuvaList;
            return TurnuvaList;
        }


        public List<Canli> Caprazla(double ihtimal)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Canli> caprazlanmisList = new List<Canli>();

            foreach (var canli in canliList)
            {
                if (rnd.NextDouble() > ihtimal)
                {
                    caprazlanmisList.Add(canli);
                    continue;
                }

                List<double> offspring1 = new List<double>();
                List<double> offspring2 = new List<double>();

                for (int i = 0; i < canli.Gen.X.Count; i++)
                {
                    double rndDouble = rnd.NextDouble();
                    double off1 = rndDouble * canli.Gen.X[i] + (1 - rndDouble) * canli.TurnuvaCifti.Gen.X[i];
                    double off2 = (1 - rndDouble) * canli.Gen.X[i] + rndDouble * canli.TurnuvaCifti.Gen.X[i];
                    offspring1.Add(off1);
                    offspring2.Add(off2);
                }

                caprazlanmisList.Add(new Canli(offspring1));
                caprazlanmisList.Add(new Canli(offspring2));
            }
            canliList = caprazlanmisList;
            return caprazlanmisList;
        }

        public List<Canli> Mutasyon(double ihtimal)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<Canli> mutasyonList = new List<Canli>();

            foreach (var canli in canliList)
            {
                if (rnd.NextDouble() > ihtimal)
                {
                    mutasyonList.Add(canli);
                    continue;
                }

                List<double> mutatedGen = new List<double>();
                foreach (var xi in canli.Gen.X)
                {
                    double mutation = xi + (rnd.NextDouble() * 2 - 1); // [-1, 1] aralığında rastgele bir değer ekle
                    mutatedGen.Add(mutation);
                }

                mutasyonList.Add(new Canli(mutatedGen));
            }

            canliList = mutasyonList;
            return mutasyonList;
        }

        public Canli BestCanli()
        {
            var c = populasyonList.OrderBy(a => a.Gen.FonksiyonSkor).FirstOrDefault();
            Console.WriteLine("En iyi Canlı:" + c.Gen.FonksiyonSkor);
            return c;
        }

        public List<Canli> Elitizm(int elitPop)
        {
            List<Canli> elitizm = populasyonList.OrderBy(a => a.Gen.FonksiyonSkor).Take(elitPop).ToList();
            canliList = populasyonList.OrderBy(a => a.Gen.FonksiyonSkor).Reverse().Take(populasyonList.Count() - elitPop).ToList();
            elitList = elitizm;
            Console.WriteLine("En iyi Fonsiyon:" + populasyonList.OrderBy(a => a.Gen.FonksiyonSkor).FirstOrDefault().Gen.FonksiyonSkor);
            return elitizm;
        }

        public List<Canli> Elitizm()
        {
            return Elitizm(elitPop);
        }
    }
}

