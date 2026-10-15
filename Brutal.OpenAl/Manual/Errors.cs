// The per-category error model. OpenAL has no per-call return code; errors are read from:
//
//   AL   (alGetError)            : per-context, per-thread, first error wins. With no current context,
//                                  openal-soft returns AL_INVALID_OPERATION (not AL_NO_ERROR).
//   ALC  (alcGetError(device))   : per-device, last error wins. Valid only while the device is open -
//                                  never query a device after a successful close (it is freed).
//   ALC  (alcGetError(NULL))     : global, for failures not attributable to an open device.
//
// GetError(...) reads without throwing; ThrowIfError(...) reads and throws. Wrapper methods never check
// errors automatically - the consumer decides when. The exception is lifetime operations whose failure
// is signaled by a return value (open/create/close/make-current): they check and throw internally, but
// only once, on that failure path.

namespace Brutal.OpenAlApi
{
    public sealed class AlException : Exception
    {
        public AlException(AlError error, string message)
            : base(message) => Error = error;

        public AlError Error { get; }
    }

    public static partial class Al
    {
        public static AlError GetError() => (AlError)PInvoke.GetError();

        public static void ThrowIfError()
        {
            var e = GetError();
            if (e != AlError.NoError)
                throw new AlException(e, Describe(e));
        }

        private static string Describe(AlError e) =>
            e switch
            {
                AlError.InvalidName => "AL_INVALID_NAME: a bad object name was passed.",
                AlError.InvalidEnum => "AL_INVALID_ENUM: an invalid enum was passed.",
                AlError.InvalidValue => "AL_INVALID_VALUE: an invalid value was passed.",
                AlError.InvalidOperation => "AL_INVALID_OPERATION: not allowed in the current state "
                    + "(this is also what a checked AL call reports when no context is current).",
                AlError.OutOfMemory => "AL_OUT_OF_MEMORY: the driver ran out of memory.",
                _ => $"AL error 0x{(int)e:X4}.",
            };
    }

    public sealed class AlcException : Exception
    {
        public AlcException(AlcError error, string message)
            : base(message) => Error = error;

        public AlcError Error { get; }
    }

    public static unsafe partial class Alc
    {
        public static AlcError GetError() => (AlcError)PInvoke.GetError(AlcDevice.Null);

        public static void ThrowIfError()
        {
            var e = GetError();
            if (e != AlcError.NoError)
                throw new AlcException(e, Describe(e));
        }

        internal static AlcError GetError(AlcDevice device) => (AlcError)PInvoke.GetError(device);

        internal static AlcError GetError(AlcCaptureDevice capture) =>
            (AlcError)PInvoke.GetError((void*)capture.Pointer.Value);

        internal static void ThrowIfError(AlcDevice device)
        {
            var e = GetError(device);
            if (e != AlcError.NoError)
                throw new AlcException(e, Describe(e));
        }

        internal static void ThrowIfError(AlcCaptureDevice capture)
        {
            var e = GetError(capture);
            if (e != AlcError.NoError)
                throw new AlcException(e, Describe(e));
        }

        private static string Describe(AlcError e) =>
            e switch
            {
                AlcError.InvalidDevice => "ALC_INVALID_DEVICE: an invalid or closed device handle was used.",
                AlcError.InvalidContext => "ALC_INVALID_CONTEXT: an invalid context handle was used.",
                AlcError.InvalidEnum => "ALC_INVALID_ENUM: an invalid enum was passed.",
                AlcError.InvalidValue => "ALC_INVALID_VALUE: an invalid value was passed.",
                AlcError.OutOfMemory => "ALC_OUT_OF_MEMORY: the driver ran out of memory.",
                _ => $"ALC error 0x{(int)e:X4}.",
            };
    }
}
