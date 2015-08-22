using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    public static class SymbolExtensions
    {
        public static IEnumerable<ProtobufAttributeData> GetProtobufAttributeData(this ISymbol symbol)
        {
            var attributes = symbol.GetAttributes();
            foreach (var attributeData in attributes)
            {
                switch (attributeData.AttributeClass.Name)
                {
                    case "ProtoMemberAttribute":
                    {
                        if (attributeData.ConstructorArguments.Length == 0)
                        {
                            continue;
                        }

                        var arg = attributeData.ConstructorArguments[0];
                        // TODO: resolve static references, consts etc ?
                        if (arg.Value is int)
                        {
                            yield return new ProtoMemberAttributeData
                            {
                                AttributeData = attributeData,
                                Tag = (int) arg.Value,
                                Symbol = symbol,
                            };
                        }
                    }
                        break;
                    case "DataMemberAttribute":
                    {
                        foreach (var namedArg in attributeData.NamedArguments)
                        {
                            // TODO: resolve static references, consts etc ?
                            if (namedArg.Key == "Order" && namedArg.Value.Value is int)
                            {
                                yield return new DataMemberAttributeData
                                {
                                    AttributeData = attributeData,
                                    Tag = (int)namedArg.Value.Value,
                                    Symbol = symbol,
                                };
                            }
                        }
                    }
                        break;
                }
            }
        }

        public static Location GetFirstArgumentLocation(this Microsoft.CodeAnalysis.AttributeData attributeData)
        {
            var attributeSyntax = (AttributeSyntax)attributeData.ApplicationSyntaxReference.GetSyntax();
            return attributeSyntax.ArgumentList.Arguments[0].GetLocation();
        }
    }
}