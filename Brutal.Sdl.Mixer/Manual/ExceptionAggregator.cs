namespace Brutal.SdlApi.Mixer;

internal sealed class ExceptionAggregator
{
    public const int MaxRetained = 16;

    private readonly List<Exception> _exceptions = [];
    private int _dropped;
    private readonly Lock _lock = new();

    public void Push(string message, Exception? innerException = null)
    {
        var wrapped = innerException is null
            ? new SdlCallbackException(message)
            : new SdlCallbackException(message, innerException);

        // Store before logging so a faulting logger cannot drop the exception.
        lock (_lock)
        {
            if (_exceptions.Count < MaxRetained)
                _exceptions.Add(wrapped);
            else
                _dropped++;
        }

        try
        {
            SdlMixer.TraceLog.Error(wrapped);
        }
        catch
        {
            // A logging fault must not cross the native boundary.
        }
    }

    public void CheckExceptions()
    {
        Exception exception;
        lock (_lock)
        {
            if (_exceptions.Count == 0)
                return;

            var dropped = _dropped;
            if (_exceptions.Count == 1 && dropped == 0)
            {
                exception = _exceptions[0];
            }
            else
            {
                var message =
                    $"{_exceptions.Count} SDL_mixer callback exception(s) occurred."
                    + (
                        dropped > 0
                            ? $" A further {dropped} were dropped (only the first {MaxRetained} are retained)."
                            : ""
                    );
                exception = new AggregateException(message, _exceptions.ToArray());
            }

            _exceptions.Clear();
            _dropped = 0;
        }

        throw exception;
    }

    public void Clear()
    {
        lock (_lock)
        {
            _exceptions.Clear();
            _dropped = 0;
        }
    }
}
