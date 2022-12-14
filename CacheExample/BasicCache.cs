namespace BasicCache;

public class BasicCache
{
    private Dictionary<Request, CachedObject> Cache = new();
    private Dictionary<Request, Task<CachedObject>> PendingRequests = new();

    public Task<CachedObject> GetFromCache(Request request)
    {
        if (Cache.ContainsKey(request))
        {
            Console.WriteLine("Returned from cache");
            return Task.FromResult(Cache[request]);
        }

        if (PendingRequests.ContainsKey(request))
        {
            Console.WriteLine("Returned from pending requests");
            return PendingRequests[request];
        }

        var requestTask = request.SendRequest().ContinueWith(task =>
        {
            Console.WriteLine("Moved from pending requests to cache");
            var result = task.Result;
            PendingRequests.Remove(request);
            Cache.Add(request, result);
            return result;
        });
        PendingRequests.Add(request, requestTask);

        Console.WriteLine("Returned from request");
        return requestTask;
    }
}
