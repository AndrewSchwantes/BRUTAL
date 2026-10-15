namespace Brutal.OpenAlApi.Params;

public static class DeviceParams
{
    extension(AlcDevice device)
    {
        public ReadOnlySpan<byte> GetString(AlcGetString param) => Alc.GetStringUtf8(device, param);

        public int Get(AlcInteger param) => Alc.GetInteger(device, param);
    }

    extension(AlcCaptureDevice capture)
    {
        public ReadOnlySpan<byte> GetString(AlcGetString param) => Alc.GetStringUtf8(capture, param);

        public int Get(AlcInteger param) => Alc.GetInteger(capture, param);
    }
}
