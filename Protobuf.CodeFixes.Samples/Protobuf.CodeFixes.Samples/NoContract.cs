using ProtoBuf;
using System.Runtime.Serialization;

namespace CodeFixes.Samples
{
    class NoContractData
    {
        [DataMember(Order = 1)]
        public int MyField;
    }

    class NoContractProto
    {
        [ProtoMember(1)]
        public int MyField;
    }
}
