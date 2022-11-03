using System;
using System.Linq.Expressions;

namespace DD
{
    public static class Programm
    {
        public static void Main()
        {
            Lab5_1();
        }
        public static void Lab5_1()
        {
            bool exit = false;

            double oldDoubleNumber = double.MinValue;

            while (!exit)
            {
                Console.Write("Введите число: ");

                string input = Console.ReadLine();

                if (input == "q")
                    return;

                if (IsFloatingPointNumber(input))
                {
                    double tempDoubleNumber = double.Parse(input);

                    if (tempDoubleNumber == oldDoubleNumber)
                        return;
                    oldDoubleNumber = tempDoubleNumber;
                }
                else
                {
                    oldDoubleNumber = double.MinValue;

                    int integer = int.Parse(input);

                    Console.WriteLine($"Целове число {integer} будет иметь символ в Юникоде {Convert.ToChar(integer)}");
                }
            }
        }
        public static bool IsFloatingPointNumber(string textForCheck)
        {
            foreach (char sybol in textForCheck)
                if (sybol == ',')
                    return true;
            return false;
        }
    }
}