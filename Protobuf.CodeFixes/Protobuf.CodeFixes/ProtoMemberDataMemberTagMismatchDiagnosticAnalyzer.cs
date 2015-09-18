using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtoMemberDataMemberTagMismatchDiagnosticAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "Protobuf-net code fixes : mismatch between tag on DataMember and tag on ProtoMember";
        public override string Title => "Protobuf-net code fixes : mismatch between tag on DataMember and tag on ProtoMember";
        public override string MessageFormat => "Mismatch on {0}: ProtoMember tag {1} vs DataMember tag {2}";
        public override string Description => "If you are setting both ProtoMember and DataMember, they should have the same tag";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        public override void Analyze(SymbolAnalysisContext context, IEnumerable<IncludeAttributeData> includeTags,
            IEnumerable<ProtobufAttributeData> memberTags)
        {
            var grouped = memberTags.GroupBy(t => t.Symbol);

            foreach (var tagPerSymbol in grouped)
            {
                var list = tagPerSymbol.ToList();
                var protoMemberAttributes = list.Where(a => a is ProtoMemberAttributeData).Cast<ProtoMemberAttributeData>().ToList();
                var dataMemberAttributes = list.Where(a => a is DataMemberAttributeData).Cast<DataMemberAttributeData>().ToList();

                foreach (var protoMemberTag in protoMemberAttributes)
                {
                    foreach (var mismatchedDataMember in dataMemberAttributes.Where(d => d.Tag != protoMemberTag.Tag))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            GetDescriptor(), 
                            protoMemberTag.GetLocation(),
                            protoMemberTag.Symbol.Name,
                            protoMemberTag.Tag,
                            mismatchedDataMember.Tag));
                        context.ReportDiagnostic(Diagnostic.Create(
                            GetDescriptor(),
                            mismatchedDataMember.GetLocation(),
                            mismatchedDataMember.Symbol.Name,
                            protoMemberTag.Tag,
                            mismatchedDataMember.Tag));
                    }
                }
            }
        }
    }
}
