using System.Net;
using System.Net.Http;

namespace HR.Test.Helpers;
public class HttpMessageHandlerMock : HttpMessageHandler
{
    private Exception? _exception;
    private Func<Task<HttpResponseMessage>>? _responseFunc;
    public int CallCount { get; private set; } = 0;

    public void MockResponse(HttpStatusCode code)
    {
        _responseFunc = async () => await Task.FromResult(new HttpResponseMessage()
        {
            StatusCode = code
        });
    }
    public void MockResponse(HttpResponseMessage response)
    {
        _responseFunc = async () => await Task.FromResult(response);
    }
    public void MockResponse(Func<Task<HttpResponseMessage>> func)
    {
        _responseFunc = func;
    }
    public void MockResponse(Exception exception)
    {
        _exception = exception;
    }

    public HttpMessageHandlerMock()
    {
        MockResponse(HttpStatusCode.OK);
    }
    public HttpMessageHandlerMock(HttpStatusCode code)
    {
        MockResponse(code);
    }
    public HttpMessageHandlerMock(HttpResponseMessage reponse)
    {
        MockResponse(reponse);
    }
    public HttpMessageHandlerMock(Exception exception)
    {
        MockResponse(exception);
    }
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CallCount++;
        if (_exception is not null) throw _exception;
        if (_responseFunc is not null) return await _responseFunc();
        return await Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
    }
}
