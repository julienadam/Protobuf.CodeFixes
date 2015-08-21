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
            var protobufAttribute = context.Symbol.GetProtoMemberAttributeData();
            if (protobufAttribute == null)
            {
                return;
            }

            AnalyzeSymbolCore(protobufAttribute.Tag, protobufAttribute.AttributeData, context);
        }

        protected abstract void AnalyzeSymbolCore(int value, AttributeData attributeData, SymbolAnalysisContext context);

    }
}