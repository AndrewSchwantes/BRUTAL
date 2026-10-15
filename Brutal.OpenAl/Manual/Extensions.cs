namespace Brutal.OpenAlApi
{
    public sealed class AlExtensionException : Exception
    {
        public AlExtensionException(string fn)
            : base($"OpenAL extension entry point '{fn}' is not available.") { }

        public AlExtensionException(string fn, string extension)
            : base($"OpenAL extension entry point '{fn}' ({extension}) is not available in the loaded library.") { }

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        internal static void ThrowMissing(string fn, string extension) => throw new AlExtensionException(fn, extension);
    }

    public static unsafe partial class Al
    {
        public static bool IsExtensionPresent(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
            fixed (byte* p = utf8.Span)
                return PInvoke.IsExtensionPresent(p);
        }

        public static void* GetProcAddress(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
            void* fn;
            fixed (byte* p = utf8.Span)
                fn = PInvoke.GetProcAddress(p);

            if (fn is null)
                throw new AlExtensionException(name);
            return fn;
        }
    }

    public static unsafe partial class Alc
    {
        public static bool IsExtensionPresent(string name) => IsExtensionPresent(AlcDevice.Null, name);

        public static void* GetProcAddress(string name) => GetProcAddress(AlcDevice.Null, name);

        // Shared plumbing for the null-device statics above and the device-scoped members on
        // Device.
        internal static bool IsExtensionPresent(AlcDevice device, string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
            fixed (byte* p = utf8.Span)
                return PInvoke.IsExtensionPresent(device, p);
        }

        internal static void* GetProcAddress(AlcDevice device, string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
            void* fn;
            fixed (byte* p = utf8.Span)
                fn = PInvoke.GetProcAddress(device, p);

            if (fn is null)
                throw new AlExtensionException(name);
            return fn;
        }

        // Resolve plumbing for the generated ExtPointers field initializers. The span must have a
        // NUL terminator in memory immediately after it - true for u8 literals, not checked here.
        // Returns null for a missing symbol instead of throwing: a null ExtPointers field is how
        // the generated methods detect an unavailable extension.
        internal static void* GetProcAddressUtf8(AlcDevice device, ReadOnlySpan<byte> nulTerminatedName)
        {
            fixed (byte* p = nulTerminatedName)
                return PInvoke.GetProcAddress(device, p);
        }
    }
}
