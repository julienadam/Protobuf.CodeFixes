using ProtoBuf;

namespace CodeFixes.Samples
{
    public class DuplicateTags
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
}
