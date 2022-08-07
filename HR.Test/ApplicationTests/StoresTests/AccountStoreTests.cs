using HR.Application.Stores;
using HR.Domain.Interfaces;
using HR.Domain.StoreModels.AccountStoreModels;

namespace HR.Test.ApplicationTests.StoresTests;
public abstract class AccountStoreTests
{
    protected AccountStore ServiceUnderTest { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }

    public AccountStoreTests()
    {
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new AccountStore(AuthorizationStoreMock.Object);
        IAccountStore sut = new AccountStore(AuthorizationStoreMock.Object);
        sut.Dispose();
    }

    public class Constructor : AccountStoreTests
    {
        [Fact]
        public void Should_be_able_to_construct()
        { }
    }

    public class AccountChangedEvent : AccountStoreTests
    {
        private bool eventRaised = false;
        protected AccountInformationStoreModel AccountInformationStoreModel { get; }
        public AccountChangedEvent()
        {
            AccountInformationStoreModel = new AccountInformationStoreModel("email@mail.com", "username1");
        }
        [Fact]
        public void Should_raise_event_when_account_changed()
        {
            // Arrange
            ServiceUnderTest.AccountChanged += OnAccountChanged;
            // Act
            ServiceUnderTest.Account = AccountInformationStoreModel;

            // Assert
            Assert.True(eventRaised);

            // Cleanup
            ServiceUnderTest.AccountChanged-=OnAccountChanged;
        }

        private void OnAccountChanged()
        {
            eventRaised = true;
        }
    }
}
