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
            n = generateNumber(1000);
            Console.WriteLine(n);
            sw.Stop();
            timeN = sw.Elapsed;
            sw.Start();
            listG = primitiveRoots(n);
            g = listG.First();
            sw.Stop();
            timeG = sw.Elapsed;

            Console.WriteLine($"Primitive root: {isPrimitiveRoot(g, n)}");
            Console.WriteLine($"n: {n}, generating [ms]: {timeN}");
            Console.WriteLine($"g: {g}, generating [ms]: {timeG}");
            BigInteger x = 0;
            BigInteger y = 0;
            sw.Start();
            var ix = generateXY(10, 100);//male x
            sw.Stop();
            Console.WriteLine($"x generating [ms]: {sw.Elapsed}");
            sw.Start();
            var iy = generateXY(10, 100); //male y
            sw.Stop();
            Console.WriteLine($"y generating [ms]: {sw.Elapsed}");

            sw.Start();
            x = genPrivateKey(n, ix, g); //duze X
            sw.Stop();
            Console.WriteLine($"Private Key: {x}, generating [ms]: {sw.Elapsed}");
            sw.Start();
            y = genPublicKey(n, iy, g); //duze Y
            sw.Stop();
            Console.WriteLine($"Public Key: {y}, generating [ms]: {sw.Elapsed}");


            var kA = calculateK(y, ix, n);
            var kB = calculateK(x, iy, n);
            Console.WriteLine($"kA: {kA}, kB: {kB}");
        }

        private static BigInteger generateNumber(BigInteger N)
        {
            Random rnx = new Random();
            BigInteger sd = 0;
            do
                sd = rnx.Next(100, 10000);
            while (BigInteger.GreatestCommonDivisor(sd, N) != 1);
            return sd;
        }

        private static List<BigInteger> primitiveRoots(BigInteger modulo)
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

                for (int i = 1; i < modulo; i++)
                {
                    resultSet.Add(i % modulo);
                }

                if (coprimes.TrueForAll(resultSet.Contains))
                {
                    primitiveRoots.Add(j);
                }
            }

            return primitiveRoots;
        }

        private static bool isPrimitiveRoot(BigInteger modulo, BigInteger arg)
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

        private static BigInteger generateXY(int min, int max)
        {
            var rnx = new Random();
            return rnx.Next(min, max);
        }

        private static BigInteger genPrivateKey(BigInteger n, BigInteger ix, BigInteger g)
        {
            return BigInteger.Pow(g, (int)ix) % n;
        }

        private static BigInteger genPublicKey(BigInteger n, BigInteger iy, BigInteger g)
        {
            return BigInteger.Pow(g, (int)iy) % n;
        }

        private static BigInteger calculateK(BigInteger basis, BigInteger pow, BigInteger n)
        {
            return BigInteger.Pow(basis, (int)pow) % n;
        }

        /*private static BigInteger CheckInteger()
        {
            BigInteger x = 0;
            string readString = Console.ReadLine();
            if (BigInteger.TryParse(readString, out x))
                return x;
            else
                return 0;
        }*/
    }
}
