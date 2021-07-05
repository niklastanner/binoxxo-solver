using System.Collections.Generic;

namespace Binoxxo_Solver
{
    class Worker
    {
        public void Run(object param)
        {
            List<List<Field>> tuples = (List<List<Field>>)param;

            do
            {
                foreach (List<Field> tuple in tuples)
                {
                    CheckPairs(tuple);
                }
            } while (!Solver.IsSolved());
        }

        #region Check Pairs
        private void CheckPairs(List<Field> tuple)
        {
            int countO = 0;
            int countX = 0;
            for (int i = 0; i < tuple.Count; i++)
            {
                countO = tuple[i].value == 0 ? ++countO : 0;
                countX = tuple[i].value == 1 ? ++countX : 0;
                if (countO == 2)
                {
                    if ((i - 2) >= 0 && tuple[i - 2].value == null)
                    {
                        tuple[i - 2].value = 1;
                    }
                    if ((i + 1) < tuple.Count && tuple[i + 1].value == null)
                    {
                        tuple[i + 1].value = 1;
                    }
                }
                if (countX == 2)
                {
                    if ((i - 2) >= 0 && tuple[i - 2].value == null)
                    {
                        tuple[i - 2].value = 0;
                    }
                    if ((i + 1) < tuple.Count && tuple[i + 1].value == null)
                    {
                        tuple[i + 1].value = 0;
                    }
                }
            }
        }
        #endregion
    }
}

