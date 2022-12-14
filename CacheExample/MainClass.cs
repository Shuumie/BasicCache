using System.Diagnostics;
using BasicCache;

class MainClass
{
    static public void Main(String[] args)
    {
        BasicCache.BasicCache cache = new BasicCache.BasicCache();
        Request request = new Request();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var task1 = cache.GetFromCache(request);
        var task2 = cache.GetFromCache(request);
        var task3 = cache.GetFromCache(request);

        TimeSpan elapsedForCacheRequests = stopwatch.Elapsed;
        
        Task<CachedObject> task4 = null, task5 = null, task6 = null;

        var result = Task.Delay(2000).ContinueWith(_ =>
        {
            task4 = cache.GetFromCache(request);
            task5 = cache.GetFromCache(request);
            task6 = cache.GetFromCache(request);
            return true;
        }).Result;

        if (task1.Result == task2.Result && task2.Result == task3.Result && task3.Result == task4.Result &&
            task4.Result == task5.Result && task5.Result == task6.Result)
        {
            Console.WriteLine("All cache results are the same");
        }
        else
        {
            Console.WriteLine("There is a problem in the cache");
        }

        var elapsedWhole = stopwatch.Elapsed;
        stopwatch.Stop();
        Console.WriteLine($"Elapsed for just the cache requests without results = {elapsedForCacheRequests}");
        Console.WriteLine($"Elapsed for all the cache results = {elapsedWhole}");
    }
}
