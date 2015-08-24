using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Protobuf.CodeFixes.AttributeData
{
    public class DataMemberAttributeData : MemberAttributeData
    {
        public override Location GetLocation()
        {
            var attributeSyntax = (AttributeSyntax)AttributeData.ApplicationSyntaxReference.GetSyntax();
            var orderArg = attributeSyntax.ArgumentList.Arguments.First(a => a.NameEquals.Name.Identifier.Text == "Order");
            return orderArg.Expression.GetLocation();
        }
    }
}