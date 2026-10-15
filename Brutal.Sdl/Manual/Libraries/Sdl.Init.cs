namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static unsafe partial class Init
    {
        public static void Initialize(SdlInitFlags flags)
        {
            // SDL_Init returns SDL bool (false = failure), rendered as a byte by the raw layer.
            if (PInvoke.Init(flags) == 0)
                throw SdlException.FromError("SDL_Init failed");
        }

        public static void Quit()
        {
            Events.ClearWatches();
            Sdl.ExceptionAggregator.Clear();
            PInvoke.Quit();
        }
    }
}
