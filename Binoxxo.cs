using System;
using System.Collections.Generic;

namespace Binoxxo_Solver
{
    class Binoxxo
    {
        private readonly Field[] game;
        public readonly int size;

        public Binoxxo(int?[] init)
        {
            game = new Field[init.Length];
            for (int i = 0; i < game.Length; i++)
            {
                game[i] = new Field(init[i]);
            }
            size = (int)Math.Sqrt(game.Length);
        }

        public Field Get(int index)
        {
            return game[index];
        }

        public string GetChar(int index)
        {
            switch (Get(index).value)
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

        public List<Field> GetRow(int index)
        {
            index = index / size * size;
            List<Field> row = new List<Field>();

            for (int i = index; i < (index + size); i++)
            {
                row.Add(Get(i));
            }

            return row;
        }

        public List<List<Field>> GetAllRows()
        {
            List<List<Field>> rows = new List<List<Field>>();

            for (int i = 0; i < size; i++)
            {
                int index = i * size;
                rows.Add(GetRow(index));
            }

            return rows;
        }

        public List<Field> GetColumn(int index)
        {
            index = index % size;
            List<Field> row = new List<Field>();

            for (int i = index; i < GetCountFields(); i += size)
            {
                row.Add(Get(i));
            }

            return row;
        }

        public List<List<Field>> GetAllColumns()
        {
            List<List<Field>> columns = new List<List<Field>>();

            for (int i = 0; i < size; i++)
            {
                columns.Add(GetColumn(i));
            }

            return columns;
        }

        public void Set(int index, int value)
        {
            game[index].value = value;
        }

        private int GetCountFields()
        {
            return (int)Math.Pow(size, 2);
        }

        public string PrintBinoxxo()
        {
            int size = this.size;

            string line = "";
            string separator = "";
            string output = "";

            for (int i = 0; i < ((size + 2) * 2 - 1); i++)
            {
                separator += "-";
            }
            output += separator + "\n";

            for (int i = 0; i < GetCountFields(); i++)
            {
                if (i % size == 0)
                {
                    line += "|";
                }
                line += GetChar(i) == null ? " _" : " " + GetChar(i);
                if (i % size == (size - 1))
                {
                    line += " |";
                    output += line + "\n";
                    line = "";
                }
            }
            output += separator + "\n";
            return output;
        }
    }
}
