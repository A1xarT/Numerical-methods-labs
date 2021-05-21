using System;
using System.Text;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 1, b = 23;
            double e = 1e-10, h = 5e-6, result, error;
            Console.OutputEncoding = Encoding.Unicode;
            NumIntegraion integraion = new NumIntegraion(a, b);
            result = integraion.TrapeziumIntegration(h);
            error = Math.Abs(RealValue(a, b) - result);
            Console.WriteLine($"\t\t\t\t\tМетод трапецій\n");
            Console.WriteLine($"|Задана похибка, ε | Крок інтегрування | Точне значення інтеграла | Отримана похибка, Δ|");
            Console.Write("{0, -19}", $"|      {e}");
            Console.Write("{0, -20}", $"|       {integraion.step}");
            Console.Write("{0, -27}", $"|     {RealValue(a, b)}");
            Console.WriteLine("{0, -14}", $"|{error}|");
            Console.WriteLine($"\n\t\t\t    Метод Рунге\n");
            double prevError = error;
            result = integraion.RungeRule(prevError);
            error = Math.Abs(RealValue(a, b) - result);
            Console.WriteLine($"| Задана похибка, Δ  |  Крок інтегрування  |  Отримана похибка  |");
            Console.Write("{0, -19}", $"|{prevError}");
            Console.Write("{0, -20}", $"| {integraion.step}");
            Console.WriteLine("{0, -14}", $"|{error}|");
            Console.ReadKey();
        }
        static double RealValue(int a, int b)
        {
            return (Math.Pow(Math.Log(b), 3) - Math.Pow(Math.Log(a), 3)) / 3;
        }
    }
    class NumIntegraion
    {
        int a, b;
        public double step;
        public NumIntegraion(int _a, int _b)
        {
            a = _a;
            b = _b;
            step = 0;
        }
        public double TrapeziumIntegration(double h)
        {
            int n = (int)((b - a) / h); // b, a - цілі, h = 0.000005
            step = h;
            return TrapeziumRule(h, n);
        }

        double TrapeziumRule(double h, double n)
        {
            double result = 0, x1 = a, x2 = a + h;
            for (int i = 0; i < n; i++)
            {
                result += (function(x1) + function(x2)) * h / 2;
                x1 = x2;
                x2 = a + h * (i + 2);
            }
            return result;
        }
        double function(double x)
        {
            return Math.Log(x) * Math.Log(x) / x;
        }
        public double RungeRule(double e)
        {
            double In = 0, I2n = 0, h, Rn;
            int n = (int)(1 / Math.Sqrt(e));
            h = (double)(b - a) / n;
            In = TrapeziumRule(h, n);
            n *= 2;
            h /= 2;
            I2n = TrapeziumRule(h, n);
            Rn = Math.Abs(In - I2n) / 3;
            while (Rn > e)
            {
                n *= 2;
                h /= 2;
                In = I2n;
                I2n = TrapeziumRule(h, n);
                Rn = Math.Abs(In - I2n) / 3;
            }
            step = h;
            return I2n;
        }
    }
}
