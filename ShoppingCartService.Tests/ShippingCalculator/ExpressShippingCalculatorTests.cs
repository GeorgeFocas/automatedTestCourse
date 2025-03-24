using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Tests.Helpers;
using Shouldly;

namespace ShoppingCartService.Tests.ShippingCalculator;

[Trait("Category", "ExpressShipping")]
public class ExpressShippingCalculatorTests
{
    private readonly ShippingMethod _shippingMethod = ShippingMethod.Express;

    public static List<object[]> SameCountrySameCityData()
    {
        return
        [
            [5,  CustomerType.Standard, 12.500d],
            [10, CustomerType.Standard, 25.00d],
            [12, CustomerType.Standard, 30.00d],
            [78, CustomerType.Standard, 195.00d],
            [13, CustomerType.Premium, 32.500d],
            [78, CustomerType.Premium, 195.000d]
        ];
    }

    public static List<object[]> SameCountryDifferentCityData()
    {
        return
        [
            [5,  CustomerType.Standard, 25.000d],
            [10, CustomerType.Standard, 50.000d],
            [12, CustomerType.Standard, 60.000d],
            [78, CustomerType.Standard, 390.000d],
            [397, CustomerType.Premium, 1985.000d],
        ];
    }

    public static List<object[]> InternationalData()
    {
        return
        [
            [5,  CustomerType.Standard, 187.500d],
            [10, CustomerType.Standard, 375.00d],
            [12, CustomerType.Standard, 450.00d],
            [78, CustomerType.Standard, 2925.00d],
            [5,  CustomerType.Premium, 187.500d],
            [10, CustomerType.Premium, 375.00d],
            [12, CustomerType.Premium, 450.00d],
            [78, CustomerType.Premium, 2925.00d]
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