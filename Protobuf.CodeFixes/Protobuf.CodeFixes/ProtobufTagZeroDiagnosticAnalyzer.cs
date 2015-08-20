using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufTagZeroDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : tag is zero";
        public const string Title = "Protobuf-net code fixes : tag is zero";
        public const string MessageFormat = "ProtoBuf tag set to 0 on {0}";
        public const string Description = "The Protocol Buffers specifications forbid using 0 for a tag identifier";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(int value, AttributeData attributeData, SymbolAnalysisContext context)
        {
            if (value == 0)
            {
                var diagnostic = Diagnostic.Create(Rule, GetTagArgumentLocation(attributeData), context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
