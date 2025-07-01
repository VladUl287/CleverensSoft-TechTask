using Task_2;

var toExclusive = 1_000;

var getCountTask = Parallel.ForAsync(0, toExclusive, (i, token) => {
    Console.WriteLine("GetCount");
    int count = Server.GetCount();
    return ValueTask.CompletedTask;
});

var addCountTask = Parallel.ForAsync(0, toExclusive, (i, token) =>
{
    Console.WriteLine("AddToCount");
    Server.AddToCount(i);
    return ValueTask.CompletedTask;
});

await Task.WhenAll(getCountTask, addCountTask);

Console.WriteLine("Success");