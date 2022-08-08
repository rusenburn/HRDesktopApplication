using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models;
using HR.Domain.Models.CountryModels;
using HR.Domain.Models.RegionModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;


namespace HR.Test.DALTests;
public abstract class RegionHttpServiceTests : IDisposable
{
    private bool _disposedValue;
    protected RegionHttpService ServiceUnderTest { get; }
    protected HttpMessageHandlerMock HttpMessageHandlerMock { get; }
    protected HttpClient Client { get; }
    protected CancellationTokenSource CancellationTokenSource { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    public RegionHttpServiceTests()
    {
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        CancellationTokenSource = new();
        HttpMessageHandlerMock = new HttpMessageHandlerMock();
        Client = new(HttpMessageHandlerMock);
        ServiceUnderTest = new RegionHttpService(Client, AuthorizationStoreMock.Object);
    }

    public class Constructor : RegionHttpServiceTests
    {
        [Fact]
        public void Constructor_should_initialize()
        { }
    }

    public class CreateOneAsync : RegionHttpServiceTests
    {
        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passed_null()
        {
            // Arrange
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>("regionCreateModel", async () => await ServiceUnderTest.CreateOneAsync(null, cancellationToken));
        }

        [Fact]
        public async Task Should_return_regionDetailObject_Successfully_when_client_return_json()
        {
            // Arrange
            var regionCreate = new RegionCreateModel("Europe");
            var expected = new RegionDetailModel(1, "Asia", new CountryModel[0]);
            HttpMessageHandlerMock.MockResponse(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = JsonContent.Create(expected)
            });

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(regionCreate, CancellationTokenSource.Token);
            // Assert
            Assert.Equal(expected?.RegionName, result?.RegionName);
            Assert.Equal(expected?.RegionId, result?.RegionId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_null_when_StatusCode_is_not_success(HttpStatusCode failCode)
        {
            // Arrange
            var regionCreate = new RegionCreateModel("Europe");
            HttpMessageHandlerMock.MockResponse(failCode);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(regionCreate, CancellationTokenSource.Token);
            // Assert
            Assert.Null(result);
        }
    }

    public class DeleOneAsync : RegionHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Accepted)]
        [InlineData(HttpStatusCode.NoContent)]
        public async Task Should_return_true_on_success(HttpStatusCode successCode)
        {
            // Arrange
            HttpMessageHandlerMock.MockResponse(successCode);

            // Act
            bool result = await ServiceUnderTest.DeleteOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_false_on_failure(HttpStatusCode failCode)
        {
            // Arrange
            HttpMessageHandlerMock.MockResponse(failCode);

            // Act
            bool result = await ServiceUnderTest.DeleteOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.False(result);
        }
    }


    public class GetAllAsync : RegionHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Accepted)]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        public async Task Should_return_an_enumerable(HttpStatusCode statusCode)
        {
            // Arrange
            HttpMessageHandlerMock.MockResponse(new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = JsonContent.Create(new RegionModel[] { })
            });
            // Act
            var result = await ServiceUnderTest.GetAllAsync(CancellationTokenSource.Token);
            // Assert
            Assert.NotNull(result);
        }

    }

    public class GetOneAsync : RegionHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_null_when_not_success(HttpStatusCode badCode)
        {
            // Arrange
            HttpMessageHandlerMock.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_RegionDetailModel_when_success()
        {
            // Arrange
            RegionDetailModel expected = new RegionDetailModel(1, "Europe", new CountryModel[] { });
            HttpMessageHandlerMock.MockResponse(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            });

            // Act
            var result = await ServiceUnderTest.GetOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.RegionName, result?.RegionName);
            Assert.Equal(expected.RegionId, result?.RegionId);
        }

    }

    public class UpdateOneAsync : RegionHttpServiceTests
    {
        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null_value()
        {
            // Arrange

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>("regionUpdateModel", () => ServiceUnderTest.UpdateOneAsync(null, CancellationTokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_null_on_bad_statusCode(HttpStatusCode badStatusCode)
        {
            // Arrange
            RegionUpdateModel model = new RegionUpdateModel(1, "Europe");
            HttpMessageHandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(model, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_model_on_bad_statusCode()
        {
            // Arrange
            RegionUpdateModel update = new RegionUpdateModel(1, "Europe");
            RegionDetailModel expected = new RegionDetailModel(1, "Asia", new CountryModel[] { });
            HttpMessageHandlerMock.MockResponse(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            });

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(update, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected?.RegionName, result?.RegionName);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                CancellationTokenSource.Dispose();
            }
            _disposedValue = true;
        }
    }
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
