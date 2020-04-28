using Demo.StoreExample.RulesPattern.Refactor;

namespace Demo.StoreExample.RulesPattern.Rules
{
    public class NewCustomerRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return !customer.IsExisting() ? 0.15m : 0;
        }
    }
}
