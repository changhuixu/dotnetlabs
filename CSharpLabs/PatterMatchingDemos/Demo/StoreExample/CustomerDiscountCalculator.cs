using System;
using System.Linq;
using Demo.StoreExample.RulesPattern;

namespace Demo.StoreExample
{
    public enum LoyalYear
    {
        None = -1,
        Zero = 0,
        One = 1,
        Five = 5,
        Ten = 10
    }

    public class CustomerDiscountCalculator
    {
        public bool IsBirthday { get; protected set; }
        public bool IsSenior { get; protected set; }
        public bool IsVeteran { get; protected set; }
        public LoyalYear LoyalYear { get; protected set; }

        public CustomerDiscountCalculator(Customer customer, DateTime now)
        {
            IsBirthday = customer.DateOfBirth > DateTime.MinValue && customer.DateOfBirth.Day == now.Day && customer.DateOfBirth.Month == now.Month;
            IsSenior = customer.DateOfBirth > DateTime.MinValue && customer.DateOfBirth < now.AddYears(-65);
            IsVeteran = customer.IsVeteran;
            LoyalYear = GetLoyalYear(customer.DateOfFirstPurchase, now);
        }

        private static LoyalYear GetLoyalYear(DateTime? dateOfFirstPurchase, DateTime now)
        {
            if (!dateOfFirstPurchase.HasValue)
            {
                return LoyalYear.None;
            }

            var purchaseYears = now.Year - dateOfFirstPurchase.Value.Year;
            if (now.AddYears(-purchaseYears) < dateOfFirstPurchase.Value) purchaseYears--;
            var loyalYear = Enum.GetValues(typeof(LoyalYear)).Cast<int>()
                .OrderByDescending(x => x)
                .First(x => x <= purchaseYears);
            return (LoyalYear)loyalYear;
        }

        public decimal CalculateDiscountPercentage()
        {
            return (LoyalYear, IsSenior, IsBirthday, IsVeteran) switch
            {
                (LoyalYear.None, _, _, _) => 0.15m,
                (LoyalYear.One, _, _, _) => 0.1m + (IsBirthday ? 0.1m : 0m),
                (LoyalYear.Five, _, _, _) => 0.12m + (IsBirthday ? 0.1m : 0m),
                (LoyalYear.Ten, _, _, _) => 0.2m + (IsBirthday ? 0.1m : 0m),
                (_, _, true, _) => 0.1m,
                (_, _, _, true) => 0.1m,
                (_, true, _, _) => 0.05m,
                _ => 0m
            };
        }
    }
}
