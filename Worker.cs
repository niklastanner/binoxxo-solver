using System.Collections.Generic;
using System.Linq;

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
                    FillGaps(tuple);
                    CompleteLine(tuple);
                    PreventIdenticalLines(tuple);
                }
            } while (!Solver.IsSolved());
        }

        #region Check Pairs
        // _XX_ -> OXXO
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

        #region Fill Gaps
        // X_X -> XOX
        private void FillGaps(List<Field> tuple)
        {
            for (int i = 0; i < tuple.Count; i++)
            {
                int? value = tuple[i].value;

                if (value != null && (i + 2) < tuple.Count && tuple[i + 1].value == null && tuple[i + 2].value == value)
                {
                    tuple[i + 1].value = 1 - value;
                }
            }
        }
        #endregion

        #region Complete Line
        // Complete Row only missing one value
        private void CompleteLine(List<Field> tuple)
        {
            int countNull = tuple.Count(e => e.value == null);

            int countO = tuple.Count(e => e.value == 0);
            if (countNull == 1 && (countO * 2) == tuple.Count)
            {
                foreach (Field f in tuple)
                {
                    if (f.value == null) { f.value = 1; }
                }
            }

            int countX = tuple.Count(e => e.value == 1);
            if (countNull == 1 && (countX * 2) == tuple.Count)
            {
                foreach (Field f in tuple)
                {
                    if (f.value == null) { f.value = 0; }
                }
            }
        }
        #endregion

        #region Prevent Identical Lines
        private void PreventIdenticalLines(List<Field> tuple)
        {
            if (tuple.Count(e => e.value == null) != 2) { return; }

            PreventIdenticalLinesIterator(tuple, Solver.GetAllColumns());
            PreventIdenticalLinesIterator(tuple, Solver.GetAllRows());
        }

        private void PreventIdenticalLinesIterator(List<Field> tuple, List<List<Field>> compareTo)
        {
            foreach (List<Field> line in compareTo)
            {
                if (line.Count(e => e.value == null) != 0) { continue; }

                if (Solver.CompareLines(tuple, line) == 2)
                {
                    for (int i = 0; i < tuple.Count; i++)
                    {
                        if (tuple[i].value == null) { tuple[i].value = 1 - line[i].value; }
                    }
                }
            }
        }
        #endregion
    }
}

