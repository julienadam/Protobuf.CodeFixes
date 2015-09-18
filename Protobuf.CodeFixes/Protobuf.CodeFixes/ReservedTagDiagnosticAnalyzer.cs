using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReservedTagDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public override string Title => "Protobuf-net code fixes : tag is in the reserved range [19000-19999]";
        public override string MessageFormat => "ProtoBuf tag set to reserved value {0} on {1}";
        public override string Description => "The range from 19000 to 19999 is reserved for internal Protocol Buffers implementation and must not be used.";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, IEnumerable<IncludeAttributeData> includeTags, IEnumerable<ProtobufAttributeData> memberTags)
        {
            foreach (var tag in memberTags.Where(a => a.Tag >= 19000 && a.Tag <= 19999))
            {
                context.ReportDiagnostic(Diagnostic.Create(GetDescriptor(), tag.GetLocation(), tag.Tag, tag.Symbol.Name));
            }
        }
    }
}
