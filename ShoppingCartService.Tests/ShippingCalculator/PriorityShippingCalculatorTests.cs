using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Tests.Helpers;
using Shouldly;

namespace ShoppingCartService.Tests.ShippingCalculator;

[Trait("Category", "PriorityShipping")]
public class PriorityShippingCalculatorTests
{
    private readonly ShippingMethod _shippingMethod = ShippingMethod.Priority;

    public static List<object[]> SameCountrySameCityData()
    {
        return
        [
            [5,  CustomerType.Standard, 10.0d],
            [10, CustomerType.Standard, 20.0d],
            [12, CustomerType.Standard, 24.000d],
            [78, CustomerType.Standard, 156.000d],
            [13, CustomerType.Premium, 13.000d],
            [78, CustomerType.Premium, 78.000d]
        ];
    }

    public static List<object[]> SameCountryDifferentCityData()
    {
        return
        [
            [5,  CustomerType.Standard, 20.0d],
            [10, CustomerType.Standard, 40.00d],
            [12, CustomerType.Standard, 48.000d],
            [78, CustomerType.Standard, 312.000d],
            [78, CustomerType.Premium, 156.000d],
            [397, CustomerType.Premium, 794.000d],
        ];
    }

    public static List<object[]> InternationalData()
    {
        return
        [
            [5,  CustomerType.Standard, 150.000d],
            [10, CustomerType.Standard, 300.000d],
            [12, CustomerType.Standard, 360.000d],
            [78, CustomerType.Standard, 2340.000d],
            [5,  CustomerType.Premium, 75.000d],
            [10, CustomerType.Premium,  150.000d],
            [12, CustomerType.Premium,  180.000d],
            [78, CustomerType.Premium,  1170.000d]
        ];
    }

    [Fact]
    public void Shipping_cost_is_zero_when_the_cart_does_not_have_items()
    {
        // Assert
        var address = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
            
        var cart = new Cart
        {
            CustomerId = "id",
            Items = [],
            ShippingAddress = address
        };
        var sut = new BusinessLogic.ShippingCalculator();

        // Act
        var result = sut.CalculateShippingCost(cart);
        result.ShouldBe(0d);
    }

    [Theory]
    [MemberData(nameof(SameCountrySameCityData))]
    public void Can_calculate_same_country_same_city_shipping_cost(uint itemQuantity, CustomerType customerType, double expectedShippingCost)
    {
        // Assert
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
            
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
            ShippingAddress = destinationAddress,
            ShippingMethod = _shippingMethod
        };
        var sut = new BusinessLogic.ShippingCalculator(warehouseAddress);

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
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Seatle", "1234 left lane.");
            
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
            ShippingAddress = destinationAddress,
            ShippingMethod = _shippingMethod
        };
        var sut = new BusinessLogic.ShippingCalculator(warehouseAddress);

        // Act
        var result = sut.CalculateShippingCost(cart);

        // Assert
        result.ShouldBe(expectedShippingCost, tolerance: 0.001d);
    }

    [Theory]
    [MemberData(nameof(InternationalData))]
    public void Can_calculate_international_shipping_cost(uint itemQuantity, CustomerType customerType,double expectedShippingCost)
    {
        // Assert
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("BRA", "Rio de Janeiro", "Celso queiroz");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
            
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
            ShippingAddress = destinationAddress,
            ShippingMethod = _shippingMethod
        };
        var sut = new BusinessLogic.ShippingCalculator(warehouseAddress);

        // Act
        var result = sut.CalculateShippingCost(cart);

        // Assert
        result.ShouldBe(expectedShippingCost);
    }
}