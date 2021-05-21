using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            double a1 = -2.5, b1 = -2,  // First root
                a2 = -0.5, b2 = 0;      // Second root
            Output(a1, b1, a2, b2, true);
            Output(a1, b1, a2, b2, false);
            CompareOuput(a1, b1);
            Console.ReadKey();
        }
        static void CompareOuput(double a, double b)
        {
            var EqSolver = new EquationSolver(a, b);
            Console.WriteLine("Methods comparison (first root)");
            Console.WriteLine(" _________________________________________________________");
            Console.WriteLine("|        | Number of iterations |   Number of iterations  |");
            Console.WriteLine("|  eps   |by Succ-Approxs method| by NewtonRaphson method |");
            Console.WriteLine("|________|______________________|_________________________|");
            double Eps = 0.01;
            int it1, it2;
            for (int i = 0; i <= 4; i++)
            {
                EqSolver.SuccessiveApproximations_method(Eps);
                it1 = EqSolver.iteration_count;
                EqSolver.NewtonRaphson_method(Eps);
                it2 = EqSolver.iteration_count;
                Console.Write("{0, -9}", $"|{Eps}");
                Console.Write("{0, -23}", $"|{it1}");
                Console.Write("{0, -26}", $"|{it2}");
                Console.Write("{0, -3}", "| ");
                Console.WriteLine();
                Eps *= 0.001;
            }
            Console.WriteLine("_________|______________________|_________________________|");
        }
        static void Output(double a1, double b1, double a2, double b2, bool isIterative)
        {
            var EqSolver = new EquationSolver(a1, b1);
            if(isIterative)Console.WriteLine("Method of Successive Approximations\n");
            else Console.WriteLine("NewtonRaphson method\n");
            Console.WriteLine("First root of the equation");
            Console.WriteLine(" _________________________________________________");
            Console.WriteLine("|        |                  |                     |");
            Console.WriteLine("|  eps   |    Root value    |  Absolute Error     |");
            Console.WriteLine("|________|__________________|_____________________|");
            double x, Eps = 0.01;
            for (int i = 0; i <= 4; i++)
            {
                x = isIterative? EqSolver.SuccessiveApproximations_method(Eps):EqSolver.NewtonRaphson_method(Eps);
                Console.Write("{0, -9}", $"|{Eps}");
                Console.Write("{0, -19}", $"|{x}");
                Console.Write("{0, -22}", $"|{EqSolver.Error}");
                Console.Write("{0, -3}", "| ");
                Console.WriteLine();
                Eps *= 0.001;
            }
            Console.WriteLine("|________|__________________|_____________________|");

            EqSolver.a = a2; EqSolver.b = b2;
            Console.WriteLine("Second root");
            Console.WriteLine(" _________________________________________________");
            Console.WriteLine("|        |                  |                     |");
            Console.WriteLine("|  eps   |    Root value    |  Absolute Error     |");
            Console.WriteLine("|________|__________________|_____________________|");
            Eps = 0.01;
            for (int i = 0; i <= 4; i++)
            {
                x = isIterative ? EqSolver.SuccessiveApproximations_method(Eps) : EqSolver.NewtonRaphson_method(Eps);
                Console.Write("{0, -9}", $"|{Eps}");
                Console.Write("{0, -19}", $"|{x}");
                Console.Write("{0, -22}", $"|{EqSolver.Error}");
                Console.Write("{0, -3}", "| ");
                Console.WriteLine();
                Eps *= 0.001;
            }
            Console.WriteLine("|________|__________________|_____________________|");
        }
    }
    class EquationSolver
    {
        public double a, b, Error;
        public int iteration_count;
        public EquationSolver(double _a, double _b)
        {
            a = _a;
            b = _b;
        }
        public double SuccessiveApproximations_method(double E)
        {
            double m1 = deriv_1(a), M1 = deriv_1(b), lambda, Xk1, Xk2, q;
            iteration_count = 1;
            if(Math.Abs(m1) > Math.Abs(M1))
            {
                double tmp = m1;
                m1 = M1;
                M1 = tmp;
            }
            lambda = 1 / M1;
            q = 1 - Math.Abs(m1 / M1);

            Xk1 = (a + b)/2;
            Xk2 = Xk1 - lambda * func(Xk1);
            while(Math.Abs(Xk2 - Xk1) > (E*(1-q)/q))
            {
                Xk1 = Xk2;
                Xk2 = Xk1 - lambda * func(Xk1);
                iteration_count++;
            }
            Error = Math.Abs(Xk2 - Xk1) * q / (1 - q);
            return Xk2;
        }
        public double NewtonRaphson_method(double E)
        {
            double m1, Xk;
            iteration_count = 1;
            m1 = Math.Abs(deriv_1(a));
            for(double d = a + 0.01; d <= b; d += 0.01)
            {
                double tmp = Math.Abs(deriv_1(d));
                if (tmp < m1)
                    m1 = tmp;
            }
            if (a * deriv_2(a) > 0)
                Xk = a;
            else 
                Xk = b;
            Xk = Xk - func(Xk) / deriv_1(Xk);
            while(Math.Abs(func(Xk))/m1 > E)
            {
                Xk = Xk - func(Xk) / deriv_1(Xk);
                iteration_count++;
            }
            Error = Math.Abs(func(Xk)) / m1;
            return Xk;
        }
        double func(double x)
        {
            return Math.Pow(x, 4) + 2 * Math.Pow(x, 3) + Math.Sin(x) + 0.5;
        }
        double deriv_1(double x)
        {
            return 4 * Math.Pow(x, 3) + 6 * Math.Pow(x, 2) + Math.Cos(x);
        }
        double deriv_2(double x)
        {
            return 12 * Math.Pow(x, 2) + 12 * x - Math.Sin(x);
        }
    }
}
