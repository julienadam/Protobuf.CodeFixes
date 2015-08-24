using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public class ProtoMemberAttributeData : MemberAttributeData
    {
        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }
    }
}