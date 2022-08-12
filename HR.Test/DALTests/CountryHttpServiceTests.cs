using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models.CountryModels;
using HR.Domain.Models.RegionModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace HR.Test.DALTests;
public abstract class CountryHttpServiceTests : IDisposable
{
    private bool _disposedValue;

    protected HttpMessageHandlerMock Handler { get; }
    protected HttpClient Client { get; }
    protected CountryHttpService ServiceUnderTest { get; }
    protected CancellationTokenSource CancellationTokenSource { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    protected CountryModel CountryModel { get; } = new CountryModel(1, "Jordan", 2);
    protected CountryDetailModel CountryDetailModel { get; } = new CountryDetailModel(1, "China", 2, new RegionModel(1, "Asia"));
    public CountryHttpServiceTests()
    {
        Handler = new HttpMessageHandlerMock();
        Client = new HttpClient(Handler);
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new CountryHttpService(Client, AuthorizationStoreMock.Object);
        CancellationTokenSource = new CancellationTokenSource();
    }

    public class Constructor : CountryHttpServiceTests
    {
        [Fact]
        public void Should_instantiate_countryHttpService()
        { }
    }

    public class Create : CountryHttpServiceTests
    {
        protected CountryCreateModel CountryCreateModel { get; } = new CountryCreateModel("Jordan", 1);

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            // Arrange
            var func = () => ServiceUnderTest.CreateOneAsync(null!, CancellationTokenSource.Token);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>("createModel", func);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_on_failure_response(HttpStatusCode badCode)
        {
            // Arrange
            Handler.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(CountryCreateModel, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_CountryModel_on_Successful_response()
        {
            // Arrange
            var expected = CountryModel with
            {
                CountryName = "Egypt"
            };
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = JsonContent.Create(expected)
            };
            Handler.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(CountryCreateModel, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result?.CountryName, expected.CountryName);
        }
    }


    public class Delete : CountryHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_false_on_bad_response(HttpStatusCode badCode)
        {
            // Arrange
            Handler.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Accepted)]
        public async Task Should_return_true_on_success_response(HttpStatusCode successCode)
        {
            // Arrage
            Handler.MockResponse(successCode);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(1, CancellationTokenSource.Token);

            // Assert
            Assert.True(result);
        }
    }

    public class GetAll : CountryHttpServiceTests
    {
        protected CountryQueryModel Query = new CountryQueryModel();

        [Fact]
        public async Task Should_return_empty_IEnumerable_on_failure()
        {
            // Arrange
            Handler.MockResponse(HttpStatusCode.BadRequest);
            // Act
            var result = await ServiceUnderTest.GetAllAsync(Query, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_return_IEnumerable_on_success()
        {
            // Arrange
            CountryModel[] expected = new CountryModel[] { CountryModel, CountryModel with { CountryName = "Syria" } };
            var response = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected)
            };
            Handler.MockResponse(response);
            // Act
            var result = (await ServiceUnderTest.GetAllAsync(Query, CancellationTokenSource.Token));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result?.Count(), expected.Length);
        }

    }

    public class GetOne : CountryHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_null_on_bad_response(HttpStatusCode badCode)
        {
            // Arrange
            int countryId = 1;
            Handler.MockResponse(badCode);


            // Act
            var result = await ServiceUnderTest.GetOneAsync(countryId, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task Should_return_null_CountryDetailModel_on_success()
        {
            // Arrange
            int countryId = 1;
            var expected = CountryDetailModel with { CountryName = "Brasil" };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            Handler.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(countryId, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected?.CountryName, result?.CountryName);
        }
    }

    public class UpdateOne : CountryHttpServiceTests
    {
        protected CountryUpdateModel CountryUpdateModel { get; } = new CountryUpdateModel(1, "Jordan", 2);

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            // Arrange
            CountryUpdateModel nullValue = null!;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>("updateModel",
                () => ServiceUnderTest.UpdateOneAsync(nullValue!, CancellationTokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.UnprocessableEntity)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task Should_return_null_on_bad_response(HttpStatusCode badCode)
        {
            // Arrange
            Handler.MockResponse(badCode);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(CountryUpdateModel with { CountryName = "Panama" }, CancellationTokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_CountryModel_on_successful_statusCode()
        {
            // Arrange
            var expected = CountryModel with { CountryName = "United Kingdom" };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            Handler.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(CountryUpdateModel with { CountryName = "Panama" }, CancellationTokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.CountryName, result?.CountryName);
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
