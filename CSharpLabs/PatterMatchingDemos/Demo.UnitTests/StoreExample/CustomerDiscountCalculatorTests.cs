using System;
using Demo.StoreExample;
using Demo.StoreExample.RulesPattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.UnitTests.StoreExample
{
    [TestClass]
    public class CustomerDiscountCalculatorTests
    {

        [TestMethod]
        public void Return15PctForNewCustomer()
        {
            var customer = new Customer();
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.15m, discount);
        }

        [TestMethod]
        public void Return10PctForVeteran()
        {
            var customer = new Customer
            {
                IsVeteran = true,
                DateOfFirstPurchase = DateTime.Today.AddDays(-1)
            };
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.1m, discount);
        }

        [TestMethod]
        public void Return5PctForSenior()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today.AddYears(-65).AddDays(-5),
                DateOfFirstPurchase = DateTime.Today.AddDays(-1)
            };
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.05m, discount);
        }

        [TestMethod]
        public void Return10PctForBirthday()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                DateOfFirstPurchase = DateTime.Today.AddDays(-1)
            };
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.10m, discount);
        }

        [TestMethod]
        public void Return12PctFor5YearLoyalCustomer()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today.AddDays(-5),
                DateOfFirstPurchase = DateTime.Today.AddYears(-5)
            };
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.12m, discount);
        }

        [TestMethod]
        public void Return22PctFor5YearLoyalCustomerOnBirthday()
        {
            var customer = new Customer
            {
                DateOfBirth = DateTime.Today,
                DateOfFirstPurchase = DateTime.Today.AddYears(-5)
            };
            var calculator = new CustomerDiscountCalculator(customer, DateTime.Now);
            var discount = calculator.CalculateDiscountPercentage();

            Assert.AreEqual(0.22m, discount);
        }
    }
}
