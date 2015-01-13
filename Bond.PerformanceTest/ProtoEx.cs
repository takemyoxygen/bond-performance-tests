using Google.ProtocolBuffers;

namespace Bond.PerformanceTest
{
    public static class ProtoEx
    {
        public static int SerializedBondSize<T>(this T item) where T : IMessage
        {
            return item.SerializedSize;
        }
    }
}
