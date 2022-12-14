namespace BasicCache;

public class Request
{
    public async Task<CachedObject> SendRequest()
    {
        await Task.Delay(2000);
        return new CachedObject();
    }
}
