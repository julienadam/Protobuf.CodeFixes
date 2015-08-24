using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

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