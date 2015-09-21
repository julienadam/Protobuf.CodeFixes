using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public class ProtoContractAttributeData : ContractAttributeData
    {
        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }
    }
}