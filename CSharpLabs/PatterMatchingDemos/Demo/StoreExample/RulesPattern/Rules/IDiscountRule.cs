using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.StoreExample.RulesPattern.Rules
{
    public interface IDiscountRule
    {
        decimal CalculateCustomerDiscount(Customer customer);
    }
}
