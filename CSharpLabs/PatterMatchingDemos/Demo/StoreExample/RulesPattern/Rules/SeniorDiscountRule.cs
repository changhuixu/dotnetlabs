using Demo.StoreExample.RulesPattern.Refactor;

namespace Demo.StoreExample.RulesPattern.Rules
{
    public class SeniorDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            var a = customer.IsSenior();
            return customer.IsSenior() ? 0.05m : 0;
        }
    }
}
