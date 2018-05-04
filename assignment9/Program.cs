using System;

namespace assignment9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Threads.Count);

            Fibonacci.Run(10, 25, 45);

            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
        }        
    }
}
