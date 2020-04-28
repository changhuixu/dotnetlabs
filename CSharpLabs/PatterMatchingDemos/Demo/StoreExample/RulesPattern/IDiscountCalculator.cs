namespace Demo.StoreExample.RulesPattern
{
    public interface IDiscountCalculator
    {
        decimal CalculateDiscountPercentage(Customer customer);
    }
}
