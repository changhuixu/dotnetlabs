using Demo.StoreExample.RulesPattern.Refactor;

namespace Demo.StoreExample.RulesPattern.Rules
{
    public class BirthdayDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return customer.IsBirthday() ? 0.10m : 0;
        }
    }
}
