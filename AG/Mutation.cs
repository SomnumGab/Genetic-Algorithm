using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG
{
    class Mutation
    {
        public List<String> Changes { get; set; }
        public List<String> UniformMutation(List<String> population, double pm)
        {
            var pop = new List<String>();
            Random rand = new Random();
            Changes = new List<string>();
            for (int i = 0; i < population.Count; i++) Changes.Add("-");
  
            for (int i = 0; i < population.Count; i++)
            {
                String temp = population[i];
                for (int y = 0; y < population[i].Length; y++)
                {
                    if (rand.NextDouble() <= pm)
                    {
                        Changes[i] = Changes[i] + ", " + y;
                        if (temp.ElementAt(y) == '1')
                        {
                            StringBuilder sb = new StringBuilder(temp);
                            sb[y] = '0';
                            temp = sb.ToString();
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder(temp);
                            sb[y] = '1';
                            temp = sb.ToString();
                        }
                    }
                }
                pop.Add(temp);
            }
            return pop;
        }
    }
}
