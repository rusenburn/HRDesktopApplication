
using HR.Application.Services;
using HR.Domain.Interfaces;

namespace HR.Test.ApplicationTests.ServicesTests;
public class NavigationServiceTests
{

    protected Mock<IFactory<IViewModel>> ViewModelFactoryMock { get; }
    protected Mock<INavigationStore> NavigationStoreMock { get; }
    protected NavigationService<IViewModel> ServiceUnderTest { get; }
    public NavigationServiceTests()
    {
        ViewModelFactoryMock = new Mock<IFactory<IViewModel>>();
        NavigationStoreMock = new Mock<INavigationStore>();
        ServiceUnderTest = new NavigationService<IViewModel>(ViewModelFactoryMock.Object,NavigationStoreMock.Object);
    }

    public class Construct: NavigationServiceTests
    {
        [Fact]
        public void Constructor_should_work()
        {}
    }

    public class Navigate: NavigationServiceTests
    {
        [Fact]
        public void Should_change_current_view_model()
        {
            // Arrange
            Mock<IViewModel> viewModelMock = new Mock<IViewModel>();
            NavigationStoreMock.SetupProperty(x => x.CurrentViewModel);
            ViewModelFactoryMock.Setup(x => x.Create())
                .Returns(viewModelMock.Object);
            // Act
            ServiceUnderTest.Navigate();

            // Assert
            NavigationStoreMock.VerifySet(x => x.CurrentViewModel=viewModelMock.Object, Times.Once);
        }
    }


}
