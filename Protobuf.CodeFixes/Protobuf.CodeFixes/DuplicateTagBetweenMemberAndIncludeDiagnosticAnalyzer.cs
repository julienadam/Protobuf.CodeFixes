using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : duplicate tag between member and include tags";
        public override string Title => "Protobuf-net code fixes : duplicate tag between member and include tags";
        public override string MessageFormat => "Duplicate tag {0} on {1}: {2}";
        public override string Description => "The Protocol Buffers specifications forbid using the same tag more than once, including tags used for subtypes";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags, List<ContractAttributeData> contractAttributes)
        {
            if (!includeTags.Any())
            {
                return;
            }

            var allTags = includeTags.Concat(memberTags);
           
            // Group it by tag
            var groupedByTag = allTags
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
                        var diagnostic = Diagnostic.Create(GetDescriptor(), a.GetLocation(), a.Tag, context.Symbol.Name, symbolList);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
