using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufDuplicateTagDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Protobuf-net code fixes : duplicate tag";
        public const string Title = "Protobuf-net code fixes : duplicate tag";
        public const string MessageFormat = "Duplicate tag {0} on {1}";
        public const string Description = "The Protocol Buffers specifications forbid using the same tag more than once";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var type = (INamedTypeSymbol) context.Symbol;
            var members = type.GetMembers();
            var groupedByTag = members
                .SelectMany(m => m.GetProtobufAttributeData())
                .Where(a => a != null)
                .GroupBy(m => m.Tag);

            foreach (var group in groupedByTag.Where(g=>g.Count() > 1))
            {
                var symbolList = string.Join(", ", group.Select(g => g.Symbol.Name));

                foreach (var a in group)
                {
                    var diagnostic = Diagnostic.Create(Rule, a.GetLocation(), a.Tag, symbolList);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
