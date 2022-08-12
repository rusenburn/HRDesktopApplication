using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models.LocationModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace HR.Test.DALTests;
public abstract class LocationHttpServiceTests : IDisposable
{
    private bool _disposedValue;

    protected LocationHttpService ServiceUnderTest { get; }
    protected HttpMessageHandlerMock HandlerMock { get; }
    protected HttpClient Client { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    protected CancellationTokenSource TokenSource { get; }
    protected LocationModel LocationModel { get; } = new LocationModel(1, "stret", "Tokyo", "1991", "Amm", 1);
    protected LocationDetailModel LocationDetailModel { get; } = new LocationDetailModel(1, "stret", "Tokyo", "1991", "Amm", 1, new(1, "", 1));
    public LocationHttpServiceTests()
    {
        TokenSource = new CancellationTokenSource();
        HandlerMock = new HttpMessageHandlerMock();
        Client = new HttpClient(HandlerMock);
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new LocationHttpService(Client, AuthorizationStoreMock.Object);
    }


    public class Constructor : LocationHttpServiceTests
    {
        [Fact]
        public void Should_instantiate()
        {
        }
    }

    public class GetAll : LocationHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_empty_on_bad_response(HttpStatusCode badCode)
        {
            // Arrange
            HandlerMock.MockResponse(badCode);
            var cancellationToken = TokenSource.Token;

            // Act
            var result = await ServiceUnderTest.GetAllAsync(cancellationToken);

            // Assert
            Assert.Equal(Enumerable.Empty<LocationModel>(), result);
        }
        [Fact]
        public async Task Should_return_expected_when_success()
        {
            // Arrange
            var expected = new LocationModel[] { LocationModel with { }, LocationModel with { }, LocationModel with { City = "Oslo" } };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);
            var cancellationToken = TokenSource.Token;

            // Act
            var result = await ServiceUnderTest.GetAllAsync(cancellationToken);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(expected.Length, result.Count());
        }
    }

    public class GetOne : LocationHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_on_bad_status_code(HttpStatusCode badCode)
        {
            // arrange
            int locationId = 1;
            HandlerMock.MockResponse(badCode);

            // Act

            var result = await ServiceUnderTest.GetOneAsync(locationId, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task Should_return_LocationDetailModel_on_Success_Statuscode()
        {
            // arrange
            LocationDetailModel expected = LocationDetailModel with { City = "London" };
            int locationId = 1;
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);

            // Act

            var result = await ServiceUnderTest.GetOneAsync(locationId, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.City, result?.City);
        }
    }


    public class CreateOne : LocationHttpServiceTests
    {
        protected LocationCreateModel LocationCreateModel { get; } = new LocationCreateModel("street", "Berlin", "18881", "P", 2);
        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => ServiceUnderTest.CreateOneAsync(null!, TokenSource.Token));
        }


        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_on_bad_status_code(HttpStatusCode badCode)
        {
            // Arrange
            HandlerMock.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(LocationCreateModel, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_LocationModel_on_success_code()
        {
            // Arrange
            var expected = LocationDetailModel with { City = "Neiroubi" };
            HttpResponseMessage response = new()
            {
                StatusCode = HttpStatusCode.Created,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(LocationCreateModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.City, result?.City);
        }
    }

    public class UpdateOne : LocationHttpServiceTests
    {
        protected LocationUpdateModel LocationUpdateModel { get; } = new LocationUpdateModel(1, "street", "Berlin", "18881", "P", 2);

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => ServiceUnderTest.UpdateOneAsync(null!, TokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        public async Task Should_return_null_on_bad_status_code(HttpStatusCode badCode)
        {
            // Arrange
            HandlerMock.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(LocationUpdateModel, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_LocationModel_on_success_code()
        {
            // Arrange
            var expected = LocationModel with { City = "New York" };
            HttpResponseMessage response = new()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };

            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(LocationUpdateModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.City, result?.City);
        }
    }

    public class DeleteOne : LocationHttpServiceTests
    {
        [Fact]
        public async Task Should_return_false_one_bad_request()
        {
            // Arrange
            int locationId = 1;
            HandlerMock.MockResponse(HttpStatusCode.BadRequest);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(locationId, TokenSource.Token);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task Should_return_true_on_success_code()
        {
            // Arrange
            int locationId = 1;
            HandlerMock.MockResponse(HttpStatusCode.NoContent);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(locationId, TokenSource.Token);

            // Assert
            Assert.True(result);
        }

    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                TokenSource.Dispose();
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
