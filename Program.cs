namespace Binoxxo_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Binoxxo binoxxo = Factory.CreateBinoxxo();

            Solver solver = new Solver(binoxxo);
            solver.Solve();
        }
    }
}
