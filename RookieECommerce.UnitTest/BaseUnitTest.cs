using AutoFixture;
using AutoFixture.AutoMoq;
using RookieEShopper.Application.Dto.Product;

namespace RookieECommerce.UnitTest
{
    public class BaseUnitTest
    {
        public readonly IFixture _fixture;

        public BaseUnitTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());            
        }
    }
}