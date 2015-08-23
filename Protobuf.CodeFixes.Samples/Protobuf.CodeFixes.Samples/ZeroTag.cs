using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    [DataContract]
    public class ZeroTagDataMemberField
    {
        [DataMember(Order = 0)]
        public int MyField;
    }

    [DataContract]
    public class ZeroTagDataMemberProperty
    {
        [DataMember(Order = 0)]
        public int MyProperty { get; set; }
    }

    [ProtoContract]
    public class ZeroTagProperty
    {
        [ProtoMember(0)]
        public int MyProperty { get; set; }
    }

    [ProtoContract]
    public class ZeroTagProtoMemberField
    {
        [ProtoMember(0)]
        public int MyField;
    }
}