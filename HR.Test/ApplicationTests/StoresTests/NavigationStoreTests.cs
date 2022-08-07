using HR.Application.Stores;

namespace HR.Test.ApplicationTests.StoresTests;
public class NavigationStoreTests
{
    protected NavigationStore ServiceUnderTest { get; }
    public NavigationStoreTests()
    {
        ServiceUnderTest = new NavigationStore();
    }

    public class Constructor : NavigationStoreTests
    {
        [Fact]
        public void Should_Construct(){}
    }
}
