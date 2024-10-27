using MudExample.Data;

namespace MudExample.Infrastructure;

public class HttpClientInterceptor : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // before request
        var uriBuilder = new UriBuilder(request.RequestUri);
        var query = uriBuilder.Query;
        
        // Check if query string is empty and append version parameter
        query += string.IsNullOrEmpty(query) ? $"v={Consts.Version}" : $"&v={Consts.Version}";
        uriBuilder.Query = query;
        
        request.RequestUri = uriBuilder.Uri;
        
        var response = await base.SendAsync(request, cancellationToken);
        
        // after request

        return response;
    }
}