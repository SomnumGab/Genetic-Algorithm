using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG
{
    class Generations
    {
        public double a { get; set; }
        public double b { get; set; }
        public int N { get; set; }
        public double accuracy { get; set; }
        public int decimals { get; set; }
        public double pk { get; set; }
        public double pm { get; set; }
        public int T { get; set; }
        public Boolean elite { get; set; }
        public Boolean testMode { get; set; }

        Conversions convs;
        Selection sele;
        Crossover cross;
        Mutation mut;
        Random rand;

        public List<GraphData> dataGraph = new List<GraphData>();
        public List<double> test = new List<double>();
        String populations;
        public Generations(double a, double b, int N, double accuracy, int decimals, double pk, double pm, int T, Boolean elite, Boolean testMode)
        {
            this.a = a;
            this.b = b;
            this.N = N;
            this.accuracy = accuracy;
            this.decimals = decimals;
            this.pk = pk;
            this.pm = pm;
            this.T = T;
            this.elite = elite;
            this.testMode = testMode;

            convs = new Conversions();
            sele = new Selection();
            cross = new Crossover();
            mut = new Mutation();
            rand = new Random();

        }

        public List<double> GenerateFirstGen()
        {
            List<Double> xreals = new List<double>();
            for (int i = 0; i < N; i++)
            {
                xreals.Add(Math.Round(rand.NextDouble() * (b - a) + a, decimals));
            }
            return xreals;
        }

        public List<double> GenerationLoop(List<double> xreals)
        {
            populations = "Parametry:\na: " + a + " b:" + b + " N:" + N + " pk:" + pk + " pm:" + pm + " dokł:" + accuracy + " Elite:" + elite.ToString() + "\n";
            dataGraph = new List<GraphData>();
            var generation = xreals;
            for (int i = 0; i < T; i++)
            {
                generation = GenerationStep(generation);
                if (!testMode)
                {
                    populations = populations + "Populacja: " + i + "\n";
                    populations = populations + "| Nr | Xreal | feval |\n";
                    for (int y = 0; y < generation.Count; y++)
                    {
                    populations = populations + "| "+y+" | "+generation[y]+" | "+convs.Feval(generation[y]) +" |\n";
                    }
                
                }
            }
            if (!testMode) File.WriteAllText("./Plik1.txt", populations);
            return generation;
        }

        public List<Results> GenerateResults(List<double> xreals)
        {
            List<String> xbin = new List<String>();
            for (int i = 0; i < N; i++)
            {
                xbin.Add(convs.ToBin(a, b, convs.ToInt(a, b, xreals[i], accuracy), accuracy));
            }

            var results = new List<Results>();
            var temp = xbin.Distinct().ToList<String>();
            for (int i = 0; i < temp.Count; i++)
            {
                var result = new Results();
                result.xreal = convs.ToReal(a, b, convs.ToIntFromBin(temp[i]), accuracy, decimals);
                result.feval = convs.Feval(result.xreal);
                var count = xbin.Where(a => a.Equals(temp[i])).Count();
                result.percent = (double)((double)count / (double)xbin.Count)*100;
                result.xbin = temp[i];
                results.Add(result);
            }
            results = results.OrderByDescending(a => a.feval).ToList();
            CreatePopulationFile();
            return results;
        }

        public List<double> GenerationStep(List<Double> xreals)
        {
            List<Double> fevals = new List<double>();
            for (int i = 0; i < N; i++)
            {
                fevals.Add(convs.Feval(xreals[i]));
            }
            var eliter = sele.FindElite(fevals, xreals);

            List<Double> dopasowania = new List<double>();
            for (int i = 0; i < N; i++)
            {
                dopasowania.Add(sele.FunDopasowania(fevals[i], fevals, accuracy));
            }

            List<Double> probabs = new List<double>();
            for (int i = 0; i < N; i++)
            {
                probabs.Add(sele.GetProbability(dopasowania, dopasowania[i]));
            }

            List<Double> Qs = new List<double>();
            for (int i = 0; i < N; i++)
            {
                Qs.Add(sele.GetQ(probabs, i));
            }

            List<Double> randoms = new List<double>();
            for (int i = 0; i < N; i++)
            {
                randoms.Add(rand.NextDouble());
            }

            List<double> winners = sele.GetWinners(randoms, Qs, xreals);

            List<String> xbin = new List<String>();
            for (int i = 0; i < N; i++)
            {
                xbin.Add(convs.ToBin(a, b, convs.ToInt(a, b, winners[i], accuracy), accuracy));
            }

            var randomsForCross = new List<double>();
            for (int i = 0; i < N; i++) randomsForCross.Add(rand.NextDouble());
            var notParents = cross.FindNotParents(xbin, pk, randomsForCross);
            var parents = cross.FindParents(xbin, pk, randomsForCross);

            cross.GeneratePC(parents.Count, parents[0].Length);
            var children = cross.GenerateChildren(parents, parents[0].Length);

            var newPop = new List<String>();
            for (int i = 0; i < notParents.Count; i++) newPop.Add(notParents[i]);
            for (int i = 0; i < children.Count; i++) newPop.Add(children[i]);

            var popAfterMut = mut.UniformMutation(newPop, pm);

            var afterMutXreal = new List<Double>();
            for (int i = 0; i < N; i++)
            {
                afterMutXreal.Add(convs.ToReal(a, b, convs.ToIntFromBin(popAfterMut[i]), accuracy, decimals));
            }

            var afterMutfeval = new List<Double>();
            for (int i = 0; i < N; i++)
            {
                afterMutfeval.Add(convs.Feval(afterMutXreal[i]));
            }
            if (elite)
            {
                var newelite = sele.FindElite(afterMutfeval, afterMutXreal);
                if (convs.Feval(eliter) > convs.Feval(newelite))
                {
                    afterMutXreal = sele.RestoreElite(eliter, afterMutXreal);
                }
            }

            afterMutfeval = new List<Double>();
            for (int i = 0; i < N; i++)
            {
                afterMutfeval.Add(convs.Feval(afterMutXreal[i]));
            }
            if(!testMode) GenerateGraphStats(afterMutfeval);
            return afterMutXreal;
        }
        public void GenerateGraphStats(List<double> fevals)
        {
            var data = new GraphData();
            data.Avg = fevals.Average();
            data.fmax = fevals.Max();
            data.fmin = fevals.Min();
            dataGraph.Add(data);
            test.Add(fevals.Max());
        }
        public  void  CreatePopulationFile()
        {
            String toFile = "Parametry:\na: " + a + " b:" + b + " N:" + N + " pk:" + pk + " pm:" + pm + " dokł:" + accuracy + " Elite:" + elite.ToString()+"\n";
            toFile = toFile + "| Nr.pop | Fmin | Favg | Fmax |\n";
            for (int i = 0; i < dataGraph.Count; i++)
            {
                toFile = toFile + "| " + i + " | " + dataGraph[i].fmin + " | " + dataGraph[i].Avg + " | " + dataGraph[i].fmax + " |\n ";
            }
            File.WriteAllText("./Plik2.txt", toFile);
        }
    }
   public struct Results
    {
        public double xreal;
        public double feval;
        public double percent;
        public String xbin;
    }
    public struct GraphData
    {
        public double Avg;
        public double fmax;
        public double fmin;
    }
}
