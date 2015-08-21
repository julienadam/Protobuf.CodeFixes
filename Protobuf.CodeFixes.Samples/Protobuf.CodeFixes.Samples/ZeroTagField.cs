using ProtoBuf;

namespace CodeFixes.Samples
{
    public class ZeroTagField
    {
        [ProtoMember(0)]
        public int MyField;
    }
}
