using System.Collections.Generic;

namespace Binoxxo_Solver
{
    class Worker
    {
        public void Run(object param)
        {
            List<int?[]> tuples = (List<int?[]>)param;

            do
            {
                // Do the cool stuff here
            } while (!Solver.IsSolved());
        }
    }
}
