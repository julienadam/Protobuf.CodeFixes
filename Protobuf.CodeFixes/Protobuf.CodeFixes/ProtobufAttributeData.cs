using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes
{
    public class ProtobufAttributeData
    {
        public int Tag { get; set; }
        public AttributeData AttributeData { get; set; }
        public ISymbol Symbol { get; set; }
    }
}