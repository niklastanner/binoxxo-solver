using System;

namespace Binoxxo_Solver
{
    class Binoxxo
    {
        private int?[] game;
        public readonly int size;

        public Binoxxo(int?[] init)
        {
            game = init;
            size = (int)Math.Sqrt(game.Length);
        }

        public int? Get(int index)
        {
            return game[index];
        }

        public string GetChar(int index)
        {
            switch (Get(index))
            {
                case null:
                    return null;
                case 0:
                    return "O";
                case 1:
                    return "X";
                default:
                    throw new ArgumentException("Index not found");
            }
        }

        public void Set(int index, int value)
        {
            game[index] = value;
        }

        private int GetCountFields()
        {
            return (int)Math.Pow(size, 2);
        }

        public void PrintBinoxxo()
        {
            int size = this.size;

            string line = "";
            string separator = "";

            for (int i = 0; i < (size + 2); i++)
            {
                separator += "-";
            }

            Console.WriteLine(separator);

            for (int i = 0; i < GetCountFields(); i++)
            {
                if (i % size == 0)
                {
                    line += "|";
                }
                line += GetChar(i) == null ? "_" : GetChar(i);
                if (i % size == (size - 1))
                {
                    line += "|";
                    Console.WriteLine(line);
                    line = "";
                }
            }
            Console.WriteLine(separator);
        }
    }
}
