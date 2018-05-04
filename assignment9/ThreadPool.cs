using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace assignment9
{
    static class ThreadPool
    {
        private static object notConsumer;

        private static int maxThreadsCount;

        private static int workingThreadsCount;

        private static Queue<Action> actions;

        private static LinkedList<Thread> workingThreads;

        //static Queue<Thread> sleepingThreads;

        static ThreadPool()
        {
            notConsumer = 4;
            maxThreadsCount = 10;
            workingThreadsCount = 0;
            actions = new Queue<Action>();
            workingThreads = new LinkedList<Thread>();
        }

        public static void Run(Action action)
        {
            lock(actions)
            {
                actions.Enqueue(action);

            }


            if (workingThreads.Count < maxThreadsCount)
            {
                

                workingThreads.AddLast(new Thread(() =>
                {
 
                    while (true)
                    {

                        Action a = null;
                        lock (actions)
                        {
                            if (actions.Count > 0)
                            {
                                a = actions.Dequeue();
                            }
                        }

                        if (a != null)
                        {
                            a.Invoke();
                        }

                        lock (actions)
                        {
                            if (actions.Count > 0)
                            {
                                continue;
                            }
                        }

                        lock (notConsumer)
                            Monitor.Wait(notConsumer);

                    }

                }));

                workingThreads.Last.Value.IsBackground = true;
                workingThreads.Last.Value.Start();
            }
            else
            {
                lock (notConsumer)
                    Monitor.PulseAll(notConsumer);
            }
        }
    }
}

