using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufReservedTagDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public const string Title = "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public const string MessageFormat = "ProtoBuf tag set to reserved value {0} on {1}";
        public const string Description = "The range from 19000 to 19999 is reserved for internal Protocol Buffers implementation and must not be used.";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(int value, AttributeData attributeData, SymbolAnalysisContext context)
        {
            if (value >= 19000 && value <= 19999)
            {
                var attributeSyntax = (AttributeSyntax)attributeData.ApplicationSyntaxReference.GetSyntax();
                var diagnostic = Diagnostic.Create(Rule, attributeSyntax.ArgumentList.Arguments[0].GetLocation(), value, context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
