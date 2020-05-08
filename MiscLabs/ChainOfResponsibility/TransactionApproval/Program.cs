using TransactionApproval.Models;

namespace TransactionApproval
{
    internal class Program
    {
        private static void Main()
        {
            var bankManager = new BankManager("Bob");
            var bankSupervisor = new Supervisor("Smith");
            bankSupervisor.SetNextUpLevel(bankManager);
            var frontLineStaff = new Teller("Taylor");
            frontLineStaff.SetNextUpLevel(bankSupervisor);

            var account = new BankAccount();
            account.WithdrawMoney(frontLineStaff, 5000);
            account.WithdrawMoney(frontLineStaff, 50000);
            account.WithdrawMoney(frontLineStaff, 500000);

            var teller2 = new Teller("Tim");
            account.WithdrawMoney(teller2, 50000);
        }
    }
}
