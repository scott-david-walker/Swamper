namespace Swamper;

internal static class HttpRequestCloner
{
    //https://stackoverflow.com/questions/25044166/how-to-clone-a-httprequestmessage-when-the-original-request-has-content
    internal static async Task<HttpRequestMessage> Clone(HttpRequestMessage req)
    {
        var clone = new HttpRequestMessage(req.Method, req.RequestUri);

        var ms = new MemoryStream();
        if (req.Content != null)
        {
            await req.Content.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);

            foreach (var header in req.Content.Headers)
            {
                clone.Content.Headers.Add(header.Key, header.Value);
            }
        }
        
        clone.Version = req.Version;

        foreach (KeyValuePair<string, object?> option in req.Options)
        {
            clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
        }
        
        foreach (var header in req.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        return clone;
    }
}