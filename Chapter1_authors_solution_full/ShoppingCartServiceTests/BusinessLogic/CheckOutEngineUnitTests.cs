using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CheckOutEngineUnitTests
    {
        private readonly IMapper _mapper;
        public CheckOutEngineUnitTests()
        {
            // Ideally do not write any test related logic here
            // Only infrastructure and environment setup

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            _mapper = config.CreateMapper();

        }

        [Fact]
        public void CalculateTotals_StandardCustomer_NoCustomerDiscount()
        {
            var address = new Address{Country = "country", City = "city", Street = "street"};

            var target = new CheckOutEngine(new ShippingCalculator(address), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item{ProductId = "prod-1", Price = 2, Quantity = 3}},
                ShippingAddress = address
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal(0, result.CustomerDiscount);
        }

        [Fact] 
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping()
        {
            var originAddress = new Address{Country = "country", City = "city 1", Street = "street"};
            var destinationAddress = new Address { Country = "country", City = "city 2", Street = "street" };

            var target = new CheckOutEngine(new ShippingCalculator(originAddress), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item{ProductId = "prod-1", Price = 2, Quantity = 3}},
                ShippingAddress = destinationAddress
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal((2 * 3) + result.ShippingCost, result.Total);
        }

        [Fact]
        public void CalculateTotals_StandardCustomerMoreThanOneItem_TotalEqualsCostPlusShipping()
        {
            var originAddress = new Address { Country = "country", City = "city 1", Street = "street" };
            var destinationAddress = new Address { Country = "country", City = "city 2", Street = "street" };

            var target = new CheckOutEngine(new ShippingCalculator(originAddress), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new()
                {
                    new Item { ProductId = "prod-1", Price = 2, Quantity = 3 },
                    new Item { ProductId = "prod-1", Price = 4, Quantity = 5 }
                },
                ShippingAddress = destinationAddress
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal((2 * 3) + (4 * 5) + result.ShippingCost, result.Total);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_HasCustomerDiscount()
        {
            var address = new Address{Country = "country", City = "city", Street = "street"};

            var target = new CheckOutEngine(new ShippingCalculator(address), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                Items = new() { new Item{ProductId = "prod-1", Price = 2, Quantity = 3}},
                ShippingAddress = address
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal(10, result.CustomerDiscount);
        }

        [Fact]
        public void CalculateTotals_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var originAddress = new Address { Country = "country", City = "city 1", Street = "street" };
            var destinationAddress = new Address { Country = "country", City = "city 2", Street = "street" };

            var target = new CheckOutEngine(new ShippingCalculator(originAddress), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                Items = new() { new Item { ProductId = "prod-1", Price = 2, Quantity = 3 } },
                ShippingAddress = destinationAddress
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal((((2 * 3) + result.ShippingCost) * 0.9), result.Total);
        }

    }
}