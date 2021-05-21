using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            double a = 1, b = 25, y0 = 0, eps = 1E-4, h = (b - a) / 10, hDivider = 2, error;
            error = RungeKuttaTable(a, b, h, y0, eps, hDivider);
            RungeStepTable(a, b, y0, new List<double> { eps, error }, 4);
            AdamsTable(a, b, h, y0, eps, hDivider);
            WriteCSV(a, b, y0, (b - a) / 10);
            Console.WriteLine("\n CSV files created");
            Console.ReadKey();
        }
        static double RealValue(double x)
        {
            return 5 * Math.Log10(x) * Math.Log(10 * x) * Math.Sin(6 * x);
        }
        static double AvgError(List<Point> points)
        {
            List<double> errors = new List<double> { };
            for (int i = 0; i < points.Count; i++)
            {
                errors.Add(Math.Abs(points[i].Y - RealValue(points[i].X)));
            }
            return errors.Average();
        }
        static double RungeKuttaTable(double a, double b, double h, double y0, double eps, double divider)
        {
            Console.WriteLine($"\n\t Runge-Kutta method, ε = {eps}\n");
            Console.WriteLine(" |  Average error, Δ    |      Step value      |");
            var points = CauchyProblem.RungeKuttaMethod(a, b, h, y0);
            double error = AvgError(points);
            Console.WriteLine($" | {error,-20} | {h,-20} |");
            while (error > eps)
            {
                points = CauchyProblem.RungeKuttaMethod(a, b, h /= divider, y0);
                error = AvgError(points);
                Console.WriteLine($" | {error,-20} | {h,-20} |");
            }
            Console.WriteLine(" |______________________|______________________|");
            return error;
        }
        static void RungeStepTable(double a, double b, double y0, List<double> epsList, int r)
        {
            Console.WriteLine($"\n\t\t\t Finding step with Runge Principle\n");
            Console.WriteLine(" |  Expected error, ε   |      Step value      |  Avg error with Runge-Kutta, Δ |");
            foreach (double eps in epsList)
            {
                double stepByRunge = CauchyProblem.RungePrinciple(a, b, y0, eps, r);
                double error = AvgError(CauchyProblem.RungeKuttaMethod(a, b, stepByRunge, y0));
                Console.WriteLine($" | {eps,-20} | {stepByRunge,-20} |    {error,-27} |");
            }
            Console.WriteLine(" |______________________|______________________|________________________________|");
        }
        static void AdamsTable(double a, double b, double h, double y0, double eps, double divider)
        {
            Console.WriteLine($"\n\t Multistep Adams method, ε = {eps}\n");
            Console.WriteLine(" |  Average error, Δ    |      Step value      |");
            var points = CauchyProblem.AdamsMethod(a, b, h, y0);
            double error = AvgError(points);
            Console.WriteLine($" | {error,-20} | {h,-20} |");
            while (error > eps)
            {
                points = CauchyProblem.AdamsMethod(a, b, h /= divider, y0);
                error = AvgError(points);
                Console.WriteLine($" | {error,-20} | {h,-20} |");
            }
            Console.WriteLine(" |______________________|______________________|");
        }
        static void WriteCSV(double a, double b, double y0, double h)
        {
            var points = CauchyProblem.RungeKuttaMethod(a, b, h, y0);
            using (var sw = new StreamWriter("RungeKuttaPoints.csv"))
            {
                foreach (Point p in points)
                {
                    sw.WriteLine($"{p.X};{p.Y}");
                }
            }
            points = CauchyProblem.AdamsMethod(a, b, h, y0);
            using (var sw = new StreamWriter("AdamsPoints.csv"))
            {
                foreach (Point p in points)
                {
                    sw.WriteLine($"{p.X};{p.Y}");
                }
            }
        }
    }
}
