namespace Brutal.OpenAlApi;

public readonly record struct AlSampleOffsetLatency(long SampleOffset, long Latency)
{
    public int WholeSamples => (int)(SampleOffset >> 32);
}

public readonly record struct AlSampleOffsetClock(long SampleOffset, long ClockTime)
{
    public int WholeSamples => (int)(SampleOffset >> 32);
}

public readonly record struct AlSecOffsetLatency(double SecOffset, double Latency);

public readonly record struct AlSecOffsetClock(double SecOffset, double ClockTime);

public readonly partial struct AlSource
{
    public AlSecOffsetLatency SecOffsetLatency
    {
        get
        {
            var (offset, latency) = Al.GetSourcedv2SOFT(Name, Al.Constants.AL_SEC_OFFSET_LATENCY_SOFT);
            return new AlSecOffsetLatency(offset, latency);
        }
    }

    public AlSampleOffsetLatency SampleOffsetLatency
    {
        get
        {
            var (offset, latency) = Al.GetSourcei64v2SOFT(Name, Al.Constants.AL_SAMPLE_OFFSET_LATENCY_SOFT);
            return new AlSampleOffsetLatency(offset, latency);
        }
    }

    public AlSecOffsetClock SecOffsetClock
    {
        get
        {
            var (offset, clock) = Al.GetSourcedv2SOFT(Name, Al.Constants.AL_SEC_OFFSET_CLOCK_SOFT);
            return new AlSecOffsetClock(offset, clock);
        }
    }

    public AlSampleOffsetClock SampleOffsetClock
    {
        get
        {
            var (offset, clock) = Al.GetSourcei64v2SOFT(Name, Al.Constants.AL_SAMPLE_OFFSET_CLOCK_SOFT);
            return new AlSampleOffsetClock(offset, clock);
        }
    }

    public void PlayAt(long deviceClockTime) => Al.PInvoke.SourcePlayAtTimeSOFT(Name, deviceClockTime);
}
