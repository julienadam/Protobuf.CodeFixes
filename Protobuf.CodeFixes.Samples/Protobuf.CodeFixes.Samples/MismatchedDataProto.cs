using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    [ProtoContract]
    [DataContract]
    class MismatchedDataProto
    {
        [DataMember(Order = 1)]
        [ProtoMember(2)]
        public int AnInt { get; set; }
    }
}
