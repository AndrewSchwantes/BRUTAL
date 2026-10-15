namespace Brutal.JoltApi;

/// <summary>
/// The exception thrown when a Jolt operation fails - a shape, constraint or settings object that won't build, or
/// an object stream that won't restore. Its <see cref="Exception.Message"/> carries the error text Jolt reported
/// (<see cref="Jolt.GetLastError"/>) at the point of failure.
/// </summary>
/// <remarks>
/// A bad argument the binding catches before calling into Jolt still throws the usual
/// <see cref="ArgumentException"/> or <see cref="InvalidOperationException"/>; this one means Jolt itself refused.
/// </remarks>
public class JoltException : Exception
{
    /// <summary>Initializes a new <see cref="JoltException"/> with the given message.</summary>
    /// <param name="message">The failure description, typically Jolt's own error text.</param>
    public JoltException(string message)
        : base(message) { }

    /// <summary>Initializes a new <see cref="JoltException"/> with the given message and inner exception.</summary>
    /// <param name="message">The failure description, typically Jolt's own error text.</param>
    /// <param name="innerException">The underlying exception that caused this failure.</param>
    public JoltException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Creates a <see cref="JoltException"/> whose message is the calling thread's last Jolt error
    /// (<see cref="Jolt.GetLastError"/>).
    /// </summary>
    public static JoltException FromLastError() => new(Jolt.GetLastError());
}
