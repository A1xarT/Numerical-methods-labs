using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            double Eps = 1E-10; 
            List<List<double>> matrix = new List<List<double>> { new List<double> { 20, 13, 3, 8 }, new List<double> { 2, 24, 7, 14 },
                new List<double> { 10, 14, 29, 4 }, new List<double> { 10, 2, 0, 19} };
            List<double> rightColumn = new List<double> { 61, 114, 108, 44};

            LinearSystemSolver Solver = new LinearSystemSolver(matrix, rightColumn);
            Console.WriteLine("Gauss-Jordan method\n");
            Solver.ConsoleOut();
            var result = Solver.GaussJordanMethod();
            if (Solver.checkSolve(result, Eps))
                Console.WriteLine("Solved");
            Console.Write("Roots: ");
            for (int i = 0; i < result.Count; i++)
                Console.Write($"x{i + 1} = {result[i]} ");
            Console.WriteLine("\n");
            // Modified matrix
            //matrix = new List<List<double>> { new List<double> { 3, -1, 1, 0 }, new List<double> { 17, 51, 13, 20 },
            //    new List<double> { 0, 3, 7, 3 }, new List<double> { 1, 6, 1, 10 } };
            //rightColumn = new List<double> { 7, 407, 28, 33 };

            Solver = new LinearSystemSolver(matrix, rightColumn);
            Console.WriteLine("Gauss-Seidel method\n");
            Solver.ConsoleOut();
            double error = 0;
            result = Solver.GaussSeidelMethod(Eps, ref error);
            if (Solver.checkSolve(result, Eps))
                Console.WriteLine("Solved");
            Console.Write("Roots: ");
            for (int i = 0; i < result.Count; i++)
                Console.Write($"x{i + 1} = {result[i]} ");
            Console.WriteLine($"\nAbsolute error = {error}");
            Console.ReadKey();
        }
    }
    class LinearSystemSolver
    {
        int n;
        List<List<double>> matrix;
        public LinearSystemSolver(List<List<double>> matr, List<double> lastCol)
        {
            n = lastCol.Count;
            matrix = matr.ToList();
            for (int i = 0; i < n; i++)
                matrix[i].Add(lastCol[i]);
        }
        public List<double> GaussJordanMethod()
        {
            List<double> xValues = new List<double> { };
            for (int i = 0; i < n; i++)
            {
                if (matrix[i][i] != 0)
                    matrix[i] = DivideRow(i, matrix[i][i]);
                for (int j = 0; j < i; j++)
                    matrix[j] = SubstractRow(j, MultiplyRow(i, matrix[j][i]));
                for (int j = i + 1; j < n; j++)
                    matrix[j] = SubstractRow(j, MultiplyRow(i, matrix[j][i]));
            }
            for (int j = 0; j < n; j++)
                xValues.Add(matrix[j][n]);
            return xValues;
        }
        public List<double> GaussSeidelMethod(double E, ref double error)
        {
            List<List<double>> tmpMatrix = new List<List<double>> { };
            foreach (List<double> lst in matrix)
                tmpMatrix.Add(lst.ToList());
            List<double> xValues = new List<double> { };
            double tmp, q = 0, sum;
            // Rebuilding matrix to X = B + aX
            for (int i = 1; i < n; i++)
            {
                tmp = tmpMatrix[i][0];
                tmpMatrix[i][0] = tmpMatrix[i][i];
                for (int j = i; j > 1; j--)
                    tmpMatrix[i][j] = tmpMatrix[i][j - 1];
                tmpMatrix[i][1] = tmp;
            }
            for (int i = 0; i < n; i++)
            {
                tmp = tmpMatrix[i][n];
                for (int j = n; j > 1; j--)
                    tmpMatrix[i][j] = tmpMatrix[i][j - 1];
                tmpMatrix[i][1] = tmp;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 2; j < n + 1; j++)
                    tmpMatrix[i][j] *= -1;
                for (int j = 1; j < n + 1; j++)
                    tmpMatrix[i][j] /= tmpMatrix[i][0];
                tmpMatrix[i][0] = 1;
            }
            // Calculating q value
            for (int i = 0; i < n; i++)
            {
                sum = 0;
                for (int j = 2; j < n + 1; j++)
                {
                    sum += Math.Abs(tmpMatrix[i][j]);
                }
                if (sum > q && sum < 1)
                    q = sum;
            }
            // Setting Xi = Bi
            for (int i = 0; i < n; i++)
                xValues.Add(tmpMatrix[i][1]);
            double diff;
            bool flag = true;
            List<double> prevX = xValues.ToList();
            while (flag)
            {
                flag = false;
                for (int i = 0; i < n; i++)
                {
                    sum = 0;
                    for (int j = 2; j < n + 1; j++)
                        sum += xValues[GetIndex(i, j)] * tmpMatrix[i][j];
                    xValues[i] = tmpMatrix[i][1] + sum;
                }
                for (int i = 0; i < n; i++)
                {
                    diff = Math.Abs(xValues[i] - prevX[i]);
                    if (diff > E * (1 - q) / q)
                    {
                        flag = true;
                        break;
                    }
                }
                error = 0;
                for (int i = 0; i < prevX.Count; i++)
                    error += Math.Abs(xValues[i] - prevX[i]);
                error /= xValues.Count;
                prevX = xValues.ToList();
            }
            return xValues;
        }
        int GetIndex(int _i, int _j)
        {
            _j -= 2;
            if (_j < _i) return _j;
            return _j + 1;
        }
        List<double> MultiplyRow(int i, double value)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] * value);
            return lst;
        }
        List<double> DivideRow(int i, double value)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] / value);
            return lst;
        }
        List<double> SubstractRow(int i, List<double> _lst)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] - _lst[j]);
            return lst;
        }
        public bool checkSolve(List<double> lst, double E)
        {
            double[] errors = new double[n];
            for(int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                    sum += matrix[i][j] * lst[j];
                errors[i] = sum - matrix[i][n];
            }
            if (errors.All(t => t < E))
                return true;
            return false;
        }
        public void ConsoleOut()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - 1; j++)
                    Console.Write("{0, -6} ", $"({matrix[i][j]})x{j+1} +");
                Console.WriteLine("{0, -11} ", $"({matrix[i][n-1]})x{n} = {matrix[i][n]}");
            }
        }
    }
}
