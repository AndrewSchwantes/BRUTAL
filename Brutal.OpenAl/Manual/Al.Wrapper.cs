namespace Brutal.OpenAlApi;

public static unsafe partial class Al
{
    public static string? GetString(AlGetString param)
    {
        // Raw byte* return, copied to managed before anything can invalidate the library-owned
        // buffer. A null result is either the NULL sentinel or an error.
        return StringUtils.Utf8ToString(PInvoke.GetString((int)param));
    }

    public static string[] GetExtensions()
    {
        var s = GetString(AlGetString.Extensions);
        return string.IsNullOrEmpty(s) ? [] : s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    public static AlDistanceModel DistanceModel
    {
        get => (AlDistanceModel)PInvoke.GetInteger(Constants.AL_DISTANCE_MODEL);
        set => PInvoke.DistanceModel((int)value);
    }

    public static float DopplerFactor
    {
        get => PInvoke.GetFloat(Constants.AL_DOPPLER_FACTOR);
        set => PInvoke.DopplerFactor(value);
    }

    public static float SpeedOfSound
    {
        get => PInvoke.GetFloat(Constants.AL_SPEED_OF_SOUND);
        set => PInvoke.SpeedOfSound(value);
    }

    public static void DeferUpdates() => PInvoke.DeferUpdatesSOFT();

    public static void ProcessUpdates() => PInvoke.ProcessUpdatesSOFT();

    public static DeferredUpdatesScope DeferUpdatesScope()
    {
        DeferUpdates();
        return new DeferredUpdatesScope();
    }

    public static string[] ResamplerNames
    {
        get
        {
            var count = PInvoke.GetInteger(Constants.AL_NUM_RESAMPLERS_SOFT);
            var names = new string[count];
            for (var i = 0; i < count; i++)
            {
                names[i] =
                    StringUtils.Utf8ToString(PInvoke.GetStringiSOFT(Constants.AL_RESAMPLER_NAME_SOFT, i))
                    ?? string.Empty;
            }
            return names;
        }
    }

    public static int DefaultResampler => PInvoke.GetInteger(Constants.AL_DEFAULT_RESAMPLER_SOFT);

    public static float GainLimit => PInvoke.GetFloat(Constants.AL_GAIN_LIMIT_SOFT);

    public static void PlaySourcesAt(ReadOnlySpan<AlSource> sources, long deviceClockTime)
    {
        if (sources.IsEmpty)
            return;

        var names = sources.Cast<AlSource, uint>();
        fixed (uint* p = names)
            PInvoke.SourcePlayAtTimevSOFT(names.Length, p, deviceClockTime);
    }
}

public readonly ref struct DeferredUpdatesScope
{
    public void Dispose() => Al.ProcessUpdates();
}
