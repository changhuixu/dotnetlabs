using System;

namespace TransactionApproval.Models
{
    public abstract class BankEmployee
    {
        public string Name { get; }
        public BankEmployee NextUpLevel { get; private set; }

        protected BankEmployee(string name)
        {
            Name = name;
        }

        public void SetNextUpLevel(BankEmployee nextUpLevel)
        {
            NextUpLevel = nextUpLevel;
        }

        public void HandleWithdrawRequest(BankAccount account, decimal amount)
        {
            if (CanHandleRequest(account, amount))
            {
                Withdraw(account, amount);
            }
            else
            {
                if (NextUpLevel == null)
                {
                    Console.WriteLine($"Not able to handle this withdraw, because {Name}'s next upper level is not set.");
                    return;
                }
                NextUpLevel.HandleWithdrawRequest(account, amount);
            }
        }

        protected abstract bool CanHandleRequest(BankAccount account, decimal amount);

        protected abstract void Withdraw(BankAccount account, decimal amount);
    }

    public class Teller : BankEmployee
    {
        public Teller(string name) : base(name)
        {
        }

        protected override bool CanHandleRequest(BankAccount account, decimal amount)
        {
            return amount <= 10000;
        }

        protected override void Withdraw(BankAccount account, decimal amount)
        {
            Console.WriteLine("Amount withdrawn by Teller");
        }
    }

    public class Supervisor : BankEmployee
    {
        public Supervisor(string name) : base(name)
        {
        }

        protected override bool CanHandleRequest(BankAccount account, decimal amount)
        {
            return amount <= 100000;
        }

        protected override void Withdraw(BankAccount account, decimal amount)
        {
            if (!account.IdOnRecord)
            {
                Console.WriteLine("Account holder does not have ID on record. Not able to proceed.");
                return;
            }

            Console.WriteLine("Amount withdrawn by Supervisor");
        }
    }

    public class BankManager : BankEmployee
    {
        public BankManager(string name) : base(name)
        {
        }

        protected override bool CanHandleRequest(BankAccount account, decimal amount)
        {
            return true;
        }

        protected override void Withdraw(BankAccount account, decimal amount)
        {
            Console.WriteLine("Amount withdrawn by Bank Manager");
        }
    }
}
