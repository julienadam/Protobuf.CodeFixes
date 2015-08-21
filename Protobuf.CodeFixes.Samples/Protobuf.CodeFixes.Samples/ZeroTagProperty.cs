using ProtoBuf;

namespace CodeFixes.Samples
{
    public class ZeroTagProperty
    {
        [ProtoMember(0)]
        public int MyProperty { get; set; }
    }
}
