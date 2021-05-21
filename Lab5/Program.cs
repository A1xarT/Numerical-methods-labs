using System;
using System.IO;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            ShowTable();
            int a = 2, b = 8, maxPower = 50;
            double eps = 0.01, deviation;
            FunctionApproximation approximation;
            LinearSystemSolver solver = new LinearSystemSolver();
            for (int i = 1; i <= maxPower; i++)
            {
                approximation = new FunctionApproximation(a, b, eps, i);
                var equations = approximation.GetLinearSystem();
                var xValues = solver.GaussianEelimination(equations);
                deviation = approximation.GetDeviation(xValues);
                if (deviation <= eps)
                {
                    Console.WriteLine($"Epsilon = {eps}");
                    Console.WriteLine($"Power of polynomial = {i - 1}");
                    Console.WriteLine($"Least Squares Deviation = {deviation}");
                    // Points
                    int n = 50;
                    double h = (double)(b - a) / n, x, Px;
                    using (var sw = new StreamWriter("Points.csv"))
                    {
                        for (int k = 0; k <= n; k++)
                        {
                            x = a + k * h;
                            Px = approximation.Pm(xValues, x);
                            sw.WriteLine($"{Math.Round(x, 3)};{Px}");
                        }
                    }
                    Console.WriteLine("CSV file created");
                    break;
                }
            }
            Console.ReadKey();
        }
        static void ShowTable()
        {
            int a = 2, b = 8, maxPower = 50;
            double eps = 0.1, deviation;
            FunctionApproximation approximation;
            LinearSystemSolver solver = new LinearSystemSolver();
            Console.WriteLine("|Epsilon|Power of polynomial|Least Squares Deviation|");
            Console.WriteLine("|_______|___________________|_______________________|");
            while (eps > 0.01)
            {
                for (int i = 1; i <= maxPower; i++)
                {
                    approximation = new FunctionApproximation(a, b, eps, i);
                    var equations = approximation.GetLinearSystem();
                    var xValues = solver.GaussianEelimination(equations);
                    deviation = approximation.GetDeviation(xValues);
                    if (deviation <= eps)
                    {
                        Console.Write("{0, -8}", $"|  {eps}");
                        Console.Write("{0, -20}", $"|         {i - 1}");
                        Console.WriteLine("{0, -20}", $"|       {Math.Round(deviation, 8)}      |");
                        Console.WriteLine("|_______|___________________|_______________________|");
                        break;
                    }
                }
                eps = Math.Round(eps - 0.02, 3);
            }
        }
    }
}
