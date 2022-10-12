using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG
{
    class Crossover
    {
        public List<int> PCs { get; set; }
        public List<String> FindParents(List<String> binGeneration, double pK, List<double> randoms)
        {
            var parents = new List<String>();

            for (int i = 0; i < binGeneration.Count; i++)
            {
                if (randoms[i] <= pK)
                {
                    parents.Add(binGeneration[i]);
                }
            }
            return parents;        
        }

        public List<String> FindNotParents(List<String> binGeneration, double pK, List<double> randoms)
        {
            var nonparents = new List<String>();

            for (int i = 0; i < binGeneration.Count; i++)
            {
                if (randoms[i] > pK)
                {
                    nonparents.Add(binGeneration[i]);
                }
            }
            return nonparents;
        }
        public void GeneratePC(int ParentCount, int l) 
        {
            Random rand = new Random();
            PCs = new List<int>();
            for (int i = 0; i < Math.Ceiling((decimal)ParentCount/2); i++)
            {
                PCs.Add(rand.Next(0, l - 2));
            }
        }
        public List<String> GenerateChildren(List<String> parents, int l) 
        {
            if (parents.Count == 1) return parents;
            var children = new List<String>();
            var j = 0;

            for (int i = 0; i < parents.Count; i+=2)
            {
                var start1 = ""; var start2 = ""; var end1 = ""; var end2 = "";
                if (i + 1 == parents.Count)
                {
                     start1 = parents[i].Substring(0, PCs[j]);
                     end2 = parents[0].Substring(PCs[j], l - PCs[j]);
                     children.Add(start1 + end2);
                }
                else
                {
                     start1 = parents[i].Substring(0, PCs[j]);
                     start2 = parents[i + 1].Substring(0, PCs[j]);

                     end1 = parents[i].Substring(PCs[j], l - PCs[j]);
                     end2 = parents[i + 1].Substring(PCs[j], l - PCs[j]);

                    children.Add(start1 + end2);
                    children.Add(start2 + end1);
                }

                j++;
            }

            return children;
        }
    }
}
