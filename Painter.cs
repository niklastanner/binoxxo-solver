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
            try
            {
                Console.CursorVisible = false;
                while (true)
                {
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine(binoxxo.PrintBinoxxo());
                }
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}
