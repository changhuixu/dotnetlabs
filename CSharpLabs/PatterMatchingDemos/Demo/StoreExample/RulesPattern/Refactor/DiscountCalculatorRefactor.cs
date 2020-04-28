using System;

namespace Demo.StoreExample.RulesPattern.Refactor
{
    public class DiscountCalculatorRefactor : IDiscountCalculator
    {
        public decimal CalculateDiscountPercentage(Customer customer)
        {
            decimal discount = 0;
            if (customer.IsSenior())
            {
                // senior discount 5%
                discount = .05m;
            }

            if (customer.IsBirthday())
            {
                discount = Math.Max(discount, 0.1m);
            }

            if (customer.IsExisting())
            {
                if (customer.HasBeenLoyalForYears(1))
                {
                    // after 1 year, loyal customers get 10%
                    discount = Math.Max(discount, .10m);
                    if (customer.HasBeenLoyalForYears(5))
                    {
                        // after 5 years, 12%
                        discount = Math.Max(discount, .12m);
                        if (customer.HasBeenLoyalForYears(10))
                        {
                            // after 10 years, 20%
                            discount = Math.Max(discount, .2m);
                        }
                    }
                    if (customer.IsBirthday())
                    {
                        discount += 0.1m;
                    }

                }
            }
            else
            {
                // first time purchase discount of 15%
                discount = Math.Max(discount, .15m);
            }
            if (customer.IsVeteran)
            {
                // veterans get 10%
                discount = Math.Max(discount, .10m);
            }

            return discount;
        }
    }
}
