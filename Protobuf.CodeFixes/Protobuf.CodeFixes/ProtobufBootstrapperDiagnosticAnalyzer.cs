using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.AttributeData;

namespace Protobuf.CodeFixes
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ProtobufBootstrapperDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private readonly ProtobufDiagnosticAnalyzerBase[] analyzers;

        public ProtobufBootstrapperDiagnosticAnalyzer()
        {
            analyzers = new ProtobufDiagnosticAnalyzerBase[]
            {
                // TODO: load all analyzers
            };
        }

        public ProtobufBootstrapperDiagnosticAnalyzer(params ProtobufDiagnosticAnalyzerBase[] analyzers)
        {
            this.analyzers = analyzers;
            SupportedDiagnostics = analyzers.Select(a => a.GetDescriptor()).ToImmutableArray();
        }


        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(analysisContext =>
            {
                if (analysisContext.Compilation.ReferencedAssemblyNames.Any(a => a.Name == "protobuf-net"))
                {
                    analysisContext.RegisterSymbolAction(Action, SymbolKind.NamedType);
                }
            });
        }

        private void Action(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = context.Symbol as INamedTypeSymbol;
            if (namedTypeSymbol == null)
            {
                return;
            }

            foreach (var analyzer in analyzers)
            {
                // Load all tag data
                var includeTags = context.Symbol.GetIncludeAttributeData();
                var memberTags = GetMemberTags(namedTypeSymbol);

                analyzer.Analyze(context, includeTags, memberTags);
            }
        }

        public IEnumerable<ProtobufAttributeData> GetMemberTags(INamedTypeSymbol type)
        {
            return (type.GetMembers()
                .Where(member => member is IFieldSymbol || member is IPropertySymbol)
                .SelectMany(member => member.GetMemberAttributeData()));
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
    }
}