using System;
using Bond.IO.Safe;
using Bond.Protocols;

namespace Bond.PerformanceTest
{
    public static class BondCompact
    {
        public static void SerializeCompact<T>(Func<T> data)
        {
            var serializer = new Serializer<CompactBinaryWriter<OutputBuffer>>(typeof (T));

            Action serialize = () =>
            {
                var p = data();

                var output = new OutputBuffer(256);
                var writer = new CompactBinaryWriter<OutputBuffer>(output);
                serializer.Serialize(p, writer);
            };

            var avg = serialize.Call(Settings.Times, Settings.WarmUp).Average();

            Console.WriteLine("{0}: Bond compact serialization time - {1}", typeof(T).Name, avg);
            Console.WriteLine("{0}: Bond compact output size - {1}", typeof(T).Name, Size(data()));
        }

        public static void DeserializeCompact<T>(Func<T> data)
        {
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data());

            var deserializer = new Deserializer<CompactBinaryReader<InputBuffer>>(typeof(T));

            Action deserialize = () =>
            {
                var input = new InputBuffer(output.Data);
                var reader = new CompactBinaryReader<InputBuffer>(input);
                deserializer.Deserialize<T>(reader);
            };

            var avg = deserialize.Call(Settings.Times, Settings.WarmUp).Average();
            Console.WriteLine("{0}: Bond compact deserialization time - {1}", typeof(T).Name, avg);
            Console.WriteLine();
        }

        public static int Size<T>(T data)
        {
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, data);
            return output.Data.Count;
        }
    }
}