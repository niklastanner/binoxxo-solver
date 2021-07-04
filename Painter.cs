using System;
using System.Threading;

namespace Binoxxo_Solver
{
    class Painter
    {
        public void ContinuousPaintBinoxxo(object param)
        {
            Binoxxo binoxxo = (Binoxxo)param;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Trying to solve binoxxo:");
                Console.WriteLine(binoxxo.PrintBinoxxo());
                Thread.Sleep(100);
            }
        }
    }
}
