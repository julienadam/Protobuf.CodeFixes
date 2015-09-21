using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Protobuf.CodeFixes.AttributeData
{
    public class ProtoIncludeAttributeData : IncludeAttributeData
    {
        public ProtoIncludeAttributeData(INamedTypeSymbol includedType) : base(includedType)
        {
        }

        public override Location GetLocation()
        {
            return AttributeData.GetFirstArgumentLocation();
        }


        public override string GetRelevantSymbolName()
        {
            var attributeSyntax = (AttributeSyntax)AttributeData.ApplicationSyntaxReference.GetSyntax();
            if (attributeSyntax.ArgumentList.Arguments.Count < 2)
            {
                return base.GetRelevantSymbolName();
            }
            var typeofExpression = attributeSyntax.ArgumentList.Arguments[1].Expression as TypeOfExpressionSyntax;
            if (typeofExpression == null)
            {
                return base.GetRelevantSymbolName();
            }

            return $"include({typeofExpression.Type.GetText()})";
        }
    }
}