using ProtoBuf;

namespace CodeFixes.Samples
{
    public class ZeroTagProtoMemberField
    {
        [ProtoMember(0)]
        public int MyField;
    }
}
