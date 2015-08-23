using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    [ProtoContract]
    public class ReservedTagsProto
    {
        [ProtoMember(19000)]
        public int MyProperty { get; set; }

        [ProtoMember(19999)]
        public int MyField;
    }

    [DataContract]
    public class ReservedTagsData
    {
        [DataMember(Order = 19000)]
        public int MyProperty { get; set; }

        [DataMember(Order = 19999)]
        public int MyField;
    }
}
