using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using ProvaPub.Repository;
using ProvaPub.Services;

namespace ProvaPub.Tests
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private CustomerService _svc;

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = WebApplication.CreateBuilder(new string[] { });

            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddDbContext<TestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));

            var serviceProvider = builder.Services.BuildServiceProvider();
            _svc = serviceProvider.GetRequiredService<CustomerService>();
        }

        [Test]
        public async Task CanPurchase_ShouldReturn_True()
        {
            // Arrange
            var customerId = 1;
            var purchaseValue = 100;

            // Act
            var result = await _svc.CanPurchase(customerId, purchaseValue);

            // Assert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task CanPurchase_ShouldReturn_False()
        {
            // Arrange
            var customerId = 1;
            var purchaseValue = 101;

            // Act
            var result = await _svc.CanPurchase(customerId, purchaseValue);

            // Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void CanPurchase_ShouldReturnThrows_UserNotExist_InvalidOperation()
        {
            // Arrange
            var customerId = 1000;
            var purchaseValue = 100;

            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _svc.CanPurchase(customerId, purchaseValue));
        }

        [Test]
        public void CanPurchase_ShouldReturnThrows_Customer_ArgumentOutOfRange()
        {
            // Arrange
            var purchaseValue = 100;

            // Act + Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _svc.CanPurchase(0, purchaseValue));
        }

        [Test]
        public void CanPurchase_ShouldReturnThrows_Value_ArgumentOutOfRange()
        {
            // Arrange
            var customerId = 1;

            // Act + Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _svc.CanPurchase(customerId, 0));
        }
    }
}
