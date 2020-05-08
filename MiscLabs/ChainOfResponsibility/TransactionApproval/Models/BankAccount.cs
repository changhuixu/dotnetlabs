namespace TransactionApproval.Models
{
    public class BankAccount
    {
        public bool IdOnRecord { get; set; }

        public void WithdrawMoney(BankEmployee frontLineStaff, decimal amount)
        {
            frontLineStaff.HandleWithdrawRequest(this, amount);
        }
    }
}
