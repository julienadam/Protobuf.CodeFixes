using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    [ProtoContract]
    public class DuplicateTagsProto
    {
        [ProtoMember(1)]
        public int MyProperty { get; set; }

        [ProtoMember(1)]
        public int MyField;

        [ProtoMember(1)]
        public int MySecondProperty { get; set; }

        [ProtoMember(1)]
        public int MySecondField;
    }

    [DataContract]
    public class DuplicateTagsData
    {
        [DataMember(Order = 1)]
        public int MyProperty { get; set; }

        [DataMember(Order = 1)]
        public int MyField;

        [DataMember(Order = 1)]
        public int MySecondProperty { get; set; }

        [DataMember(Order = 1)]
        public int MySecondField;
    }
}
