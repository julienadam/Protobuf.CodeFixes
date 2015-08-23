using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TagZeroDiagnosticAnalyzer : ProtobufTagDiagnosticAnalyzerBase
    {
        public const string DiagnosticId = "Protobuf-net code fixes : tag is zero";
        public const string Title = "Protobuf-net code fixes : tag is zero";
        public const string MessageFormat = "ProtoBuf tag set to 0 on {0}";
        public const string Description = "The Protocol Buffers specifications forbid using 0 for a tag identifier";
        public const string Category = "Protobuf";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override void AnalyzeSymbolCore(IEnumerable<ProtobufAttributeData> attributes, SymbolAnalysisContext context)
        {
            foreach (var attributeData in attributes.Where(a => a.Tag == 0))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, attributeData.GetLocation(), context.Symbol.Name));
            }
        }
    }
}
