using System;

namespace assignment9
{
    /// <summary>
    /// Fibonacci class for calculating Fibonacci numbers.
    /// </summary>
    internal static class Fibonacci
    {
        /// <summary>
        /// Calculates n-th Fibonacci number.
        /// </summary>
        /// <param name="number">n</param>
        /// <returns>n-th Fibonacci number</returns>
        public static int Calculate(int number)
        {
            if (number <= 1)
            {
                return number;
            }

            return Calculate(number - 1) + Calculate(number - 2);
        }

        /// <summary>
        /// Run the Fibonacci class with given parametrs.
        /// </summary>
        /// <param name="count">How many numbers to count.</param>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        public static void Run(int count, int min, int max)
        {
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                ThreadPool.Run(() => Console.WriteLine(Fibonacci.Calculate(random.Next(min, max))));
            }

            ThreadPool.WaitToAll();
        }
    }
}
