﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Console.WriteLine("Trying to solve the following Binoxxo:");

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
                if (!thread.Join(lifespan)) { break; }
                timeout.Stop();
                lifespan -= (int)timeout.ElapsedMilliseconds;
            }
        }

        #region Validation Methods
        public bool IsSolved()
        {
            for (int i = 0; i < binoxxo.size; i++)
            {
                if (binoxxo.Get(i) == null) { return false; }
            }
            return true;
        }

        public bool ValidateBinoxxo()
        {
            if (!IsSolved()) { return false; }

            List<int?[]> rows = binoxxo.GetAllRows();
            List<int?[]> columns = binoxxo.GetAllColumns();

            // Check uniqueness of each row and column
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = i + 1; j < rows.Count; j++)
                {
                    if (rows[i].SequenceEqual(rows[j])) { return false; }
                }

                foreach (int?[] column in columns)
                {
                    if (rows[i].SequenceEqual(column)) { return false; }
                }
            }

            // Check for other rules
            int requiredCount = binoxxo.size / 2;
            foreach (int?[] row in rows)
            {
                // Check for equal distribution of X and O
                int count = row.Count(e => e == 0);
                if (count != requiredCount) { return false; }

                // Check for pairs
                int countO = 0;
                int countX = 0;
                foreach (int l in row)
                {
                    countO = l == 0 ? countO++ : 0;
                    countX = l == 1 ? countX++ : 0;
                    if (countO > 2 || countX > 2) { return false; }
                }
            }
            foreach (int?[] column in columns)
            {
                // Check for equal distribution of X and O
                int count = column.Count(e => e == 0);
                if (count != requiredCount) { return false; }

                // Check for pairs
                int countO = 0;
                int countX = 0;
                foreach (int l in column)
                {
                    countO = l == 0 ? countO++ : 0;
                    countX = l == 1 ? countX++ : 0;
                    if (countO > 2 || countX > 2) { return false; }
                }
            }

            return true;
        }
        #endregion
    }
}
