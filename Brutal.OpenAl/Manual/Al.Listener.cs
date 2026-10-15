namespace Brutal.OpenAlApi;

public static unsafe partial class Al
{
    public static partial class Listener
    {
        public static (float3 At, float3 Up) Orientation
        {
            get
            {
                var v = stackalloc float[6];
                PInvoke.GetListenerfv(Constants.AL_ORIENTATION, v);
                return (new float3(v[0], v[1], v[2]), new float3(v[3], v[4], v[5]));
            }
            set
            {
                var v = stackalloc float[6] { value.At.X, value.At.Y, value.At.Z, value.Up.X, value.Up.Y, value.Up.Z };
                PInvoke.Listenerfv(Constants.AL_ORIENTATION, v);
            }
        }
    }
}
