using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG
{
    class Selection
    {
        public double FunDopasowania(double feval, List<double> fevals, double acurracy)
        {
            Conversions conv = new Conversions();
            var min = fevals.Min();
            return feval - min + acurracy;

        }
        public double GetProbability(List<double> dopasowania, double dopasowanie)
        {
            var sum = dopasowania.Sum();
            return dopasowanie / sum;
        }
        public double GetQ(List<double> probabilites, int index) 
        {
            var sum = 0.0;
            for (int i = 0; i <= index; i++)
            {
                sum += probabilites[i];
            }
            return sum;
        }
        public List<double> GetWinners(List<double> randoms, List<double> Qs, List<double> xreals)
        {
            var winners = new List<double>();
            for (int i = 0; i < randoms.Count; i++)
            {
                for (int j = 0; j < Qs.Count; j++)
                {
                    if (randoms[i] <= Qs[j])
                    {
                        winners.Add(xreals[j]);
                        break;
                    }
                }
            }
            return winners;
        }
        public double FindElite(List<double> fevals, List<double> xreals)
        {
            var best = fevals[0];
            var index = 0;
            for (int i = 0; i < fevals.Count; i++)
            {
                if (fevals[i] > best)
                {
                    best = fevals[i];
                    index = i;
              }
            }
            return xreals[index];
        }
        public List<double> RestoreElite(double elite, List<double> xreals)
        {
            Random rand = new Random();
            var temp = rand.Next(xreals.Count);
            List<double> xd = new List<double>();
            for (int i = 0; i < xreals.Count; i++)
            {
                if (i == temp) xd.Add(elite);
                else xd.Add(xreals[i]);
            }
            return xd;
        }
    }
}
