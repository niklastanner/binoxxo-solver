using System;

namespace Binoxxo_Solver
{
    class Factory
    {
        public static Binoxxo CreateBinoxxo()
        {
            Console.WriteLine("Reading Binoxxo from user input");
            Console.Write("Line 1: ");
            string input = Console.ReadLine();
            char[] line = input.ToCharArray();
            int?[] init = new int?[(int)Math.Pow(line.Length, 2)];
            int gameSize = line.Length;
            int fullSize = init.Length;
            bool first = true;

            for (int i = 0; i < fullSize; i += gameSize)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.Write("Line {0}: ", i / gameSize + 1);
                    input = Console.ReadLine();
                    line = input.ToCharArray();
                }

                try
                {
                    if (input.Length == gameSize)
                    {
                        for (int j = 0; j < gameSize; j++)
                        {
                            string str = line[j].ToString();
                            int? value;
                            if (str.Equals(" "))
                            {
                                value = null;
                            }
                            else if (str.Equals("O") || str.Equals("o") || str.Equals("0"))
                            {
                                value = 0;
                            }
                            else if (str.Equals("X") || str.Equals("x") || str.Equals("1"))
                            {
                                value = 1;
                            }
                            else
                            {
                                throw new ArgumentException("Input invalid");
                            }
                            init[i + j] = value;
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException($"Input does not contain {gameSize} characters");
                    }
                }
                catch (Exception e)
                {
                    if (e is IndexOutOfRangeException || e is ArgumentException)
                    {
                        Console.WriteLine(e.Message);
                        i -= gameSize;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            Console.WriteLine("");

            return new Binoxxo(init);
        }
    }
}
