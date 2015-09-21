using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    public static class SymbolExtensions
    {
        public static IEnumerable<ProtobufAttributeData> GetMembersAttributeDate(this INamedTypeSymbol symbol)
        {
            return (symbol.GetMembers()
                .Where(member => member is IFieldSymbol || member is IPropertySymbol)
                .SelectMany(member => member.GetMemberAttributeData()));
        }

        public static IEnumerable<ContractAttributeData> GetContractAttributeData(this INamedTypeSymbol symbol)
        {
            var attributes = symbol.GetAttributes();
            foreach (var attributeData in attributes)
            {
                switch (attributeData.AttributeClass.Name)
                {
                    case "ProtoContractAttribute":
                    {
                        yield return new ProtoContractAttributeData
                        {
                            AttributeData = attributeData,
                            Symbol = symbol,
                        };
                    }
                        break;
                    case "DataContractAttribute":
                    {
                        yield return new DataContractAttributeData
                        {
                            AttributeData = attributeData,
                            Symbol = symbol,
                        };
                    }
                        break;
                }
            }
        }

        public static IEnumerable<IncludeAttributeData> GetIncludeAttributeData(this INamedTypeSymbol symbol)

        {
            var attributes = symbol.GetAttributes();
            foreach (var attributeData in attributes)
            {
                switch (attributeData.AttributeClass.Name)
                {
                    case "ProtoIncludeAttribute":
                        if (attributeData.ConstructorArguments.Length < 2)
                        {
                            continue;
                        }

                        var tagArg = attributeData.ConstructorArguments[0];
                        var includedTypeArg = attributeData.ConstructorArguments[1];
                        
                        // TODO: resolve static references, consts etc ?
                        if (tagArg.Value is int)
                        {
                            INamedTypeSymbol includedType = null;
                            if (includedTypeArg.Kind == TypedConstantKind.Type)
                            {
                                includedType = includedTypeArg.Value as INamedTypeSymbol;
                            }
                            
                            yield return new ProtoIncludeAttributeData(includedType)
                            {
                                AttributeData = attributeData,
                                Tag = (int)tagArg.Value,
                                Symbol = symbol,
                            };
                        }
                        break;
                }
            }
        }

        public static IEnumerable<ProtobufAttributeData> GetMemberAttributeData(this ISymbol symbol)
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