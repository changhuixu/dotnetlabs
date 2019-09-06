using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimThrottle
{
    class Program
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);

        static void Main(string[] args)
        {

            for (int t = 0; t < 4; t++)
            {
                semaphoreSlim.Wait();
                Task.Factory.StartNew((index) =>
                {
                    //Simulate processing  
                    Console.WriteLine($"Running task: {index}");
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                }, t).ContinueWith(task => { semaphoreSlim.Release(); });
            }

            for (int t = 0; t < 4; t++)
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    tokenSource.CancelAfter(TimeSpan.FromSeconds(3));
                    try
                    {
                        semaphoreSlim.Wait(tokenSource.Token);

                        Task.Factory.StartNew((index) =>
                        {
                            //Simulate processing  
                            Console.WriteLine($"Running task: {index}");
                            Thread.Sleep(TimeSpan.FromSeconds(5));

                        }, t, tokenSource.Token).ContinueWith(task => { semaphoreSlim.Release(); }, tokenSource.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        Console.WriteLine("Wait time expired before semaphore released");
                    }
                }

            }

            for (int t = 0; t < 4; t++)
            {
                if (semaphoreSlim.Wait(TimeSpan.FromSeconds(3)))
                {
                    Task.Factory.StartNew((index) =>
                    {
                        //Simulate processing  
                        Console.WriteLine($"Running task: {index}");
                        Thread.Sleep(TimeSpan.FromSeconds(5));

                    }, t).ContinueWith(task => { semaphoreSlim.Release(); });
                }
                else
                {
                    Console.WriteLine("Wait time expired before semaphore released");
                }
            }


            //var allTasks = new List<Task>();
            //var throttler = new SemaphoreSlim(initialCount: maxThreads);
            //foreach (var url in urls)
            //{
            //    await throttler.WaitAsync();
            //    allTasks.Add(
            //        Task.Run(async () =>
            //        {
            //            try
            //            {
            //                var html = await client.GetStringAsync(url);
            //                Console.WriteLine($"retrieved {html.Length} characters from {url}");
            //            }
            //            finally
            //            {
            //                throttler.Release();
            //            }
            //        }));
            //}
            //await Task.WhenAll(allTasks);
            Console.ReadLine();
        }

        public static async Task ForEachAsyncSemaphore<T>(IEnumerable<T> source,
            int degreeOfParallelism, Func<T, Task> body)
        {
            var tasks = new List<Task>();
            using (var throttler = new SemaphoreSlim(degreeOfParallelism))
            {
                foreach (var element in source)
                {
                    await throttler.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            await body(element);
                        }
                        finally
                        {
                            throttler?.Release();
                        }
                    }));
                }
                await Task.WhenAll(tasks);
            }
        }
    }
}
