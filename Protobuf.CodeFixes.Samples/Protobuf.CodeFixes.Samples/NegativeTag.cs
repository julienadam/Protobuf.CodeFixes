using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    [DataContract]
    public class NegativeTagData
    {
        [DataMember(Order = -1)]
        public int MyField;

        [DataMember(Order = -2)]
        public int MyProperty { get; set; }
    }

    [ProtoContract]
    public class NegativeTagProto
    {
        [ProtoMember(-1)]
        public int MyField;

        [ProtoMember(-2)]
        public int MyProperty { get; set; }
    }
}