using System;
using System.Threading;

namespace LockStatementLab
{
    public class Account
    {
        private readonly object _locker = new object();
        public int Id { get; protected set; }
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
