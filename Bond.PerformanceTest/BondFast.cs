using System;
using Bond.IO.Safe;
using Bond.Protocols;

namespace Bond.PerformanceTest
{
    public static class BondFast
    {
        public static void SerializeFast<T>(Func<T> data)
        {
            var serializer = new Serializer<FastBinaryWriter<OutputBuffer>>(typeof(T));

            Action serialize = () =>
            {
                var p = data();

                var output = new OutputBuffer(256);
                var writer = new FastBinaryWriter<OutputBuffer>(output);
                serializer.Serialize(p, writer);
            };

            var avg = serialize.Call(Settings.Times, Settings.WarmUp).Average();

            Console.WriteLine("{0}: Bond fast serialization time - {1}", typeof(T).Name, avg);
            Console.WriteLine("{0}: Bond fast output size - {1}", typeof(T).Name, Size(data()));
        }

        public static void DeserializeFast<T>(Func<T> data)
        {
            var output = new OutputBuffer();
            var writer = new FastBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data());

            var deserializer = new Deserializer<FastBinaryReader<InputBuffer>>(typeof(T));

            Action deserialize = () =>
            {
                var input = new InputBuffer(output.Data);
                var reader = new FastBinaryReader<InputBuffer>(input);
                deserializer.Deserialize<T>(reader);
            };

            var avg = deserialize.Call(Settings.Times, Settings.WarmUp).Average();
            Console.WriteLine("{0}: Bond fast deserialization time - {1}", typeof(T).Name, avg);
            Console.WriteLine();
        }

        public static int Size<T>(T data)
        {
            var output = new OutputBuffer();
            var writer = new FastBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data);
            return output.Data.Count;

        }
    }
}