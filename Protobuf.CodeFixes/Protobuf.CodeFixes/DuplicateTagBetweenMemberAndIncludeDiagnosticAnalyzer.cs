using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Protobuf-net code fixes : duplicate tag between member and include tags";
        public const string Title = "Protobuf-net code fixes : duplicate tag between member and include tags";
        public const string MessageFormat = "Duplicate tag {0} on {1}: {2}";

        public const string Description =
            "The Protocol Buffers specifications forbid using the same tag more than once, including tags used for subtypes";

        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var type = (INamedTypeSymbol) context.Symbol;
            var include = type.GetIncludeAttributeData().ToList();

            // Bail early if no include
            if (include.Any())
            {
                // Get full list of attributes, member and include
                var members = type.GetMembers()
                    .SelectMany(m => m.GetMemberAttributeData())
                    .Concat(include)
                    .Where(a => a != null);

                // Group it by tag
                var groupedByTag = members
                    .GroupBy(m => m.Tag)
                    .ToList();

                // Any group with more than one element is suspicious
                foreach (var group in groupedByTag.Where(g => g.Count() > 1))
                {
                    // Any group with an include means an error
                    if (group.Any(a => a is ProtoIncludeAttributeData))
                    {
                        var symbolList = string.Join(", ", group.Select(g => g.GetRelevantSymbolName()));
                        foreach (var a in group)
                        {
                            var diagnostic = Diagnostic.Create(Rule, a.GetLocation(), a.Tag, type.Name, symbolList);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
        }
    }
}
