using HR.DAL.HttpServices;
using HR.Domain.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace HR.Test.DALTests;

public abstract class AccountHttpServiceTests : IDisposable
{
    protected HttpMessageHandlerMock MessageHandlerMock { get; set; }
    protected HttpClient Client { get; set; }
    protected AccountHttpService ServiceUnderTest { get; }
    protected AccountRegisterModel AccountRegisterModel { get; private set; }
    protected AccountInformationModel AccountInformationModel { get; private set; }
    protected AccountLoginModel AccountLoginModel { get; private set; }
    protected CancellationTokenSource CancellationTokenSource { get; }
    public AccountHttpServiceTests()
    {
        MessageHandlerMock = new HttpMessageHandlerMock();
        Client = new HttpClient(MessageHandlerMock);
        ServiceUnderTest = new AccountHttpService(Client);
        AccountRegisterModel = new AccountRegisterModel("Email@mail.com", "Email@mail.com", "Test123");
        AccountInformationModel = new AccountInformationModel("Email@mail.com", "Email@mail.com");
        AccountLoginModel = new AccountLoginModel("Email@mail.com", "Email@mail.com");
        CancellationTokenSource = new();
    }

    public class RegisterAsync : AccountHttpServiceTests
    {
        [Fact]
        public void ShouldWork_when_instatiating_the_AccountHttpService()
        {

        }

        [Fact]
        public async Task ShouldWork_on_successful_response()
        {
            // Arrange
            AccountRegisterModel = AccountRegisterModel with
            {
                Email = "Email@mail.com"
            };
            var message = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(AccountInformationModel)
            };
            MessageHandlerMock.MockResponse(message);

            // Act
            AccountInformationModel? result = await ServiceUnderTest.RegisterAsync(AccountRegisterModel, CancellationTokenSource.Token);

            // Assert
            Assert.Equal(1, MessageHandlerMock.CallCount);
            Assert.NotNull(result);
            Assert.Equal(AccountInformationModel.Email, result?.Email);
        }
        [Fact]
        public async Task ShouldReturn_null_when_not_success_code()
        {
            // Arrange
            MessageHandlerMock.MockResponse(HttpStatusCode.BadRequest);
            // Act
            var result = await ServiceUnderTest.RegisterAsync(AccountRegisterModel, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }
    }
    public class LoginAsync:AccountHttpServiceTests
    {
        [Fact]
        public async Task Should_throw_ArgumentNullException_when_argument_is_null()
        {
            // Arrange 
            CancellationTokenSource tokenSource = new();
            var token = tokenSource.Token;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await ServiceUnderTest.LoginAsync(null, token));
        }

        [Fact]
        public async Task Should_return_Token_when_json_returned()
        {
            // Arrange
            TokenModel tokenModel = new TokenModel("token", "tt");
            
            var message = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(tokenModel)
            };
            MessageHandlerMock.MockResponse(message);

            // Act
            var response = await ServiceUnderTest.LoginAsync(AccountLoginModel, CancellationTokenSource.Token);

            // Assert
            Assert.Equal(tokenModel.AccessToken, response?.AccessToken);
        }
    }


    public void Dispose()
    {
        CancellationTokenSource.Dispose();
    }
}
//public class AccountHttpServiceTests
//{
//    public HttpClient? Client { get; set; }
//    public AccountHttpService? SUT { get; set; }
//    public class HttpMessageHandlerMock : HttpMessageHandler
//    {
//        private readonly HttpStatusCode _code;
//        public int Calls { get; private set; }

//        public HttpMessageHandlerMock(HttpStatusCode code)
//        {
//            this._code = code;
//            Calls = 0;
//        }
//        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            Calls++;
//            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
//        }
//    }
//    private AccountHttpService GetInstance()
//    {
//        if (SUT is null)
//        {
//            return new AccountHttpService(Client);
//        }
//        return SUT;
//    }

//    public AccountHttpServiceTests()
//    {
//    }
//    [Fact]
//    public void AccountHttpServiceClass_ShouldInstatiate()
//    {

//    }

//    [Fact]
//    public async Task OnOk_ShouldReturnToken()
//    {
//        // arrange
//        var handler = new HttpMessageHandlerMock(HttpStatusCode.OK);
//        Client = new HttpClient(handler);
//        SUT = GetInstance();
//        // act
//        await SUT.RegisterAsync(new AccountRegisterModel("mail@mail.com","mail@mail.com","test123"));

//        // assert
//        Assert.Equal(1, handler.Calls);
//    }
//}
