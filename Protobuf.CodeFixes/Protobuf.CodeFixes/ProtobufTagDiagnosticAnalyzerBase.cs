using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    public abstract class ProtobufTagDiagnosticAnalyzerBase : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Field);
        }

        protected void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var attributes = context.Symbol.GetAttributes();
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
                    AnalyzeSymbolCore((int) arg.Value, attributeData, context);
                }
            }
        }

        protected abstract void AnalyzeSymbolCore(int value, AttributeData attributeData, SymbolAnalysisContext context);

        protected Location GetTagArgumentLocation(AttributeData attributeData)
        {
            var attributeSyntax = (AttributeSyntax) attributeData.ApplicationSyntaxReference.GetSyntax();
            return attributeSyntax.ArgumentList.Arguments[0].GetLocation();
        }
    }
}