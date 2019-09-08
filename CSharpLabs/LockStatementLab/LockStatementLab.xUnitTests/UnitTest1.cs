using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LockStatementLab.xUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestLockerUsingTasks()
        {
            ThreadPool.SetMinThreads(30, 30);
            var account = new Account(1000);
            var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(() => account.Credit(10))).ToArray();
            Task.WaitAll(tasks);

            Assert.NotEqual(2000, account.Balance);
        }

        [Fact]
        public void TestLockerUsingThreads()
        {
            var account = new Account(1000);
            Enumerable.Range(0, 100).Select(i =>
            {
                var thread = new Thread(() => account.Credit(10));
                thread.Start();
                return thread;
            }).ToList().ForEach(t => t.Join());

            Assert.NotEqual(2000, account.Balance);
        }
    }

    public class Account
    {
        private readonly object _locker = new object();
        public decimal Balance { get; private set; }

        protected Account() { }

        public Account(decimal initialBalance) : this()
        {
            Balance = initialBalance;
        }

        public void Credit(decimal amount)
        {
            lock (_locker)
            {
                Console.WriteLine($"<{Thread.CurrentThread.ManagedThreadId,2}> Balance Before Credit: {Balance,5}");
                Console.WriteLine($"<{Thread.CurrentThread.ManagedThreadId,2}>         Amount to Add: {amount,5}");
                Balance += amount;
                Console.WriteLine($"<{Thread.CurrentThread.ManagedThreadId,2}> Balance After  Credit: {Balance,5}");
            }
        }
    }
}
