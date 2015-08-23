using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtoMemberDataMemberTagMismatchDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : mismatch between tag on DataMember and tag on ProtoMember";
        public const string Title = "Protobuf-net code fixes : mismatch between tag on DataMember and tag on ProtoMember";
        public const string MessageFormat = "Mismatch on {0}: ProtoMember tag {1} vs DataMember tag {2}";
        public const string Description = "If you are setting both ProtoMember and DataMember, they should have the same tag";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(IEnumerable<ProtobufAttributeData> attributes, SymbolAnalysisContext context)
        {
            var list = attributes as IList<ProtobufAttributeData> ?? attributes.ToList();
            var protoMemberAttributes = list.Where(a => a is ProtoMemberAttributeData).Cast<ProtoMemberAttributeData>().ToList();
            var dataMemberAttributes = list.Where(a => a is DataMemberAttributeData).Cast<DataMemberAttributeData>().ToList();

            foreach (var protoAttributeData in protoMemberAttributes)
            {
                foreach(var mismatchedDataMember in dataMemberAttributes.Where(d => d.Tag != protoAttributeData.Tag))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Rule, 
                        protoAttributeData.GetLocation(),
                        context.Symbol.Name, 
                        protoAttributeData.Tag, 
                        mismatchedDataMember.Tag));
                    context.ReportDiagnostic(Diagnostic.Create(
                        Rule, 
                        mismatchedDataMember.GetLocation(),
                        context.Symbol.Name, 
                        protoAttributeData.Tag, 
                        mismatchedDataMember.Tag));
                }
            }
        }
    }
}
