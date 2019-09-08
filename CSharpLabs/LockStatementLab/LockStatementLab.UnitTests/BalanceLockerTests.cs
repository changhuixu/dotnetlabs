using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.ClassLevel)]

namespace LockStatementLab.UnitTests
{
    [TestClass]
    //[DoNotParallelize]
    public class BalanceLockerTests
    {
        [TestMethod]
        public void TestLockerUsingTasks()
        {
            var account = new Account(1000);
            var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(() => account.Credit(10))).ToArray();
            Task.WaitAll(tasks);

            Assert.AreEqual(2000, account.Balance);
        }

        [TestMethod]
        [DoNotParallelize]
        public void TestLockerUsingThreads()
        {
            //ThreadPool.SetMinThreads(30, 30);
            //ThreadPool.GetMinThreads(out var minWorker, out var minIOC);
            //Console.WriteLine($"Number of Processors: {Environment.ProcessorCount}; Min Worker Threads: {minWorker}; Min Asynchronous I/O Completion Threads: {minIOC}.");

            var account = new Account(1000);
            Enumerable.Range(0, 100).Select(i =>
            {
                var thread = new Thread(() => account.Credit(10));
                thread.Start();
                return thread;
            }).ToList().ForEach(t => t.Join());

            Assert.AreEqual(2000, account.Balance);
        }

        [TestMethod]
        public async Task TestDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase("TestMethod4").Options;
            using (var dbContext = new MyDbContext(options))
            {
                await dbContext.Accounts.AddAsync(new Account(1000));
                await dbContext.SaveChangesAsync();

                var tasks = new Task[1000];
                for (var i = 0; i < tasks.Length; i++)
                {
                    var account = await dbContext.Accounts.FirstAsync();
                    tasks[i] = Task.Run(() => account.Credit(1));
                }
                Task.WaitAll(tasks);

                await dbContext.SaveChangesAsync();
                Assert.AreEqual(2000, dbContext.Accounts.First().Balance);
            }
        }

        [TestMethod]
        public void TestObjectInList()
        {
            var accounts = new List<Account> { new Account(1000) };

            var tasks = new Task[10];
            for (var i = 0; i < tasks.Length; i++)
            {
                var account = accounts.First();
                tasks[i] = Task.Run(() => account.Credit(100));
            }
            Task.WaitAll(tasks);

            Assert.AreEqual(2000, accounts.First().Balance);  // works even without locker... Why?
        }


        [TestMethod]
        public void TestObjectInList2()
        {
            var accounts = new List<Account> { new Account(1000) };

            var tasks = new Task[10];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var account = accounts.First();
                    account.Credit(100);
                });
            }
            Task.WaitAll(tasks);

            Assert.AreEqual(2000, accounts.First().Balance);  // works even without locker... Why?
        }

        [TestMethod]
        public void TestObjectInList3()
        {
            var accounts = new List<Account> { new Account(1000) };

            var threads = new Thread[10];
            for (var i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    var account = accounts.First();
                    account.Credit(100);
                });
            }
            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);     // The purpose of this line is merely to display Console output in sequence.

            Assert.AreEqual(2000, accounts.First().Balance);
        }

        [TestMethod]
        public void TestLockerInDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("TestMethod6").Options;
            using (var dbContext = new MyDbContext(options))
            {
                dbContext.Accounts.Add(new Account(1000));
                dbContext.SaveChanges();
            }

            var tasks = new Task[10];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    using (var dbContext = new MyDbContext(options))
                    {
                        var account = dbContext.Accounts.First();
                        account.Credit(100);
                        dbContext.SaveChanges();
                    }
                });
            }
            Task.WaitAll(tasks);

            using (var dbContext = new MyDbContext(options))
            {
                Assert.AreEqual(1, dbContext.Accounts.Count());
                var finalBalance = dbContext.Accounts.First().Balance;
                Console.WriteLine($"<{Thread.CurrentThread.ManagedThreadId,2}> Final Balance: {finalBalance}");
                Assert.IsTrue(finalBalance > 1000);
                Assert.IsTrue(finalBalance < 2000);
            }
        }
    }
}
