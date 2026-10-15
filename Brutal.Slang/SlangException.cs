namespace Brutal.SlangApi;

/// <summary>
///     Thrown by the throwing form of generated COM-interface extension methods when an
///     underlying call returns a failing <see cref="Result"/>.
/// </summary>
public sealed class SlangException : Exception
{
    public Result Result { get; }

    public SlangException(Result result)
        : base($"Slang call failed with result {result}.")
    {
        Result = result;
    }

    public SlangException(Result result, string message)
        : base(message)
    {
        Result = result;
    }

    public SlangException(Result result, string message, Exception innerException)
        : base(message, innerException)
    {
        Result = result;
    }
}
