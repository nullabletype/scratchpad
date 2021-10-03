using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DispatchProxyExample
{
    class Program
    {
        static void Main(string[] args)
        {
            IHello hello = new Hello();
            IHello helloProxy = HelloDispatchProxy<IHello>.CreateProxy(new Hello());

            int iterations = 100000;
            int groupIterations = 20;

            IEnumerable<((TimeSpan Standard, TimeSpan Proxied), double PercentageIncrease)> resultSet = RunGrouptest(hello, helloProxy, iterations, groupIterations);

            string csv = "";

            foreach (var result in resultSet)
            {
                Console.WriteLine($"Without: {result.Item1.Standard.TotalMilliseconds}ms | With: {result.Item1.Proxied.TotalMilliseconds}ms | {result.PercentageIncrease}%");
                csv += $"{result.Item1.Standard.TotalMilliseconds},{result.Item1.Proxied.TotalMilliseconds},{result.PercentageIncrease / 100}{Environment.NewLine}";
            }

            System.IO.File.WriteAllText("results.csv", csv);
        }

        private static IEnumerable<((TimeSpan Standard, TimeSpan Proxied) TimeSet, double PercentageIncrease)> RunGrouptest(IHello standard, IHello proxied, int iterations, int groupIterations)
        {
            for (int i = 0; i <= groupIterations; i++)
            {
                TimeSpan withoutProxy = RunTest(standard, iterations);
                TimeSpan withProxy = RunTest(proxied, iterations);

                double percentage = ((withProxy.TotalMilliseconds - withoutProxy.TotalMilliseconds) / withoutProxy.TotalMilliseconds) * 100;

                yield return ((withoutProxy, withProxy), percentage);
            }
        }

        private static TimeSpan RunTest(IHello hello, int iterations)
        {
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i <= iterations; i++)
            {
                hello.SayHello(GenerateRandomString(10));
            }
            watch.Stop();
            return watch.Elapsed;
        }

        /// <summary>
        /// Taken from https://stackoverflow.com/questions/14687658/random-name-generator-in-c-sharp
        /// </summary>
        /// <param name="len">Length of random string</param>
        /// <returns></returns>
        public static string GenerateRandomString(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string randomString = "";
            randomString += consonants[r.Next(consonants.Length)].ToUpper();
            randomString += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                randomString += consonants[r.Next(consonants.Length)];
                b++;
                randomString += vowels[r.Next(vowels.Length)];
                b++;
            }

            return randomString;
        }
    }
}
