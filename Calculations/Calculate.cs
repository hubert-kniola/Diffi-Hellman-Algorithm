using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.Linq;

namespace Calculations
{
    public class Calculate
    {
        public static TimeSpan Time(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        public static void difiAlgorithm()
        {
            Stopwatch sw = Stopwatch.StartNew();
            BigInteger n, g;
            TimeSpan timeN, timeG;
            List<BigInteger> listG;

            sw.Start();
            n = GeneratePrime();
            sw.Stop();
            timeN = sw.Elapsed;
            sw.Start();
            g = FindPrimitive(n);
            sw.Stop();
            timeG = sw.Elapsed;

            Console.WriteLine($"Primitive root: {IsPrimitiveRoot(g, n)}");
            Console.WriteLine($"Value of n: {n} | Generation time [ms]: {timeN}");
            Console.WriteLine($"Value of g: {g} | Generation time [ms]: {timeG}");
            BigInteger x = 0;
            BigInteger y = 0;
            sw.Start();
            var ix = GenerateXY(10, 100); //male x
            sw.Stop();
            Console.WriteLine($"Value of x: {ix} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var iy = GenerateXY(10, 100); //male y
            sw.Stop();
            Console.WriteLine($"Value of y: {iy} | Generation time [ms]: {sw.Elapsed}");

            sw.Start();
            x = GenKey(g, ix, n); //duze X
            sw.Stop();
            Console.WriteLine($"Private Key: {x}, generating [ms]: {sw.Elapsed}");
            sw.Start();
            y = GenKey(g, iy, n); //duze Y
            sw.Stop();
            Console.WriteLine($"Public Key: {y}, generating [ms]: {sw.Elapsed}");


            var kA = GenKey(y, ix, n);
            var kB = GenKey(x, iy, n);
            Console.WriteLine($"kA: {kA}\nkB: {kB}");
        }

        private static BigInteger Power(BigInteger x, BigInteger y, BigInteger p)
        {
            BigInteger res = 1;
            x = x % p;

            while (y > 0)
            {
                if (y % 2 == 1)
                    res = (res * x) % p;

                y = y >> 1;
                x = (x * x) % p;
            }
            return res;
        }

        private static void FindFactors(List<BigInteger> s, BigInteger n)
        {
            while (n % 2 == 0)
            {
                s.Add(2);
                n = n / 2;
            }

            for (int i = 3; i <= Math.Sqrt((int)n); i = i + 2)
            {
                while (n % i == 0)
                {
                    s.Add(i);
                    n = n / i;
                }
            }

            if (n > 2)
                s.Add(n);
        }

        private static BigInteger FindPrimitive(BigInteger n)
        {
            List<BigInteger> s = new List<BigInteger>();

            if (CheckPrime(n) == false)
                return -1;

            BigInteger phi = n - 1;
            FindFactors(s, phi);

            for (int r = 2; r <= phi; r++)
            {
                bool flag = false;
                foreach (int a in s)
                {
                    if (Power(r, phi/a, n) == 1)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                    return r;
            }
            return -1;
        }

        private static BigInteger GenerateNumber(BigInteger N)
        {
            Random rnx = new Random();
            BigInteger sd = 0;
            do
                sd = rnx.Next(100, 10000000);
            while (BigInteger.GreatestCommonDivisor(sd, N) != 1);
            return sd;
        }

        private static List<BigInteger> PrimitiveRoots(BigInteger modulo)
        {
            var coprimes = new List<BigInteger>();

            for (int i = 1; i < modulo; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, modulo) == 1)
                {
                    coprimes.Add(i);
                }
            }

            var primitiveRoots = new List<BigInteger>();
            for (int j = 1; j < modulo; j++)
            {
                var resultSet = new List<BigInteger>();

                for (int k = 1; k < modulo; k++)
                {
                    resultSet.Add(BigInteger.Pow(j, k) % modulo);
                }

                if (coprimes.TrueForAll(resultSet.Contains))
                {
                    primitiveRoots.Add(j);
                }
            }

            return primitiveRoots;
        }

        private static bool IsPrimitiveRoot(BigInteger modulo, BigInteger arg)
        {
            var coprimes = new List<BigInteger>();

            for (int i = 0; i < modulo; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, modulo) == 1)
                {
                    coprimes.Add(i);
                }
            }

            var resultSet = new List<BigInteger>();

            for (int i = 0; i < modulo; i++)
            {
                resultSet.Add(BigInteger.Pow(arg, i) % modulo);
            }

            return coprimes.TrueForAll(resultSet.Contains);
        }

        private static BigInteger GeneratePrime()
        {
            BigInteger x;
            do
                x = GenerateNumber(1000);
            while (!CheckPrime(x));
            return x;
        }

        private static bool CheckPrime(BigInteger n)
        {
            var isPrime = true;
            var sqrt = Math.Sqrt((int)n);
            if ((n % 2) == 0)
                isPrime = false;
            for (var i = 3; i <= sqrt; i += 2)
                if ((n % i) == 0) isPrime = false;
            return isPrime;
        }

        private static BigInteger GenerateXY(int min, int max)
        {
            var rnx = new Random();
            return rnx.Next(min, max);
        }

        private static BigInteger GenKey(BigInteger g, BigInteger ix, BigInteger n)
        {
            return BigInteger.Pow(g, (int)ix) % n;
        }

        private static BigInteger GenK(BigInteger basis, BigInteger pow, BigInteger n)
        {
            return BigInteger.Pow(basis, (int)pow) % n;
        }
    }
}
