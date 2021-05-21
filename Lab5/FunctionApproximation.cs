using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    class FunctionApproximation
    {
        int a, b, N;
        double eps;
        public FunctionApproximation(int _a, int _b, double _eps, int _n)
        {
            a = _a;
            b = _b;
            eps = _eps;
            N = _n;
        }
        public double GetDeviation(List<double> xValues)
        {
            double h = (double)(b - a) / N, deviation = 0, Px, x;
            for (int i = 0; i <= N; i++)
            {
                x = a + i * h;
                Px = Pm(xValues, x);
                deviation += Math.Pow(Function(x) - Px, 2);
            }
            return Math.Sqrt(deviation / (N + 1));

        }
        public double Pm(List<double> xValues, double x)
        {
            double Pm = 0;
            for (int i = 0; i < N; i++)
            {
                Pm += xValues[i] * LegendrePolynomial(i, x);
            }
            return Pm;
        }
        public List<List<double>> GetLinearSystem()
        {
            double[][] matrix = new double[N][];
            for (int i = 0; i < N; i++)
                matrix[i] = new double[N + 1];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = RungeMethod(i, j);
                }
            }
            List<List<double>> lst = new List<List<double>> { };
            foreach (double[] subArr in matrix)
                lst.Add(subArr.ToList());
            return lst;
        }
        public double Function(double x)
        {
            return 0.5 * Math.Pow(Math.E, Math.Pow(x, (double)1 / 3)) * Math.Sin(3 * x);
        }
        double LegendrePolynomial(int n, double x)
        {
            if (n == 0) return 1;
            if (n == 1) return x;
            double Pn1 = 1, Pn2 = x, Pn = 0;
            for (int i = 2; i <= n; i++)
            {
                Pn = x * Pn2 * (2 * n + 1) / (n + 1) - Pn1 * n / (n + 1);
                Pn1 = Pn2;
                Pn2 = Pn;
            }
            return Pn;
        }
        double LegendreCell(int i, int j, double x)
        {
            if (j == N) return LegendrePolynomial(i, x) * Function(x);
            return LegendrePolynomial(i, x) * LegendrePolynomial(j, x);
        }
        double SimpsonsRule(int n, double h, int i, int j)
        {
            double res = LegendreCell(i, j, a) + LegendreCell(i, j, b), sig1 = 0, sig2 = 0;
            for (int k = 1; k <= n - 1; k += 2)
                sig1 += LegendreCell(i, j, a + k * h);
            for (int k = 2; k <= n - 2; k += 2)
                sig2 += LegendreCell(i, j, a + k * h);
            res += 4 * sig1 + 2 * sig2;
            return res * h / 3;
        }
        double RungeMethod(int i, int j)
        {
            if (eps > 0.01) eps = 0.01;
            int n = (int)((b - a) / Math.Sqrt(Math.Sqrt(eps)));
            double h = (double)(b - a) / n, In = SimpsonsRule(n, h, i, j), I2n = SimpsonsRule(n *= 2, h /= 2, i, j);
            while (Math.Abs((In - I2n) / I2n) > 15 *eps)
            {
                In = I2n;
                I2n = SimpsonsRule(n *= 2, h /= 2, i, j);
            }
            return I2n;
        }
    }
}
