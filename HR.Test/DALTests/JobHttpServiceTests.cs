using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models.EmployeeModels;
using HR.Domain.Models.JobHistoryModels;
using HR.Domain.Models.JobModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace HR.Test.DALTests;
public class JobHttpServiceTests : IDisposable
{
    private bool _disposedValue;
    protected CancellationTokenSource TokenSource { get; }
    protected JobHttpService ServiceUnderTest { get; }
    protected HttpClient Client { get; }
    protected HttpMessageHandlerMock HandlerMock { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    protected JobModel JobModel { get; } = new JobModel(1, "Job Title", 20, 50);

    public JobHttpServiceTests()
    {
        TokenSource = new CancellationTokenSource();
        HandlerMock = new HttpMessageHandlerMock();
        Client = new HttpClient(HandlerMock);
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new JobHttpService(Client, AuthorizationStoreMock.Object);
    }

    public class Constructor : JobHttpServiceTests
    {
        [Fact]
        public void Should_Instantiate()
        { }
    }

    public class CreateOne : JobHttpServiceTests
    {
        protected JobCreateModel JobCreateModel { get; } = new JobCreateModel("Pro", 1, 51);

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => ServiceUnderTest.CreateOneAsync(null!, TokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            var jobCreate = JobCreateModel with { JobTitle = "SomeTitle" };
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(jobCreate, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_JobModel_when_getting_success_status_code()
        {
            var expected = JobModel with { JobTitle = "WebDev" };
            var jobCreate = JobCreateModel with { JobTitle = "SomeTitle" };
            var response = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.Created
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(jobCreate, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.JobTitle, result?.JobTitle);
        }

    }

    public class DeleteOne : JobHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_false_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            int jobId = 1;
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(jobId, TokenSource.Token);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Should_return_true_when_getting_success_status_code()
        {
            // Arrange
            int jobId = 1;
            HandlerMock.MockResponse(HttpStatusCode.NoContent);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(jobId, TokenSource.Token);

            // Assert
            Assert.True(result);
        }
    }

    public class GetAll : JobHttpServiceTests
    {
        protected JobQueryModel JobQueryModel { get; } = new JobQueryModel();

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_empty_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            var query = JobQueryModel with { };
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(query, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_return_JobModels_when_getting_success_status_code()
        {
            // Arrange
            var list = new List<JobModel>() {
                JobModel with { },
                JobModel with { },
                JobModel with { }
            };
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(list)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(JobQueryModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(list.Count, result.Count());
        }
    }

    public class GetOne : JobHttpServiceTests
    {
        protected JobDetailModel JobDetailModel { get; } =
            new(1,
                "SomeTitle",
                20,
                500,
                Array.Empty<EmployeeModel>(),
                Array.Empty<JobHistoryModel>());

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            int jobId = 1;
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(jobId, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_JobDetailModel_when_getting_success_status_code()
        {
            // Arrange
            var jobId = 1;
            var expected = JobDetailModel with { JobTitle = "Teacher" };
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(jobId, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.JobTitle, result?.JobTitle);
        }
    }

    public class UpdateOne : JobHttpServiceTests
    {
        protected JobUpdateModel JobUpdateModel { get; } = new JobUpdateModel(1, "SomeTitle", 80, 85);

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => ServiceUnderTest.UpdateOneAsync(null!, TokenSource.Token));
        }
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            var jobUpdate = JobUpdateModel with { JobTitle = "SomeRandomTitle" };
            HandlerMock.MockResponse(badStatusCode);
            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(jobUpdate, TokenSource.Token);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_JobModel_when_getting_valid_json_content()
        {
            // Arrange
            var expected = JobModel with { JobTitle = "Teacher" };
            var jobUpdate = JobUpdateModel with { JobTitle = "Some Title" };
            HttpResponseMessage response = new()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.OK
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(jobUpdate, TokenSource.Token);

            Assert.NotNull(result);
            Assert.Equal(expected.JobTitle, result?.JobTitle);
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
