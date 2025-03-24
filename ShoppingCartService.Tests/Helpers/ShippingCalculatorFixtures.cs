using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.Tests.Helpers;

public static class ShippingCalculatorFixtures
{
    public static Address CreateAddress(string country, string city, string street) =>
        new()
        {
            Country = country,
            City = city,
            Street = street
        };

    public static Cart CreateCart(CustomerType customerType = CustomerType.Standard, uint itemQuantity = 0,
        ShippingMethod shippingMethod = ShippingMethod.Standard, Address? shippingAddress = null) => new()
    {
                CustomerId = "id",
                CustomerType = customerType,
                Items =
                [
                    new Item
                    {
                        Price = 10d,
                        ProductId = "id",
                        Quantity = itemQuantity
                    }
                ],
                ShippingAddress = shippingAddress,
                ShippingMethod = shippingMethod
            };
}