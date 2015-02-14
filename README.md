## Comparison of [Microsoft.Bond](https://github.com/Microsoft/bond) and [Google Protocol Buffers](https://developers.google.com/protocol-buffers) performance

### Simle data type

A simple `Person` data type was declared in both Bond and Protocol Buffers. `Person` has some string and int fields, nested types and repeated fields. 
Here is a comparison of `Fast`/`Compact` Bond binary formats vs [Google.ProtocolBuffers](https://code.google.com/p/protobuf-csharp-port) serialization and deserialization time as well as message size:

```
Person: Protocol Buffers serialization time - 00:00:00.0000016
Person: Protocol Buffers output size - 154
Person: Protocol Buffers deserialization time - 00:00:00.0000011

Person: Bond compact serialization time - 00:00:00.0000009
Person: Bond compact output size - 155
Person: Bond compact deserialization time - 00:00:00.0000007

Person: Bond fast serialization time - 00:00:00.0000009
Person: Bond fast output size - 179
Person: Bond fast deserialization time - 00:00:00.0000008
```