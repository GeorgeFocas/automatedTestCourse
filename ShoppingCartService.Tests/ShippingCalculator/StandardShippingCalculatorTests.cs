using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Tests.Helpers;
using Shouldly;

namespace ShoppingCartService.Tests.ShippingCalculator;

[Trait("Category", "StandardShipping")]    
public class StandardShippingCalculatorTests
{
    private readonly ShippingMethod _shippingMethod = ShippingMethod.Standard;

    public static List<object[]> SameCountrySameCityData()
    {
        return
        [
            [5, 5.0d],
            [10, 10.0d],
            [12, 12.0d],
            [78, 78.0d]
        ];
    }

    public static List<object[]> SameCountryDifferentCityData()
    {
        return
        [
            [5, 10.0d],
            [10, 20.0d],
            [12, 24.0d],
            [78, 156.0d]
        ];
    }

    public static List<object[]> InternationalData()
    {
        return
        [
            [5, 75.0d],
            [10, 150.0d],
            [12, 180.0d],
            [78, 1170.0d]
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
    public void Can_calculate_same_country_same_city_shipping_cost(uint itemQuantity, double expectedShippingCost)
    {
        // Assert
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
            
        var cart = new Cart
        {
            CustomerId = "id",
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

    [Theory]
    [MemberData(nameof(SameCountryDifferentCityData))]
    public void Can_calculate_same_country_different_city_shipping_cost(uint itemQuantity, double expectedShippingCost)
    {
        // Assert
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Seatle", "1234 left lane.");
            
        var cart = new Cart
        {
            CustomerId = "id",
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

    [Theory]
    [MemberData(nameof(InternationalData))]
    public void Can_calculate_international_shipping_cost(uint itemQuantity, double expectedShippingCost)
    {
        // Assert
        var destinationAddress = ShippingCalculatorFixtures.CreateAddress("BRA", "Rio de Janeiro", "Celso queiroz");
        var warehouseAddress = ShippingCalculatorFixtures.CreateAddress("USA", "Dallas", "1234 left lane.");
            
        var cart = new Cart
        {
            CustomerId = "id",
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