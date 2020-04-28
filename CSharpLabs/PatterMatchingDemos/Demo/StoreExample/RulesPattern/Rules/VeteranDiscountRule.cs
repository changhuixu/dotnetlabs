namespace Demo.StoreExample.RulesPattern.Rules
{
    public class VeteranDiscountRule : IDiscountRule
    {
        public decimal CalculateCustomerDiscount(Customer customer)
        {
            return customer.IsVeteran ? 0.10m : 0;
        }
    }
}
