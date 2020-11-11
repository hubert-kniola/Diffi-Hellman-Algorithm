using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Calculations
{
    public class Calculate
    {
        #region Methodes
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

        /// <summary>
        /// Metoda szukająca pierwiastka pierwotnego
        /// </summary>
        /// <param name="n">Liczba pierwsza</param>
        /// <returns></returns>
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
                    if (Power(r, phi / a, n) == 1)
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

        /// <summary>
        /// Metoda sprawdzająca parzystość liczby
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static bool IsOdd(int number)
        {
            if (number % 2 != 0)
                return true;
            else
                return false;
        }

        private static List<BigInteger> PrimitiveRoots(BigInteger g)
        {
            var coprimes = new List<BigInteger>();

            for (int i = 1; i < g; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, g) == 1)
                {
                    coprimes.Add(i);
                }
            }

            var primitiveRoots = new List<BigInteger>();
            for (int j = 1; j < g; j++)
            {
                var resultSet = new List<BigInteger>();

                for (int k = 1; k < g; k++)
                {
                    resultSet.Add(BigInteger.Pow(j, k) % g);
                }

                if (coprimes.TrueForAll(resultSet.Contains))
                {
                    primitiveRoots.Add(j);
                }
            }

            return primitiveRoots;
        }

        /// <summary>
        /// Metoda sprawdzająca czy podana liczba jest pierwiastkiem pierwotnym
        /// </summary>
        /// <param name="g">Pierwiastek pierwotny</param>
        /// <param name="n">Liczba pierwsza</param>
        /// <returns></returns>
        private static bool IsPrimitiveRoot(BigInteger g, BigInteger n)
        {
            var coprimes = new List<BigInteger>();

            for (int i = 0; i < g; i++)
            {
                if (BigInteger.GreatestCommonDivisor(i, g) == 1)
                {
                    coprimes.Add(i);
                }
            }

            var resultSet = new List<BigInteger>();

            for (int i = 0; i < g; i++)
            {
                resultSet.Add(BigInteger.Pow(n, i) % g);
            }

            return coprimes.TrueForAll(resultSet.Contains);
        }

        /// <summary>
        /// Metoda sprawdzająca czy liczba jest pierwsza
        /// </summary>
        /// <param name="n">Liczba pierwsza</param>
        /// <returns></returns>
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
        #endregion

        #region NumberGenerators
        /// <summary>
        /// Metoda sprawdzająca czy wylosowana liczba jest liczbą pierwszą
        /// </summary>
        /// <returns></returns>
        private static BigInteger GeneratePrime()
        {
            BigInteger x;
            do
                x = GenerateXY(100, 10000000);
            while (!CheckPrime(x));
            return x;
        }

        /// <summary>
        /// Metoda generująca wartości z podanego zakresu
        /// </summary>
        /// <param name="min">Wartość minimalna</param>
        /// <param name="max">Wartośc maksymalna</param>
        /// <returns></returns>
        private static BigInteger GenerateXY(int min, int max)
        {
            Thread.Sleep(100);
            return new Random().Next(min, max);
        }

        /// <summary>
        /// Metoda wyliczająca wartość z odpowiedniego wzoru
        /// </summary>
        /// <param name="g">Pierwiastek pierwotny</param>
        /// <param name="ix">Wartość małego x</param>
        /// <param name="n">Liczba pierwsza</param>
        /// <returns></returns>
        private static BigInteger GenKey(BigInteger g, BigInteger ix, BigInteger n)
        {
            return (BigInteger.Pow(g, (int)ix) % n);
        }
        #endregion

        #region MainFunction
        /// <summary>
        /// Funkcja główna programu wyliczająca wartośc klucza dla 2 osób
        /// </summary>
        public static void difiAlgorithm()
        {
            Stopwatch sw = Stopwatch.StartNew();
            BigInteger n = 0;
            BigInteger g = 0;
            TimeSpan timeN, timeG;

            #region NG_Generate
            sw.Start();
            n = GeneratePrime();
            sw.Stop();
            timeN = sw.Elapsed;
            sw.Start();
            g = FindPrimitive(n);
            sw.Stop();
            timeG = sw.Elapsed;

            var yesOrNo = IsPrimitiveRoot(g, n);
            Console.WriteLine($"=== 2 USERS ===\nGenerated G is primitive root: {yesOrNo}");
            if (yesOrNo == false) return;
            Console.WriteLine($"Value of n: {n} | Generation time [ms]: {timeN}");
            Console.WriteLine($"Value of g: {g} | Generation time [ms]: {timeG}");
            #endregion

            #region LowercaseXY_Generate           
            sw.Start();
            var ix = GenerateXY(10, 10000); //male x
            sw.Stop();
            Console.WriteLine($"Value of x: {ix} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var iy = GenerateXY(10, 10000); //male y
            sw.Stop();
            Console.WriteLine($"Value of y: {iy} | Generation time [ms]: {sw.Elapsed}");
            #endregion

            #region CapitalXY_Generate
            BigInteger x, y;
            sw.Start();
            x = GenKey(g, ix, n); //duze X
            sw.Stop();
            Console.WriteLine($"Private Key: {x} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            y = GenKey(g, iy, n); //duze Y
            sw.Stop();
            Console.WriteLine($"Public Key: {y} | Generation time [ms]: {sw.Elapsed}");
            #endregion

            #region FinalCalculation
            var kA = GenKey(y, ix, n);
            var kB = GenKey(x, iy, n);
            Console.WriteLine($"kA: {kA}\nkB: {kB}");
            #endregion
        }
        #endregion

        /// <summary>
        /// Funkcja główna wyliczająca wartośc klucza dla grupy 4 osób
        /// </summary>
        public static void difiForGroup()
        {
            Stopwatch sw = Stopwatch.StartNew();
            BigInteger n, g;
            TimeSpan timeN, timeG;

            #region NG_Generate
            sw.Start();
            n = GeneratePrime();
            sw.Stop();
            timeN = sw.Elapsed;
            sw.Start();
            g = FindPrimitive(n);
            sw.Stop();
            timeG = sw.Elapsed;

            var yesOrNo = IsPrimitiveRoot(g, n);
            Console.WriteLine($"=== 5 USERS ===\nGenerated G is primitive root: {yesOrNo}");
            if (yesOrNo == false) return;
            Console.WriteLine($"Value of n: {n} | Generation time [ms]: {timeN}");
            Console.WriteLine($"Value of g: {g} | Generation time [ms]: {timeG}");
            #endregion

            #region LowercaseXYZW_Generate        
            Console.WriteLine("===========================================");
            sw.Start();
            var ix = GenerateXY(10, 10000); //male x
            sw.Stop();
            Console.WriteLine($"Value of x: {ix} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var iy = GenerateXY(10, 10000); //male y
            sw.Stop();
            Console.WriteLine($"Value of y: {iy} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var iz = GenerateXY(10, 10000); //male z
            sw.Stop();
            Console.WriteLine($"Value of z: {iz} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var iw = GenerateXY(10, 10000); //male w
            sw.Stop();
            Console.WriteLine($"Value of w: {iw} | Generation time [ms]: {sw.Elapsed}");
            Console.WriteLine("===========================================");
            #endregion
    
            #region CapitalXYZW_Generate
            BigInteger x, y, z, w, q, k, l, m;
            sw.Start();
            x = GenKey(g, ix, n); //duze X
            sw.Stop();
            Console.WriteLine($"Private Key: {x} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            y = GenKey(g, iy, n); //duze Y
            sw.Stop();
            Console.WriteLine($"Public Key: {y} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            z = GenKey(g, iz, n); //duze Z
            sw.Stop();
            Console.WriteLine($"Public Key: {z} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            w = GenKey(g, iw, n); //duze W
            sw.Stop();
            Console.WriteLine($"Public Key: {w} | Generation time [ms]: {sw.Elapsed}");
            Console.WriteLine("===========================================");
            #endregion

            #region FinalCalculation
            var kA = GenKey(y, ix, n);
            var kB = GenKey(x, iy, n);
            var kC = GenKey(z, iw, n);
            var kD = GenKey(w, iz, n);

            Console.WriteLine($"kA: {kA}\nkB: {kB}\nkC: {kC}\nkD: {kD}");

            ix = kB / 6; //obliczanie nowych wartości małej x z otrzymanej wartości klucza
            iy = kD / 6; //obliczanie nowych wartości małej y z otrzymanej wartości klucza
            var newX = GenKey(g, ix, n); //generowanie nowego duzego X
            var newY = GenKey(g, iy, n); //generowanie nowego duzego Y

            Console.WriteLine($"newX: {newX}\nnewY: {newY}");

            sw.Start();
            var kOne = GenKey(newY, ix, n);
            sw.Stop();
            Console.WriteLine($"kOne: {kOne} | Generation time [ms]: {sw.Elapsed}");
            sw.Start();
            var kTwo = GenKey(newX, iy, n);
            sw.Stop();
            Console.WriteLine($"kTwo: {kTwo} | Generation time [ms]: {sw.Elapsed}");
            #endregion
        }

    }
}
