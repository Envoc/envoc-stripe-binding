namespace StripeTerminal;

public static class TaskExtensions
{
    //Taken from: https://stackoverflow.com/a/53393575/15495928
    public static async Task<T> WaitOrCancel<T>(this Task<T> task, CancellationToken token, Action actionWhenCancelled = null)
    {
        if (token.IsCancellationRequested)
        {
            actionWhenCancelled?.Invoke();

            return default;
        }

        await Task.WhenAny(task, token.WhenCancelled());

        if (token.IsCancellationRequested)
        {
            actionWhenCancelled?.Invoke();

            return default;
        }

        return await task;
    }

    public static Task WhenCancelled(this CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
        return tcs.Task;
    }
}