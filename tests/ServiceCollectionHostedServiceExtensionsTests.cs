using System;
using nanoFramework.Hosting.UnitTests.Fakes;
using nanoFramework.TestFramework;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class ServiceCollectionHostedServiceExtensionsTests
    {
        [TestMethod]
        public void AddHostedService_must_implement_IHostedService()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService(typeof(FakeHostedService));

                    Assert.ThrowsException(typeof(ArgumentException), () => services.AddHostedService(typeof(ServiceA)));

                }).Build();
        }
    }
}
