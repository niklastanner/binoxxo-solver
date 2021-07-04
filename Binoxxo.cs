using System;
using System.Collections.Generic;

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

        public int?[] GetRow(int index)
        {
            index = index / size * size;
            int?[] row = new int?[size];

            int j = 0;
            for (int i = index; i < (index + size); i++)
            {
                row[j] = Get(i);
                j++;
            }

            return row;
        }

        public List<int?[]> GetAllRows()
        {
            List<int?[]> rows = new List<int?[]>();

            for (int i = 0; i < size; i++)
            {
                int index = i * size;
                rows.Add(GetRow(index));
            }

            return rows;
        }

        public int?[] GetColumn(int index)
        {
            index = index % size;
            int?[] row = new int?[size];

            int j = 0;
            for  (int i = index; i < GetCountFields(); i += size)
            {
                row[j] = Get(i);
                j++;
            }

            return row;
        }

        public List<int?[]> GetAllColumns()
        {
            List<int?[]> columns = new List<int?[]>();

            for (int i = 0; i < size; i++)
            {
                columns.Add(GetColumn(i));
            }

            return columns;
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
