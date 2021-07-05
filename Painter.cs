using System;
using System.Threading;

namespace Binoxxo_Solver
{
    class Painter
    {
        public void ContinuousPaintBinoxxo(object param)
        {
            Binoxxo binoxxo = (Binoxxo)param;
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            while (true)
            {
                Console.WriteLine(binoxxo.PrintBinoxxo());
                Console.SetCursorPosition(left, top);
            }
        }
    }
}
