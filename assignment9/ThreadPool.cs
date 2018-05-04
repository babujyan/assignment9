using System;
using System.Collections.Generic;
using System.Threading;

namespace assignment9
{
    /// <summary>
    /// Simple thread pool.
    /// </summary>
    static class ThreadPool
    {

        /// <summary>
        /// Notifies when task is created.
        /// </summary>
        private static object notConsumer;

        /// <summary>
        /// Notifies when thread is waiting.
        /// </summary>
        private static object not;

        /// <summary>
        /// For knowing which threads are working.If it's 0 all threads are not working.
        /// </summary>
        private static int isWorking;

        /// <summary>
        /// Maximum thread count.
        /// </summary>
        private static int maxThreadsCount;

        /// <summary>
        /// Queue for tasks.
        /// </summary>
        private static Queue<Action> actions;

        /// <summary>
        /// Working threads.
        /// </summary>
        private static List<Thread> workingThreads;

        /// <summary>
        /// Static constructor for initializing parametrs.
        /// </summary>
        static ThreadPool()
        {
            //no thread is working.
            isWorking = 0x00;

            //doesn't matter.
            not = 4;
            notConsumer = 4;

            //maximum 10 threads.
            maxThreadsCount = 10;

            
            actions = new Queue<Action>();
            workingThreads = new List<Thread>();
        }

        /// <summary>
        /// Wait to all threads to finish thasks.
        /// </summary>
        public static void WaitToAll()
        {
            while(true)
            {
                lock ((object)isWorking)
                {
                    if(isWorking == 0)
                    {
                        return;
                    }
                }

                lock (not)
                {
                    Monitor.Wait(not);
                }
            }
        }
        
        /// <summary>
        /// Run a thask.
        /// </summary>
        /// <param name="action"> Delegate for thask.</param>
        public static void Run(Action action)
        {
            lock(actions)
            {
                actions.Enqueue(action);
            }
            
            if (workingThreads.Count < maxThreadsCount)
            {
                int index = workingThreads.Count;

                workingThreads.Add(new Thread(() =>
                {
                    int threadIndex;

                    lock ((object)index)
                    {
                        threadIndex = index;
                    }

                    while (true)
                    {
                        lock((object)isWorking)
                        {
                            isWorking |= (1 << threadIndex);
                        }

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
                                                
                        lock (not)
                        {
                            lock ((object)isWorking)
                            {
                                isWorking &= ~(1 << threadIndex);
                            }

                            Monitor.PulseAll(not);
                        }

                        lock (actions)
                        {
                            if (actions.Count > 0)
                            {
                                continue;
                            }
                        }
                        
                        lock (notConsumer)
                        {
                            Monitor.Wait(notConsumer);
                        }

                    }

                }));
                
                workingThreads[index].IsBackground = true;
                workingThreads[index].Start();
            }
            else
            {
                lock (notConsumer)
                {
                    Monitor.PulseAll(notConsumer);
                }
            }
        }
    }
}

