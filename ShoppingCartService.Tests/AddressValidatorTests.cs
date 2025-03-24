using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Shouldly;

namespace ShoppingCartService.Tests;

[Trait("Category", "AddressValidator")]
public class AddressValidatorTests
{
    private readonly AddressValidator _sut = new();

    public static List<object[]> TestData =
    [
        new object[] { (Address)null, false },
        new object[] { new Address { Country = "Country" }, false },
        new object[] { new Address { City = "City"}, false },
        new object[] { new Address { Street = "Street"}, false },
        new object[] { new Address { Country = "Country" , City = "City"}, false },
        new object[] { new Address { Country = "Country" , Street = "Street"}, false },
        new object[] { new Address { City = "City" , Street = "Street" }, false },
        new object[] { new Address { Country = "Country" , City = "City", Street = "Street" }, true },
    ];
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void Can_validate_address(Address address, bool expectedResult)
    {
        // Arrange
        
        // Act
        var result = _sut.IsValid(address);
        
        // Assert
        result.ShouldBe(expectedResult);
    }
}