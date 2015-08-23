using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public class ProtoMemberAttributeData : ProtobufAttributeData
    {
        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }
    }
}