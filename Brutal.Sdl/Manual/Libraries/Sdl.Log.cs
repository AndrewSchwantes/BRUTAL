namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static class Log
    {
        public static void Message(SdlLogCategory category, SdlLogPriority priority, string message)
        {
            ArgumentNullException.ThrowIfNull(message);
            PInvoke.LogMessage((int)category, priority, Escape(message));
        }

        public static void Message(string message) => Message(SdlLogCategory.Application, SdlLogPriority.Info, message);

        public static void Trace(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Trace, message);

        public static void Verbose(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Verbose, message);

        public static void Debug(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Debug, message);

        public static void Info(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Info, message);

        public static void Warn(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Warn, message);

        public static void Error(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Error, message);

        public static void Critical(SdlLogCategory category, string message) =>
            Message(category, SdlLogPriority.Critical, message);

        // Escapes printf conversion specifiers so the caller's text is logged verbatim.
        private static string Escape(string message) => message.Contains('%') ? message.Replace("%", "%%") : message;
    }
}
