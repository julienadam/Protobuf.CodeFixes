using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NegativeTagDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : tag is negative";
        public override string Title => "Protobuf-net code fixes : tag is negative";
        public override string MessageFormat => "ProtoBuf tag set to a negative value on {0}";
        public override string Description => "The Protocol Buffers specifications forbid using negative values for a tag identifier";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags, List<ContractAttributeData> contractAttributes)
        {
            foreach (var tag in memberTags.Where(a => a.Tag < 0))
            {
                context.ReportDiagnostic(Diagnostic.Create(GetDescriptor(), tag.GetLocation(), tag.Symbol.Name));
            }
        }
    }
}
