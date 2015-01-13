using Bond.IO.Safe;
using Bond.Protocols;

namespace Bond.PerformanceTest
{
    public static class BondEx
    {
        public static int CompactBondSize<T>(this T data)
        {
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data);
            return output.Data.Count;
        }

        public static int FastBondSize<T>(this T data)
        {
            var output = new OutputBuffer();
            var writer = new FastBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data);
            return output.Data.Count;

        }
    }
}
