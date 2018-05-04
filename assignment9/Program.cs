using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace assignment9
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            //Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
            for (int i = 0; i < 1000; i++)
                lock ((object)i)
                {
                    var j = i;
                    ThreadPool.Run(() =>
                    {


                      {
                            Thread.Sleep(random.Next(0,1000));
                            Console.WriteLine(j);
                      }
                    //

                    });
                }

            while (true) ;
            //Thread.Sleep(5000);
                //Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Threads.Count);
        }
    }
}
