using AutoFixture;
using AutoFixture.AutoMoq;

namespace RookieECommerce.UnitTest
{
    public class BaseUnitTest
    {
        public readonly IFixture _fixture;

        public BaseUnitTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
    }
}