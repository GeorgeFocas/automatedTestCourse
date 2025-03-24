using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorUnitTests
    {
        [Fact]
        public void IsValid_doesNotHaveCountry_returnFalse()
        {
            var address = new Address
            {
                Country = null,
                City = "city-1",
                Street = "1234 street"
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_doesNotHaveCity_returnFalse()
        {
            var address = new Address
            {
                Country = "country-1",
                City = null,
                Street = "1234 street"
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_doesNotHaveStreet_returnFalse()
        {
            var address = new Address
            {
                Country = "country-1",
                City = "city-1",
                Street = null
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_validValues_returnTrue()
        {
            var address = new Address
            {
                Country = "country-1",
                City = "city-1",
                Street = "street-1"
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.True(result);
        }
    }
}
