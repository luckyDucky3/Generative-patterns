namespace Builder;

public class HttpRequest
{
    public string Url { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> Headers { get; } = new();
    public string Body { get; set; }

    public void Send()
    {
        Console.WriteLine($"Sending {Method} request to {Url}");
        Console.WriteLine("Headers:");
        foreach (var header in Headers)
        {
            Console.WriteLine($"  {header.Key}: {header.Value}");
        }
        if (!string.IsNullOrEmpty(Body))
        {
            Console.WriteLine($"Body: {Body}");
        }
    }
}

public interface IHttpRequestBuilder
{
    void SetUrl(string url);
    void SetMethod(string method);
    void AddHeader(string key, string value);
    void SetBody(string body);
    HttpRequest GetRequest();
}

public class RestApiRequestBuilder : IHttpRequestBuilder
{
    private HttpRequest _request = new();

    public void SetUrl(string url) => _request.Url = url;
    public void SetMethod(string method) => _request.Method = method;
    public void AddHeader(string key, string value) => _request.Headers[key] = value;
    public void SetBody(string body) => _request.Body = body;

    public HttpRequest GetRequest()
    {
        // Автоматически добавляем JSON-заголовок, если его нет
        _request.Headers.TryAdd("Content-Type", "application/json");
        return _request;
    }
}

public class GraphQlRequestBuilder : IHttpRequestBuilder
{
    private HttpRequest _request = new();

    public void SetUrl(string url) => _request.Url = url;
    public void SetMethod(string method) => _request.Method = "POST"; // GraphQL всегда POST
    public void AddHeader(string key, string value) => _request.Headers[key] = value;
    public void SetBody(string query) 
    {
        // GraphQL требует {"query": "..."} формат
        _request.Body = $"{{\"query\": \"{query}\"}}";
    }

    public HttpRequest GetRequest()
    {
        // GraphQL требует заголовок
        _request.Headers["Content-Type"] = "application/json";
        return _request;
    }
}

public class GetRequestDirector
{
    private readonly IHttpRequestBuilder _builder;

    public GetRequestDirector(IHttpRequestBuilder builder)
    {
        _builder = builder;
    }

    // Строит GET-запрос для получения данных
    public HttpRequest BuildGetRequest(string url)
    {
        _builder.SetUrl(url);
        _builder.SetMethod("GET");
        return _builder.GetRequest();
    }
}

public class MutationRequestDirector
{
    private readonly IHttpRequestBuilder _builder;

    public MutationRequestDirector(IHttpRequestBuilder builder)
    {
        _builder = builder;
    }
    public HttpRequest Build(string url, string method, string body, Dictionary<string, string>? headers = null)
    {
        _builder.SetUrl(url);
        _builder.SetMethod(method);
        if (method.ToUpperInvariant() != "GET")
            _builder.SetBody(body);
        if (headers != null)
        {
            foreach (var header in headers)
                _builder.AddHeader(header.Key, header.Value);
        }
        return _builder.GetRequest();
    }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = new RestApiRequestBuilder();
        var director = new GetRequestDirector(builder);
        var request = director.BuildGetRequest("https://film-zone.ru/Film/SearchById/64");
        request.Send();
        
        
        var newBuilder = new GraphQlRequestBuilder();
        var newDirector = new MutationRequestDirector(newBuilder);
        var newRequest = newDirector.Build("https://film-zone.ru/User/Index", "POST", "{\"name\": \"John\"}");
        newRequest.Send();
    }
}