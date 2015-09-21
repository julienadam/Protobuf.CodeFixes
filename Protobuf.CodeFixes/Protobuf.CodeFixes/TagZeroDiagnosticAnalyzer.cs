using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TagZeroDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : tag is zero";
        public override string Title => "Protobuf-net code fixes : tag is zero";
        public override string MessageFormat => "ProtoBuf tag set to 0 on {0}";
        public override string Description => "The Protocol Buffers specifications forbid using 0 for a tag identifier";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags, List<ContractAttributeData> contractAttributes)
        {
            foreach (var tag in memberTags.Where(t => t.Tag == 0))
            {
                context.ReportDiagnostic(Diagnostic.Create(GetDescriptor(), tag.GetLocation(), tag.Symbol.Name));
            }
        }
    }
}
