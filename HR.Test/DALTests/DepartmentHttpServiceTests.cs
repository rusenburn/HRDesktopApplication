using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models.DepartmentModels;
using HR.Domain.Models.EmployeeModels;
using HR.Domain.Models.JobHistoryModels;
using HR.Domain.Models.LocationModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace HR.Test.DALTests;
public abstract class DepartmentHttpServiceTests : IDisposable
{
    private bool _disposedValue;

    protected DepartmentHttpService ServiceUnderTest { get; }
    protected HttpMessageHandlerMock HandlerMock { get; }
    protected HttpClient Client { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    protected CancellationTokenSource TokenSource { get; }
    protected DepartmentModel DepartmentModel { get; } = new DepartmentModel(1, "DepName", 1);
    public DepartmentHttpServiceTests()
    {
        HandlerMock = new HttpMessageHandlerMock();
        Client = new HttpClient(HandlerMock);
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new DepartmentHttpService(Client, AuthorizationStoreMock.Object);
        TokenSource = new CancellationTokenSource();
    }

    public class Contructor : DepartmentHttpServiceTests
    {
        [Fact]
        public void Should_Instantiate()
        { }
    }

    public class CreateOne : DepartmentHttpServiceTests
    {
        protected DepartmentCreateModel DepartmentCreateModel { get; } = new DepartmentCreateModel("Department", 1);
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
        public async Task Should_return_null_when_bad_status_code_is_returns(HttpStatusCode badStatusCode)
        {
            // Arrange
            var createModel = DepartmentCreateModel with { DepartmentName = "RandomName" };
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(createModel, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_DepartmentModel_when_status_code_is_success()
        {
            // Arrange
            var expected = DepartmentModel with { DepartmentName = "Lego" };
            var createModel = DepartmentCreateModel with { DepartmentName = "RandomName" };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Created,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);
            // Act
            var result = await ServiceUnderTest.CreateOneAsync(createModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.DepartmentName, result?.DepartmentName);
        }
    }


    public class DeleteOne : DepartmentHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_false_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            var departmentId = 1;
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(departmentId, TokenSource.Token);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task Should_return_true_when_getting_success_status_code()
        {
            // Arrange
            var departmentId = 1;
            var successStatusCode = HttpStatusCode.NoContent;
            HandlerMock.MockResponse(successStatusCode);

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(departmentId, TokenSource.Token);

            // Assert
            Assert.True(result);
        }
    }

    public class GetAll : DepartmentHttpServiceTests
    {

        protected DepartmentQueryModel Query { get; } = new DepartmentQueryModel();
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_empty_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(Query, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_return_DepartmentModels_when__getting_success_status_code()
        {
            // Arrange
            var list = new List<DepartmentModel>() {
                DepartmentModel with { },
                DepartmentModel with { DepartmentName="Mac"},
                DepartmentModel with { DepartmentName ="KFC"}
            };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(list)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(Query, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(list.Count, result.Count());
        }
    }


    public class GetOne : DepartmentHttpServiceTests
    {
        protected DepartmentDetailModel DepartmentDetailModel =
            new(1, "Name", 1,
                new LocationModel(1, "stret", "Coty", "1991", "Provice", 1),
                Array.Empty<EmployeeModel>()
                , Array.Empty<JobHistoryModel>());

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            int departmentId = 1;
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(departmentId, TokenSource.Token);

            // Assert
            Assert.Null(result);
            Assert.Equal(1, HandlerMock.CallCount);
        }

        [Fact]
        public async Task Should_return_DepartmentDetailModel_when_getting_success_status_code()
        {
            // Arrange
            int departmentId = 1;
            var expected = DepartmentDetailModel with { DepartmentName = "Bandai" };
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(departmentId, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.DepartmentName, result?.DepartmentName);
        }
    }


    public class UpdaeOne : DepartmentHttpServiceTests
    {
        protected DepartmentUpdateModel DepartmentUpdateModel = new(1, "KFC", 1);

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
            var updateModel = DepartmentUpdateModel with { DepartmentName = "Name" };
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(updateModel, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task Should_return_DepartmentModel_when_getting_success_status_code()
        {
            var expected = DepartmentModel with { DepartmentName = "Sony" };
            var updateModel = DepartmentUpdateModel with { DepartmentName = "Mic" };
            var response = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.OK
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(updateModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.DepartmentName, result?.DepartmentName);
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
