using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6
{
    class CauchyProblem
    {
        public static List<Point> RungeKuttaMethod(double a, double b, double h, double y0)
        {
            List<Point> points = new List<Point> { };
            int n = (int)Math.Round((b - a) / h);
            double k1, k2, k3, k4, deltaY, x = a, y = y0;
            points.Add(new Point(x, y));
            for (int i = 1; i <= n; i++)
            {
                k1 = h * Function(x);
                k2 = h * Function(x + h / 2);
                k3 = h * Function(x + h / 2);
                k4 = h * Function(x + h);
                deltaY = (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                y += deltaY;
                x = a + i * h;
                points.Add(new Point(x, y));
            }
            return points;
        }
        private static double GetY(double a, double y0, double xEnd, double h)
        {
            if (xEnd < a) throw new Exception("Wrong parameter");
            int n = (int)Math.Round((xEnd - a) / h);
            double k1, k2, k3, k4, deltaY, x = a, y = y0;
            for (int i = 1; i <= n; i++)
            {
                k1 = h * Function(x);
                k2 = h * Function(x + h / 2);
                k3 = h * Function(x + h / 2);
                k4 = h * Function(x + h);
                deltaY = (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                y += deltaY;
                x = a + i * h;
            }
            return y;
        }
        public static double RungePrinciple(double a, double b, double y0, double eps, int r)
        {
            double y1, y2, h = Math.Pow(eps, (double)1 / r), point = a + h, factor = Math.Pow(2, r) - 1;
            y1 = GetY(a, y0, point, h);
            y2 = GetY(a, y0, point, h /= 2);
            while (Math.Abs(y1 - y2) / factor > eps)
            {
                y1 = y2;
                y2 = GetY(a, y0, point, h /= 2);
            }
            return h;
        }
        public static List<Point> AdamsMethod(double a, double b, double h, double y0)
        {
            List<Point> points = new List<Point> { };
            int n = (int)Math.Round((b - a) / h);
            List<double> constantsM3 = new List<double> { (double)-3 / 8, (double)37 / 24, (double)-59 / 24, (double)55 / 24 },
                funcValues = new List<double> { 0, 0, 0, 0 };
            // Y(0)...Y(3)
            points.AddRange(RungeKuttaMethod(a, a + 3 * h, h, y0));
            // f(0)...f(3)
            for(int i = 0; i <= 3; i++)
            {
                funcValues[i] = Function(a + i * h);
            }
            double x, y = points.Last().Y;
            // Y(4)...Y(n)
            for (int i = 4; i <= n; i++)
            {
                x = a + i * h;
                for (int j = 0; j < constantsM3.Count; j++)
                {
                    y += h * constantsM3[j] * funcValues[j];
                }
                points.Add(new Point(x, y));
                ShiftLeft(funcValues, Function(x));
            }
            return points;
        }
        private static void ShiftLeft(List<double> lst, double insertItem)
        {
            for (int i = 0; i < lst.Count - 1; i++)
            {
                lst[i] = lst[i + 1];
            }
            lst[lst.Count - 1] = insertItem;
        }
        private static double Function(double x)
        {
            return 5 * ((Math.Log(x) + Math.Log(10 * x)) * Math.Sin(6 * x) + 6 * x * Math.Log(x) * Math.Log(10 * x) * Math.Cos(6 * x)) / (x * Math.Log(10));
        }
    }
}
