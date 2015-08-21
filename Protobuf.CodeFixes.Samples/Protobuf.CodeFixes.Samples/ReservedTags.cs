using ProtoBuf;

namespace CodeFixes.Samples
{
    public class ReservedTags
    {
        [ProtoMember(19000)]
        public int MyProperty { get; set; }

        [ProtoMember(19999)]
        public int MyField;
    }
}
