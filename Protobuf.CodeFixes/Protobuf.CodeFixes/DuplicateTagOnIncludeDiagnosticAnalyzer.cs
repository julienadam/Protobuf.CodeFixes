using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DuplicateTagOnIncludeDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : duplicate tag on includes";
        public override string Title => "Protobuf-net code fixes : duplicate tag on includes";
        public override string MessageFormat => "Duplicate ProtoInclude tag {0} on {1}";
        public override string Description => "The Protocol Buffers specifications forbid using the same tag more than once, including tags used for subtypes";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, IEnumerable<IncludeAttributeData> includeTags, IEnumerable<ProtobufAttributeData> memberTags)
        {
            var type = (INamedTypeSymbol)context.Symbol;
            var attributes = type.GetIncludeAttributeData();
            var groupedByTag = attributes.GroupBy(a => a.Tag).ToList();
            foreach (var group in groupedByTag.Where(g => g.Count() > 1))
            {
                //TODO: check the type, if all tags point to the same type, it's ok
                foreach (var a in group)
                {
                    var diagnostic = Diagnostic.Create(GetDescriptor(), a.GetLocation(), a.Tag, type.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
