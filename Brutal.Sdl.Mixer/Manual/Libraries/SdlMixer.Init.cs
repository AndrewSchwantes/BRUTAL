namespace Brutal.SdlApi.Mixer;

public static unsafe partial class SdlMixer
{
    public static void Quit()
    {
        // Free the callback GCHandles only after MIX_Quit returns; the audio thread uses them until then.
        PInvoke.Quit();
        ClearCallbacks();
        ExceptionAggregator.Clear();
    }
}
