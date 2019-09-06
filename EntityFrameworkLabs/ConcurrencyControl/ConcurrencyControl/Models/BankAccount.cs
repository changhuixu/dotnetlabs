using System;

namespace ConcurrencyControl.Models
{
    public abstract class BankAccount
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }

        public void Credit(decimal amount)
        {
            Console.WriteLine($"Balance before credit:{Balance,5}");
            Console.WriteLine($"Amount to add        :{amount,5}");
            Balance += amount;
            Console.WriteLine($"Balance after credit :{Balance,5}");
        }

        public decimal Debit(decimal amount)
        {
            if (Balance >= amount)
            {
                Console.WriteLine($"Balance before debit :{Balance,5}");
                Console.WriteLine($"Amount to remove     :{amount,5}");
                Balance -= amount;
                Console.WriteLine($"Balance after debit  :{Balance,5}");
                return amount;
            }
            return 0;
        }
    }
}
