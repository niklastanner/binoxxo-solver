using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Binoxxo_Solver
{
    class Solver
    {
        private static Binoxxo binoxxo;
        private readonly List<Thread> threads = new List<Thread>();
        private readonly int lifespan = 1000 * 5;

        public Solver(Binoxxo binoxxo)
        {
            Solver.binoxxo = binoxxo;
        }

        public void Solve()
        {
            Console.WriteLine("Trying to solve the following Sudoku:");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Do the solver thing

            WaitForAllThreads(lifespan);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("\nRunTime {0}.{1:D3} Seconds", ts.Seconds, ts.Milliseconds);

            if (ValidateBinoxxo())
            {
                Console.WriteLine("Solved correctly\n");
            }
            else
            {
                Console.WriteLine("I am way too dumb to solve this binoxxo\n");
            }

            binoxxo.PrintBinoxxo();
        }

        private void WaitForAllThreads(int lifespan)
        {
            foreach (Thread thread in threads)
            {
                Stopwatch timeout = Stopwatch.StartNew();
                if (!thread.Join(lifespan))
                {
                    break;
                }
                timeout.Stop();
                lifespan -= (int)timeout.ElapsedMilliseconds;
            }
        }

        #region Validation Methods
        public bool IsSolved()
        {
            for (int i = 0; i < binoxxo.size; i++)
            {
                if (binoxxo.Get(i) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidateBinoxxo()
        {
            return true;
        }
        #endregion
    }
}
