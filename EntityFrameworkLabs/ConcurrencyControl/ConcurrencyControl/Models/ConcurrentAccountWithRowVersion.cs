namespace ConcurrencyControl.Models
{
    public class ConcurrentAccountWithRowVersion : BankAccount
    {
        public byte[] Timestamp { get; set; }
    }
}
