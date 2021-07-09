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
                    FillGaps(tuple);
                    PreventTriplets(tuple);
                    EqualizeXAndO(tuple);
                    CompleteLine(tuple);
                    PreventIdenticalLines(tuple);
                    FarNeighbors(tuple);
                    FarSiblings(tuple);
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

        #region Prevent Triplets
        // _X__OX -> OX__OX
        private void PreventTriplets(List<Field> tuple)
        {
            if (Solver.CountElement(tuple, null) != 3) { return; }

            int value = Solver.CountElement(tuple, 0) > Solver.CountElement(tuple, 1) ? 1 : 0;
            int[] indexes = Solver.GetIndexesOf(tuple, null);

            int doubleNullIndex;
            int singleNullIndex;
            if ((indexes[0] + 1) == indexes[1])
            {
                doubleNullIndex = indexes[0];
                singleNullIndex = indexes[2];
            }
            else
            {
                doubleNullIndex = indexes[1];
                singleNullIndex = indexes[0];
            }

            if ((doubleNullIndex - 1) >= 0 && tuple[doubleNullIndex - 1].value == value)
            {
                tuple[singleNullIndex].value = value;
            }
            else if ((doubleNullIndex + 2) < tuple.Count && tuple[doubleNullIndex + 2].value == value){
                tuple[singleNullIndex].value = value;
            }
        }
        #endregion

        #region Equalize X and O
        // _X__XX -> OXOOXX
        private void EqualizeXAndO(List<Field> tuple)
        {
            if (Solver.CountElement(tuple, null) > 0)
            {
                if ((Solver.CountElement(tuple, 0) * 2) == tuple.Count)
                {
                    foreach (Field f in tuple)
                    {
                        if (f.value == null) { f.value = 1; }
                    }
                }
                else if ((Solver.CountElement(tuple, 1) * 2) == tuple.Count)
                {
                    foreach (Field f in tuple)
                    {
                        if (f.value == null) { f.value = 0; }
                    }
                }
            }
        }
        #endregion

        #region Complete Line
        // Complete Row only missing one value
        private void CompleteLine(List<Field> tuple)
        {
            int countNull = Solver.CountElement(tuple, null);

            int countO = Solver.CountElement(tuple, 0);
            if (countNull == 1 && (countO * 2) == tuple.Count)
            {
                foreach (Field f in tuple)
                {
                    if (f.value == null) { f.value = 1; }
                }
            }

            int countX = Solver.CountElement(tuple, 1);
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
            if (Solver.CountElement(tuple, null) != 2 && Solver.CountElement(tuple, null) != 3) { return; }

            PreventIdenticalLinesIterator(tuple, Solver.GetAllColumns());
            PreventIdenticalLinesIterator(tuple, Solver.GetAllRows());
        }

        private void PreventIdenticalLinesIterator(List<Field> tuple, List<List<Field>> compareTo)
        {
            foreach (List<Field> line in compareTo)
            {
                if (Solver.CountElement(line, null) != 0) { continue; }
                int nullCount = Solver.CountElement(tuple, null);
                int differences = Solver.CompareLines(tuple, line);

                if (nullCount == 2 && differences == 2)
                {
                    int[] indexes = Solver.GetIndexesOf(tuple, null);

                    if (tuple[indexes[0]].value != tuple[indexes[1]].value)
                    {
                        tuple[indexes[0]].value = 1 - line[indexes[0]].value;
                        tuple[indexes[1]].value = 1 - line[indexes[1]].value;
                    }
                }

                // XOOX    XOOX
                // _O__ -> _OX_
                else if (nullCount == 3 && differences == 3 && Solver.GetNullMargin(tuple, 3) == 4)
                {
                    int index = -1;
                    for (int i = 0; i < tuple.Count; i++)
                    {
                        if (tuple[i].value == null)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (tuple[index + 1].value == null)
                    {
                        tuple[index + 1].value = 1 - line[index + 1].value;
                    }
                    else if (tuple[index + 2].value == null)
                    {
                        tuple[index + 2].value = 1 - line[index + 2].value;
                    }
                }
            }
        }
        #endregion

        #region Far Neighbors
        // O__O__X__O -> OX_O__X__O
        private void FarNeighbors(List<Field> tuple)
        {
            if (Solver.CountElement(tuple, null) != 6) { return; }
            int countO = Solver.CountElement(tuple, 0);
            int countX = Solver.CountElement(tuple, 1);
            if (countO != (countX + 2) && countX != (countO + 2)) { return; }
            int[] positions = new int[4];
            positions[0] = Solver.TupleContainsPattern(tuple, "o__o__x__o");
            positions[1] = Solver.TupleContainsPattern(tuple, "o__x__o__o");
            positions[2] = Solver.TupleContainsPattern(tuple, "x__x__o__x");
            positions[3] = Solver.TupleContainsPattern(tuple, "x__o__x__x");

            if (positions[0] != -1) { tuple[positions[0] + 1].value = 1; }
            if (positions[1] != -1) { tuple[positions[1] + 8].value = 1; }
            if (positions[2] != -1) { tuple[positions[2] + 1].value = 0; }
            if (positions[3] != -1) { tuple[positions[3] + 8].value = 0; }
        }
        #endregion

        #region Far Siblings
        // O____O -> OX__XO
        private void FarSiblings(List<Field> tuple)
        {
            int countNull = 0;
            for (int i = 0; i < tuple.Count; i++)
            {
                countNull = tuple[i].value == null ? ++countNull : 0;
                if (countNull == 4 && (i - 4) >= 0 && (i + 1) < tuple.Count)
                {
                    if (tuple[i - 4].value == tuple[i + 1].value)
                    {
                        tuple[i - 3].value = 1 - tuple[i - 4].value;
                        tuple[i].value = 1 - tuple[i - 4].value;
                    }
                    break;
                }
            }
        }
        #endregion
    }
}
