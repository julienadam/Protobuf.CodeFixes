using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Protobuf.CodeFixes
{
    public static class SymbolExtensions
    {
        public static ProtobufAttributeData GetProtoMemberAttributeData(this ISymbol symbol)
        {
            var attributes = symbol.GetAttributes();
            foreach (var attributeData in attributes)
            {
                if (attributeData.AttributeClass.Name != "ProtoMemberAttribute")
                {
                    continue;
                }

                var arg = attributeData.ConstructorArguments[0];
                // TODO: resolve static references, consts etc ?
                if (arg.Value is int)
                {
                    return new ProtobufAttributeData
                    {
                        AttributeData = attributeData,
                        Tag = (int) arg.Value,
                        Symbol = symbol,
                    };
                }
            }

            return null;
        }

        public static Location GetFirstArgumentLocation(this AttributeData attributeData)
        {
            var attributeSyntax = (AttributeSyntax)attributeData.ApplicationSyntaxReference.GetSyntax();
            return attributeSyntax.ArgumentList.Arguments[0].GetLocation();
        }
    }
}