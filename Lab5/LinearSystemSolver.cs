using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    class LinearSystemSolver
    {
        public List<double> GaussianEelimination(List<List<double>> matrx)
        {
            int n = matrx.Count;
            List<List<double>> matrix = matrx.ToList();
            for (int i = 0; i < n; i++)
                matrix[i] = matrx[i].ToList();
            List<double> xValues = new List<double> { };

            for (int i = 0; i < n; i++)
            {
                if (matrix[i][i] == 0) continue;
                matrix[i] = DivideRow(i, matrix[i][i], n, matrix);
                for (int j = i + 1; j < n; j++)
                {
                    matrix[j] = SubstractRow(j, MultiplyRow(i, matrix[j][i], n, matrix), n, matrix);
                }
            }
            for (int i = n - 1; i >= 0; i--)
            {
                double Xi = matrix[i].Last();
                for (int j = 0; j < n - 1 - i; j++)
                {
                    Xi -= xValues[j] * matrix[i][n - 1 - j];
                }
                xValues.Add(Xi);
            }
            xValues.Reverse();
            return xValues;
        }
        List<double> MultiplyRow(int i, double value, int n, List<List<double>> matrix)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] * value);
            return lst;
        }
        List<double> DivideRow(int i, double value, int n, List<List<double>> matrix)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] / value);
            return lst;
        }
        List<double> SubstractRow(int i, List<double> _lst, int n, List<List<double>> matrix)
        {
            List<double> lst = new List<double> { };
            for (int j = 0; j < n + 1; j++)
                lst.Add(matrix[i][j] - _lst[j]);
            return lst;
        }
    }
}
