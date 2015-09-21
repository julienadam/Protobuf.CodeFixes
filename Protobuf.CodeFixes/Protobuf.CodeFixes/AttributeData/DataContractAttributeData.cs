using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public class DataContractAttributeData : ContractAttributeData
    {
        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }
    }
}