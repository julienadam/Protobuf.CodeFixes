using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    public class ZeroTagDataMemberField
    {
        [DataMember(Order = 0)]
        public int MyField;
    }
}