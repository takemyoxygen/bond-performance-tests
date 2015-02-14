using System;
using System.Collections.Generic;
using Bond.IO.Safe;
using Bond.Protocols;

namespace Bond.PerformanceTest
{
    class Program
    {
        private const int Times = 1000000;

        private const int WarmUp = 100;

        static void Main(string[] args)
        {
            SerializeProtobufPerson();
            DeserializeProtobufPerson();

            SerializeBondPersonCompactly();
            DeserializeBondPersonCompactly();

            SerializeBondPersonQuickly();
            DeserializeBondPersonQuickly();

            Console.ReadLine();
        }

        private static void SerializeBondPersonCompactly()
        {
            var serializer = new Serializer<CompactBinaryWriter<OutputBuffer>>(typeof (Bond.Person));

            Action serialize = () =>
            {
                var p = CreateBondPerson();

                var output = new OutputBuffer(256);
                var writer = new CompactBinaryWriter<OutputBuffer>(output);
                serializer.Serialize(p, writer);
            };

            var timeSpan = serialize.Call(Times, WarmUp).Average();

            Console.WriteLine("Person: Bond compact serialization time - " + timeSpan);
            Console.WriteLine("Person: Bond compact output size -" + CreateBondPerson().CompactBondSize());
        }

        private static void DeserializeBondPersonCompactly()
        {
            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, CreateBondPerson());

            var deserializer = new Deserializer<CompactBinaryReader<InputBuffer>>(typeof(Bond.Person));

            Action deserialize = () =>
            {
                var input = new InputBuffer(output.Data);
                var reader = new CompactBinaryReader<InputBuffer>(input);
                deserializer.Deserialize<Bond.Person>(reader);
            };

            var average = deserialize.Call(Times, WarmUp).Average();
            Console.WriteLine("Person: Bond cpmpact deserialization time - " + average);
            Console.WriteLine();
        }

        private static void SerializeBondPersonQuickly()
        {
            var serializer = new Serializer<FastBinaryWriter<OutputBuffer>>(typeof(Bond.Person));

            Action serialize = () =>
            {
                var person = CreateBondPerson();

                var output = new OutputBuffer(256);
                var writer = new FastBinaryWriter<OutputBuffer>(output);
                serializer.Serialize(person, writer);
            };

            var average = serialize.Call(Times, WarmUp).Average();

            Console.WriteLine("Person: Bond fast serialization time - " + average);
            Console.WriteLine("Person: Bond fast output size - " + CreateBondPerson().FastBondSize());
        }

        private static void DeserializeBondPersonQuickly()
        {
            var output = new OutputBuffer();
            var writer = new FastBinaryWriter<OutputBuffer>(output);
            Serialize.To(writer, CreateBondPerson());

            var deserializer = new Deserializer<FastBinaryReader<InputBuffer>>(typeof(Bond.Person));

            Action deserialize = () =>
            {
                var input = new InputBuffer(output.Data);
                var reader = new FastBinaryReader<InputBuffer>(input);
                deserializer.Deserialize<Bond.Person>(reader);
            };

            var average = deserialize.Call(Times, WarmUp).Average();
            Console.WriteLine("Person: Bond fast deserialization time - " + average);
            Console.WriteLine();
        }

        private static void SerializeProtobufPerson()
        {
            Action serialize = () => CreateProtobufPerson().ToByteArray();

            var timeSpan = serialize.Call(Times, WarmUp).Average();

            Console.WriteLine("Person: Protocol Buffers serialization time - " + timeSpan);
            Console.WriteLine("Person: Protocol Buffers output size - " + CreateProtobufPerson().SerializedSize);
        }

        private static void DeserializeProtobufPerson()
        {
            var serialised = CreateProtobufPerson().ToByteArray();

            Action deserialize = () => Person.ParseFrom(serialised);

            var average = deserialize.Call(Times, WarmUp).Average();

            Console.WriteLine("Person: Protocol Buffers deserialization time - " + average);
            Console.WriteLine();
        }

        private static Bond.Person CreateBondPerson()
        {
            return new Bond.Person
            {
                firstName = "Test",
                lastName = "Person",
                age = 22,
                gender = Bond.Gender.Male,
                policeRecords = new List<Bond.PoliceRecord>
                {
                    new Bond.PoliceRecord
                    {
                        crime = "crime1",
                        id = 1
                    },

                    new Bond.PoliceRecord
                    {
                        crime = "some crime with very very very very very long description. asldg ;askdg a;sdgjk asdgjahsd asd gasd asdjgahsl dgasldjgh",
                        id = 2
                    }
                }
            };
        }

        private static Person CreateProtobufPerson()
        {
            return Person
                .CreateBuilder()
                .SetFirstName("Test")
                .SetLastName("Person")
                .SetAge(22)
                .SetGender(Gender.Male)
                .AddPoliceRecords(PoliceRecord
                    .CreateBuilder()
                    .SetCrime("crime1")
                    .SetId(1))
                .AddPoliceRecords(PoliceRecord
                    .CreateBuilder()
                    .SetCrime("some crime with very very very very very long description. asldg ;askdg a;sdgjk asdgjahsd asd gasd asdjgahsl dgasldjgh")
                    .SetId(2))
                .Build();
        }
    }
}
