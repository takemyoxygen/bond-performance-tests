using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Bond.IO.Safe;
using Bond.Protocols;

namespace Bond.PerformanceTest
{
    [Config(typeof(BenchmarksConfig))]
    public class DeserializationBenchmarks
    {
        private byte[] googleProtobufPersonBytes;
        private byte[] protobufNetPersonBytes;

        private readonly Deserializer<CompactBinaryReader<InputBuffer>> compactBondDeserializer = new Deserializer<CompactBinaryReader<InputBuffer>>(typeof(Bond.Person));
        private readonly Deserializer<FastBinaryReader<InputBuffer>> fastBondDeserializer = new Deserializer<FastBinaryReader<InputBuffer>>(typeof(Bond.Person));
        private byte[] compactBondPersonBytes;
        private byte[] fastBondPersonBytes;

        [Setup]
        public void Setup()
        {
            googleProtobufPersonBytes = BenchmarksData.ProtobufPerson().ToByteArray();

            var protobufNetStream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(protobufNetStream, BenchmarksData.ProtobufNetPerson());
            protobufNetPersonBytes = protobufNetStream.ToArray();

            var bondPerson = BenchmarksData.BondPerson();

            var compactOutput = new OutputBuffer(256);
            var compactWriter = new CompactBinaryWriter<OutputBuffer>(compactOutput);
            var compactBondSerializer = new Serializer<CompactBinaryWriter<OutputBuffer>>(typeof(Bond.Person));
            compactBondSerializer.Serialize(bondPerson, compactWriter);
            compactBondPersonBytes = compactOutput.Data.ToArray();

            var fastOutput = new OutputBuffer(256);
            var fastWriter = new FastBinaryWriter<OutputBuffer>(fastOutput);
            var fastBondSerializer = new Serializer<FastBinaryWriter<OutputBuffer>>(typeof(Bond.Person));
            fastBondSerializer.Serialize(bondPerson, fastWriter);
            fastBondPersonBytes = fastOutput.Data.ToArray();
        }

        [Benchmark]
        public void GoogleProtobuf()
        {
            Person.ParseFrom(googleProtobufPersonBytes);
        }

        [Benchmark]
        public void ProtobufNet()
        {
            var memoryStream = new MemoryStream(protobufNetPersonBytes);
            ProtoBuf.Serializer.Deserialize<ProtobufNet.Person>(memoryStream);
        }

        [Benchmark]
        public void CompactBond()
        {
            var input = new InputBuffer(compactBondPersonBytes);
            var reader = new CompactBinaryReader<InputBuffer>(input);
            compactBondDeserializer.Deserialize<Bond.Person>(reader);
        }

        [Benchmark]
        public void FastBond()
        {
            var input = new InputBuffer(fastBondPersonBytes);
            var reader = new FastBinaryReader<InputBuffer>(input);
            fastBondDeserializer.Deserialize<Bond.Person>(reader);
        }
    }
}