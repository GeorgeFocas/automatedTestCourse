using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Shouldly;

namespace ShoppingCartService.Tests
{
    public class ExpeditedShippingCalculatorTests
    {
        private readonly ShippingMethod _shippingMethod = ShippingMethod.Expedited;

        public static List<object[]> SameCountrySameCityData()
        {
            return
            [
                [5, CustomerType.Standard, 6.0d],
                [10, CustomerType.Standard, 12.0d],
                [12, CustomerType.Standard, 14.400d],
                [78, CustomerType.Standard, 93.600d],
                [5, CustomerType.Premium, 5.0d],
                [10, CustomerType.Premium, 10.0d],
                [12, CustomerType.Premium, 12.000d],
                [78, CustomerType.Premium, 78.000d]
            ];
        }

        public static List<object[]> SameCountryDifferentCityData()
        {
            return
            [
                [5, CustomerType.Standard,12.0d],
                [10, CustomerType.Standard, 24.0d],
                [12, CustomerType.Standard, 28.800d],
                [78, CustomerType.Standard, 187.200d],
                [5, CustomerType.Premium, 10.0d],
                [10, CustomerType.Premium, 20.0d],
                [12, CustomerType.Premium, 24.000d],
                [78, CustomerType.Premium, 156.000d]
            ];
        }

        public static List<object[]> InternationalData()
        {
            return
            [
                [5, CustomerType.Standard,  90.000d],
                [10, CustomerType.Standard, 180.000d],
                [12, CustomerType.Standard, 216.000d],
                [78, CustomerType.Standard, 1404.000d],
                [5, CustomerType.Premium,  75.000d],
                [10, CustomerType.Premium, 150.000d],
                [12, CustomerType.Premium, 180.000d],
                [78, CustomerType.Premium, 1170.000d]
            ];
        }

        [Fact]
        public void Shipping_cost_is_zero_when_the_cart_does_not_have_items()
        {
            // Assert
            var cart = new Cart
            {
                CustomerId = "id",
                Items = [],
                ShippingAddress = new Address
                {
                    Country = "USA",
                    City = "Dallas",
                    Street = "1234 left lane."
                }
            };
            var sut = new ShippingCalculator();

            // Act
            var result = sut.CalculateShippingCost(cart);
            result.ShouldBe(0d);
        }

        [Theory]
        [MemberData(nameof(SameCountrySameCityData))]
        public void Can_calculate_same_country_same_city_shipping_cost(uint itemQuantity, CustomerType customerType, double expectedShippingCost)
        {
            // Assert
            var cart = new Cart
            {
                CustomerId = "id",
                CustomerType = customerType,
                Items =
                [
                    new()
                    {
                        Price = 10d,
                        ProductId = "id",
                        Quantity = itemQuantity
                    }
                ],
                ShippingAddress = new Address
                {
                    Country = "USA",
                    City = "Dallas",
                    Street = "1234 left lane."
                },
                ShippingMethod = _shippingMethod
            };
            var sut = new ShippingCalculator();

            // Act
            var result = sut.CalculateShippingCost(cart);

            // Assert
            result.ShouldBe(expectedShippingCost, tolerance: 0.001d);
        }

        [Theory]
        [MemberData(nameof(SameCountryDifferentCityData))]
        public void Can_calculate_same_country_different_city_shipping_cost(uint itemQuantity, CustomerType customerType, double expectedShippingCost)
        {
            // Assert
            var cart = new Cart
            {
                CustomerId = "id",
                CustomerType = customerType,
                Items =
                [
                    new()
                    {
                        Price = 10d,
                        ProductId = "id",
                        Quantity = itemQuantity
                    }
                ],
                ShippingAddress = new Address
                {
                    Country = "USA",
                    City = "Seatle",
                    Street = "1234 left lane."
                },
                ShippingMethod = _shippingMethod
            };
            var sut = new ShippingCalculator();

            // Act
            var result = sut.CalculateShippingCost(cart);

            // Assert
            result.ShouldBe(expectedShippingCost, tolerance: 0.001d);
        }

        [Theory]
        [MemberData(nameof(InternationalData))]
        public void Can_calculate_international_shipping_cost(uint itemQuantity, CustomerType customerType, double expectedShippingCost)
        {
            // Assert
            var cart = new Cart
            {
                CustomerId = "id",
                CustomerType = customerType,
                Items =
                [
                    new()
                    {
                        Price = 10d,
                        ProductId = "id",
                        Quantity = itemQuantity
                    }
                ],
                ShippingAddress = new Address
                {
                    Country = "BRA",
                    City = "Rio de Janeiro",
                    Street = "Rua Celso Queiroz"
                },
                ShippingMethod = _shippingMethod
            };
            var sut = new ShippingCalculator();

            // Act
            var result = sut.CalculateShippingCost(cart);

            // Assert
            result.ShouldBe(expectedShippingCost);
        }
    }
}
