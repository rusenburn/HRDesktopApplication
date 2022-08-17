using HR.DAL.HttpServices;
using HR.Domain.Interfaces;
using HR.Domain.Models.DepartmentModels;
using HR.Domain.Models.EmployeeModels;
using HR.Domain.Models.JobHistoryModels;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace HR.Test.DALTests;
public abstract class EmployeeHttpServiceTests : IDisposable
{
    private bool _disposedValue;

    protected EmployeeHttpService ServiceUnderTest { get; }
    protected CancellationTokenSource TokenSource { get; }
    protected HttpMessageHandlerMock HandlerMock { get; }
    protected HttpClient Client { get; }
    protected Mock<IAuthorizationStore> AuthorizationStoreMock { get; }
    protected EmployeeModel EmployeeModel { get; }
        = new(1,
              "FirstName",
              "LastName",
              "Email@email.com",
              "07777777",
              DateTime.UtcNow,
              500,
              1,
              1,
              1);

    public EmployeeHttpServiceTests()
    {
        HandlerMock = new HttpMessageHandlerMock();
        Client = new HttpClient(HandlerMock);
        TokenSource = new CancellationTokenSource();
        AuthorizationStoreMock = new Mock<IAuthorizationStore>();
        ServiceUnderTest = new EmployeeHttpService(AuthorizationStoreMock.Object, Client);
    }

    public class Constructor : EmployeeHttpServiceTests
    {
        [Fact]
        public void Should_Instantiate()
        {
        }
    }

    public class GetAll : EmployeeHttpServiceTests
    {

        protected EmployeeQueryModel EmployeeQueryModel { get; } = new EmployeeQueryModel();
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_empty_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(EmployeeQueryModel, TokenSource.Token);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_return_EmployeeModels_when_getting_success_status_code()
        {
            // Arrange
            var expected = new EmployeeModel[] {
                EmployeeModel with { FirstName = "Firstname 1"},
                EmployeeModel with { FirstName = "firstname 2"},
                EmployeeModel with { FirstName = "firstname 3"}
            };

            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expected)
            };
            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.GetAllAsync(EmployeeQueryModel, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expected.Length, result.Count());
        }
    }

    public class GetOne : EmployeeHttpServiceTests
    {
        protected EmployeeDetailModel EmployeeDetailModel { get; }
        public GetOne()
        {
            EmployeeDetailModel = new EmployeeDetailModel(
        1,
          "FirstName",
          "LastName",
          "Email@email.com",
          "07777777",
          DateTime.UtcNow,
          500,
          1,
          1,
          1,
          EmployeeModel with { },
          new DepartmentModel(1, "Dep", 1),
          Array.Empty<JobHistoryModel>());
        }
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            int employeeId = 1;
            HandlerMock.MockResponse(badStatusCode);

            // Act
            var result = await ServiceUnderTest.GetOneAsync(employeeId, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_EmployeeDetailModel_when_getting_valid_json_content()
        {
            var expected = EmployeeDetailModel with { FirstName = "Joe" };
            var employeeId = 1;
            HttpResponseMessage message = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.OK
            };
            HandlerMock.MockResponse(message);

            // act
            var result = await ServiceUnderTest.GetOneAsync(employeeId, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.FirstName, result?.FirstName);
        }
    }

    public class CreateOne : EmployeeHttpServiceTests
    {
        protected EmployeeCreateModel EmployeeCreateModel { get; } = new EmployeeCreateModel(1, "FirstName", "LastName", "Email", "09999999");

        [Fact]
        public async Task Should_throw_ArgumentNullException_when_passing_null_argument()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.CreateOneAsync(null!, TokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            HandlerMock.MockResponse(badStatusCode);
            var employeeCreate = EmployeeCreateModel with { FirstName = "Laila" };

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(employeeCreate, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task Should_return_EmployeeModel_when_getting_valid_json_content()
        {
            var expected = EmployeeModel with { FirstName = "Robert" };
            var employeeCreate = EmployeeCreateModel with { FirstName = "AJ" };
            var response = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.Created,
            };

            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.CreateOneAsync(employeeCreate, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.FirstName, result?.FirstName);
        }
    }

    public class UpdateOne : EmployeeHttpServiceTests
    {
        protected EmployeeUpdateModel EmployeeUpdateModel { get; } = new EmployeeUpdateModel(1, "Firstnamt", "lastname", "mail@mail.com", "0999999", 1);

        [Fact]
        public async Task Should_throw_ArgumentNullExpception_when_passing_null()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => ServiceUnderTest.UpdateOneAsync(null!, TokenSource.Token));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_null_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            HandlerMock.MockResponse(badStatusCode);
            var employeeUpdate = EmployeeUpdateModel with { FirstName = "Employee" };

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(employeeUpdate, TokenSource.Token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_EmployeeModel_when_getting_success_status_code()
        {
            var expected = EmployeeModel with { FirstName = "Ray" };
            var employeeUpdate = EmployeeUpdateModel with { FirstName = "Rona" };
            var response = new HttpResponseMessage()
            {
                Content = JsonContent.Create(expected),
                StatusCode = HttpStatusCode.OK,
            };

            HandlerMock.MockResponse(response);

            // Act
            var result = await ServiceUnderTest.UpdateOneAsync(employeeUpdate, TokenSource.Token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.FirstName, result?.FirstName);
        }
    }


    public class DeleteOne : EmployeeHttpServiceTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_return_false_when_getting_bad_status_code(HttpStatusCode badStatusCode)
        {
            // Arrange
            HandlerMock.MockResponse(badStatusCode);
            var employeeId = 1;

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(employeeId, TokenSource.Token);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.OK)]
        public async Task Should_return_true_when_getting_success_status_code(HttpStatusCode successCode)
        {
            // Arrange
            HandlerMock.MockResponse(successCode);
            var employeeId = 1;

            // Act
            var result = await ServiceUnderTest.DeleteOneAsync(employeeId, TokenSource.Token);

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
                HandlerMock.Dispose();
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
