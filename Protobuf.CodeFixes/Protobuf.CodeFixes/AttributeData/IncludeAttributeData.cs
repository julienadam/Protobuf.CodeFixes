using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public abstract class IncludeAttributeData : ProtobufAttributeData
    {
    }

    public class ProtoIncludeAttributeData : IncludeAttributeData
    {
        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }
    }
}