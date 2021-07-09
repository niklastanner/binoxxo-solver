using System;
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
            Console.Clear();
            Console.WriteLine("Trying to solve the following Binoxxo:\n");
            Thread painter = new Thread(new Painter().ContinuousPaintBinoxxo);
            painter.Start(binoxxo);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Worker worker = new Worker();
            threads.Add(new Thread(worker.Run));
            threads.Add(new Thread(worker.Run));
            threads[0].IsBackground = true;
            threads[1].IsBackground = true;
            threads[0].Start(binoxxo.GetAllRows());
            threads[1].Start(binoxxo.GetAllColumns());

            WaitForAllThreads(lifespan);
            painter.Abort();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.Clear();
            Console.WriteLine("\nRunTime {0}.{1:D3} Seconds", ts.Seconds, ts.Milliseconds);

            if (ValidateBinoxxo())
            {
                Console.WriteLine("Solved correctly\n");
            }
            else
            {
                Console.WriteLine("I am way too dumb to solve this binoxxo\n");
            }

            Console.WriteLine(binoxxo.PrintBinoxxo());
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

        #region Support Methods
        public static List<List<Field>> GetAllRows()
        {
            return binoxxo.GetAllRows();
        }

        public static List<List<Field>> GetAllColumns()
        {
            return binoxxo.GetAllColumns();
        }

        public static int CompareLines(List<Field> list1, List<Field> list2)
        {
            return binoxxo.CompareLines(list1, list2);
        }

        public static int CountElement(List<Field> tuple, int? countMe)
        {
            return tuple.Count(e => e.value == countMe);
        }

        public static int TupleContainsPattern(List<Field> tuple, string pattern)
        {
            char[] patternArray = pattern.ToArray();
            int?[] patternIntArray = new int?[patternArray.Length];

            for (int i = 0; i < patternIntArray.Length; i++)
            {
                if (patternArray[i].Equals(' ') || patternArray[i].Equals('_'))
                {
                    patternIntArray[i] = null;
                }
                else if (patternArray[i].Equals('0') || patternArray[i].Equals('o') || patternArray[i].Equals('O'))
                {
                    patternIntArray[i] = 0;
                }
                else if (patternArray[i].Equals('1') || patternArray[i].Equals('x') || patternArray[i].Equals('X'))
                {
                    patternIntArray[i] = 1;
                }
            }

            int arrayPos = 0;
            for (int i = 0; i < tuple.Count; i++)
            {
                if (tuple[i].value == patternIntArray[arrayPos])
                {
                    arrayPos++;
                }
                if (arrayPos == patternIntArray.Length)
                {
                    return i - patternIntArray.Length + 1;
                }
            }

            return -1;
        }

        public static int GetNullMargin(List<Field> tuple, int nullCount)
        {
            int margin = 0;
            int count = 0;
            foreach (Field f in tuple)
            {
                if (f.value == null || margin > 0) { margin++; }
                if (f.value == null) { count++; }
                if (count == nullCount) { break; }
            }

            return margin;
        }

        public static int[] GetIndexesOf(List<Field> tuple, int? value)
        {
            int[] indexes = new int[CountElement(tuple, value)];
            int count = 0;
            for (int i = 0; i < tuple.Count; i++)
            {
                if (tuple[i].value == value)
                {
                    indexes[count++] = i;
                }
            }

            return indexes;
        }
        #endregion

        #region Validation Methods
        public static bool IsSolved()
        {
            for (int i = 0; i < Math.Pow(binoxxo.size, 2); i++)
            {
                if (binoxxo.Get(i).value == null) { return false; }
            }
            return true;
        }

        public static bool ValidateBinoxxo()
        {
            if (!IsSolved()) { return false; }

            List<List<Field>> rows = binoxxo.GetAllRows();
            List<List<Field>> columns = binoxxo.GetAllColumns();

            // Check uniqueness of each row and column
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = i + 1; j < rows.Count; j++)
                {
                    if (rows[i].SequenceEqual(rows[j])) { return false; }
                }

                foreach (List<Field> column in columns)
                {
                    if (rows[i].SequenceEqual(column)) { return false; }
                }
            }

            // Check for other rules
            int requiredCount = binoxxo.size / 2;
            foreach (List<Field> row in rows)
            {
                // Check for equal distribution of X and O
                int count = CountElement(row, 0);
                if (count != requiredCount) { return false; }

                // Check for pairs
                int countO = 0;
                int countX = 0;
                foreach (Field l in row)
                {
                    countO = l.value == 0 ? ++countO : 0;
                    countX = l.value == 1 ? ++countX : 0;
                    if (countO > 2 || countX > 2) { return false; }
                }
            }
            foreach (List<Field> column in columns)
            {
                // Check for equal distribution of X and O
                int count = CountElement(column, 0);
                if (count != requiredCount) { return false; }

                // Check for pairs
                int countO = 0;
                int countX = 0;
                foreach (Field l in column)
                {
                    countO = l.value == 0 ? ++countO : 0;
                    countX = l.value == 1 ? ++countX : 0;
                    if (countO > 2 || countX > 2) { return false; }
                }
            }

            return true;
        }
        #endregion
    }
}
