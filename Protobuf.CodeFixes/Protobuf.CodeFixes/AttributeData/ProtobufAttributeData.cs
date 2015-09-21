using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public abstract class ProtobufAttributeData
    {
        public int? Tag { get; set; }
        public Microsoft.CodeAnalysis.AttributeData AttributeData { get; set; }
        public ISymbol Symbol { get; set; }

        public abstract Location GetLocation();

        public virtual string GetRelevantSymbolName()
        {
            return Symbol.Name;
        }
    }
}