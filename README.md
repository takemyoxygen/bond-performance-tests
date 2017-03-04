## Comparison of [Microsoft.Bond](https://github.com/Microsoft/bond) and [Google Protocol Buffers](https://developers.google.com/protocol-buffers) performance

### Simle data type

A simple `Person` data type was declared in both Bond and Protocol Buffers. `Person` has some string and int fields, nested types and repeated fields. 
Below is a comparison of `Fast`/`Compact` Bond binary formats vs [Google.ProtocolBuffers](https://code.google.com/p/protobuf-csharp-port) and [protofub-net](https://github.com/mgravell/protobuf-net)

System configuration:
``` ini

BenchmarkDotNet=v0.10.1, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-4870HQ CPU 2.50GHz, ProcessorCount=4
Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1080.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1080.0

```
Serialization:

         Method |        Mean |     StdDev |       Op/s | Rank |  Gen 0 | Allocated |
--------------- |------------ |----------- |----------- |----- |------- |---------- |
 GoogleProtobuf | 653.0290 ns |  5.9276 ns | 1531325.47 |    1 | 0.0482 |     456 B |
    ProtobufNet | 891.4352 ns | 14.0410 ns | 1121786.48 |    4 | 0.0669 |     720 B |
    CompactBond | 691.7178 ns |  8.1182 ns | 1445676.21 |    2 | 0.0253 |     312 B |
       FastBond | 752.5360 ns | 15.7623 ns | 1328840.02 |    3 | 0.0238 |     312 B |

Deserialization:

         Method |          Mean |     StdDev |       Op/s | Rank |  Gen 0 | Allocated |
--------------- |-------------- |----------- |----------- |----- |------- |---------- |
 GoogleProtobuf | 1,015.6574 ns | 12.9840 ns |  984583.95 |    2 | 0.1088 |     976 B |
    ProtobufNet | 1,772.1160 ns | 13.8147 ns |  564297.15 |    3 | 0.0679 |     704 B |
    CompactBond |   636.9526 ns | 18.3007 ns | 1569975.56 |    1 | 0.0713 |     624 B |
       FastBond |   645.8444 ns |  9.3764 ns | 1548360.45 |    1 | 0.0749 |     624 B |