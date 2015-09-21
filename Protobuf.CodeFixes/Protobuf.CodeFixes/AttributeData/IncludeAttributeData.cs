using Microsoft.CodeAnalysis;

namespace Protobuf.CodeFixes.AttributeData
{
    public abstract class IncludeAttributeData : ProtobufAttributeData
    {
        protected IncludeAttributeData(INamedTypeSymbol includedType)
        {
            IncludedType = includedType;
        }

        public INamedTypeSymbol IncludedType { get; }
    }
}