using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DuplicateTagDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : duplicate tag";
        public override string Title => "Protobuf-net code fixes : duplicate tag";
        public override string MessageFormat => "Duplicate tag {0} on {1}";
        public override string Description => "The Protocol Buffers specifications forbid using the same tag more than once";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags, List<ContractAttributeData> contractAttributes)
        {
            var groupedByTag = memberTags
               .GroupBy(m => m.Tag);

            foreach (var group in groupedByTag.Where(g => g.Count() > 1))
            {
                // If all the tags are on the same symbol, skip, it's not an error
                if (group.Select(g => g.Symbol).Distinct().Count() == 1)
                {
                    continue;
                }

                var symbolList = string.Join(", ", group.Select(g => g.Symbol.Name));

                foreach (var a in group)
                {
                    var diagnostic = Diagnostic.Create(GetDescriptor(), a.GetLocation(), a.Tag, symbolList);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
