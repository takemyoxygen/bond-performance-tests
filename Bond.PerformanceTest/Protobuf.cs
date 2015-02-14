using System;
using Google.ProtocolBuffers;

namespace Bond.PerformanceTest
{
    public static class Protobuf
    {
        public static void SerializeProtobuf<T>(Func<T> data) where T : IMessage
        {
            Action serialize = () => data().ToByteArray();

            var avg = serialize.Call(Settings.Times, Settings.WarmUp).Average();

            Console.WriteLine("{0}: Protocol Buffers serialization time - {1}", typeof(T).Name, avg);
            Console.WriteLine("{0}: Protocol Buffers output size - {1}", typeof(T).Name, data().SerializedSize);
        }

        public static void DeserializeProtobufPerson<T>(Func<T> data, Func<byte[], T> parse) where T : IMessage
        {
            var serialized = data().ToByteArray();

            Action deserialise = () => parse(serialized);

            var avg = deserialise.Call(Settings.Times, Settings.WarmUp).Average();

            Console.WriteLine("Person: Protocol Buffers deserialization time - " + avg);
            Console.WriteLine();
        }
    }
}