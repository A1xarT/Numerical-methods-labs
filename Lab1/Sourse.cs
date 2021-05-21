// KV82 Lyubchich Illya
// Variant 16
// e^x  [-16.2; 5.7]
using System;

namespace Lab1
{
    class Sourse
    {
        public static double Xi(double a, double b, int i)
        {
            return a + (b - a) * i / 10;
        }
        public static void Main(string[] agrz)
        {
            double a = -16.2, b = 5.7;
            xDivided x = new xDivided((a + b) / 2);
            Exp_Calculation func = new Exp_Calculation();
            Console.WriteLine(" ____________________________________________________________");
            Console.WriteLine("|        |      |                     |                      |");
            Console.WriteLine("|  eps   |   n  |      Abs Error      |   Remainder Term     |");
            Console.WriteLine("|________|______|_____________________|______________________|");

            for (int i = 2; i <= 14; i += 3)
            {
                double eps = Math.Pow(10, -i);
                func.Calculate(x, eps);
                Console.Write("{0, -9}", "| " + "e-" + i);
                Console.Write("{0,-7}", "|  " + func.N);
                Console.Write("{0, -22}", "|" + func.AbsoluteError);
                Console.Write("{0,-23}", "|" + func.RemainderTerm);
                Console.Write("{0, -3}", "| ");
                Console.WriteLine();
            }
            Console.WriteLine("|________|______|_____________________|______________________|");
            int n = func.Calculate(x, Math.Pow(10, -8));
            Console.WriteLine(" __________________________________________________________________");
            Console.WriteLine("|                     |                     |                      |");
            Console.WriteLine("|         Xi          |      Abs Error      |   Remainder Term     |");
            Console.WriteLine("|_____________________|_____________________|______________________|");
            for (int i = 0; i < 11; i++)
            {
                double arg = Xi(a, b, i);
                func.Calculate(new xDivided(arg), n);
                Console.Write("{0, -22}", "| " + arg);
                Console.Write("{0, -22}", "|" + func.AbsoluteError);
                Console.Write("{0,-23}", "|" + func.RemainderTerm);
                Console.Write("{0, -3}", "| ");
                Console.WriteLine();
            }
            Console.Write("|_____________________|_____________________|______________________|");

            Console.ReadKey();
        }
    }
    public class xDivided
    {
        public xDivided(double x)
        {
            integer = (int)Math.Truncate(x);
            fractional = x - integer;
            entire = x;
        }
        public int integer;
        public double fractional;
        public double entire;
    }
    public class Exp_Calculation
    {
        public int N { get; set; }
        public double Value { get; set; }
        public double RemainderTerm { get; set; }
        public double AbsoluteError { get; set; }
        public int Calculate(xDivided arg, double E)
        {
            double SumOfSeq = 1;
            RemainderTerm = 1;
            int k = 0;
            while (Math.Abs(RemainderTerm) > E)
            {
                RemainderTerm *= (arg.entire) / (++k);
                SumOfSeq += RemainderTerm;
            }
            Value = SumOfSeq - RemainderTerm;
            if (arg.entire < 0)                             // if sign changes then abs(Rn) < abs(first thrown out) 
                RemainderTerm *= (arg.entire) / (k + 1);    // so we pick next one as R(n)
            AbsoluteError = Math.Abs(Math.Abs(Math.Exp(arg.entire)) - Value);
            N = k + 1;
            return N;
        }
        public void Calculate(xDivided arg, int N)
        {
            double Term = 1, SeqSum = 1;
            int k = 0;
            while (k < N - 1)
            {
                Term *= arg.entire / (++k);
                SeqSum += Term;
            }
            SeqSum -= Term;
            if (arg.entire < 0)                             // if sign changes then abs(Rn) < abs(first thrown out) 
                Term *= arg.entire / (k + 1);               // so we pick next one as R(n)
            RemainderTerm = Term;
            Value = SeqSum;
            AbsoluteError = Math.Abs(Math.Abs(Math.Exp(arg.entire)) - Value);
        }
    }

}