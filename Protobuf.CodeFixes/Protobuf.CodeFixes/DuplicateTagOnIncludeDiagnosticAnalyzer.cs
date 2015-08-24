using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DuplicateTagOnIncludeDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Protobuf-net code fixes : duplicate tag on includes";
        public const string Title = "Protobuf-net code fixes : duplicate tag on includes";
        public const string MessageFormat = "Duplicate ProtoInclude / KnownType tag {0} on {1}";
        public const string Description = "The Protocol Buffers specifications forbid using the same tag more than once, including tags used for subtypes";
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
            var attributes = type.GetIncludeAttributeData();
            var groupedByTag = attributes.GroupBy(a => a.Tag).ToList();
            foreach (var group in groupedByTag.Where(g => g.Count() > 1))
            {
                //TODO: check the type, if all tags point to the same type, it's ok
                foreach (var a in group)
                {
                    var diagnostic = Diagnostic.Create(Rule, a.GetLocation(), a.Tag, type.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
