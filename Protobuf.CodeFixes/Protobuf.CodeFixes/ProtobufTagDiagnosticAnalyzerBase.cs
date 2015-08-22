using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

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
            var protobufAttribute = context.Symbol.GetProtobufAttributeData();
            if (protobufAttribute == null)
            {
                return;
            }

            AnalyzeSymbolCore(protobufAttribute, context);
        }

        protected abstract void AnalyzeSymbolCore(IEnumerable<ProtobufAttributeData> protobufAttributes, SymbolAnalysisContext context);

    }
}