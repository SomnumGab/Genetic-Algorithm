using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG
{
    class Testing
    {
         private int[] Ns = {30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80};
         private int[] Ts = {50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150};
       private double[] pks = {0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9};
         private double[] pms = {0.0001, 0.0005, 0.001, 0.001, 0.005, 0.01}; 

       /*  private int[] Ns = {30, 35};
         private int[] Ts = {50, 60};
         private double[] pks = {0.5, 0.55};
         private double[] pms = {0.0001, 0.0005};
       */
        public List<TestResult> TestTime()
        {
            var result = new List<TestResult>();
            for (int i = 0; i < Ns.Length; i++)
            {
                for (int y = 0; y < Ts.Length; y++)
                {
                    for (int x = 0; x < pks.Length; x++)
                    {
                        for (int z = 0; z < pms.Length; z++)
                        {
                            var max = 0.0;
                            var avg = 0.0;
                            for (int c = 0; c < 100; c++)
                            {
                                Generations gens = new Generations(-4, 12, Ns[i], 0.001, 3, pks[x], pms[z], Ts[y], true, true);
                                List<Results> results = gens.GenerateResults(gens.GenerationLoop(gens.GenerateFirstGen()));
                                if (max < results[0].feval) max = results[0].feval;
                                avg += results[0].feval;
                            }
                            var tr = new TestResult();
                            tr.N = Ns[i]; tr.T = Ts[y]; tr.pk = pks[x]; tr.pm = pms[z];
                            tr.max = max; tr.avg = (avg / 100);
                            result.Add(tr);
                        }
                    }
                }
            }
            return result;
        }

    }
}

public struct TestResult
{
    public int N;
    public int T;
    public double pk;
    public double pm;
    public double avg;
    public double max;
}
