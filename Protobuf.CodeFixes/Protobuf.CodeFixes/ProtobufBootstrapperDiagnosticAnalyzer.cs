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
        public ProtobufDiagnosticAnalyzerBase[] Analyzers { get; }

        public ProtobufBootstrapperDiagnosticAnalyzer()
        {
            Analyzers = new ProtobufDiagnosticAnalyzerBase[]
            {
                // TODO: load all analyzers
            };
        }

        public ProtobufBootstrapperDiagnosticAnalyzer(params ProtobufDiagnosticAnalyzerBase[] analyzers)
        {
            this.Analyzers = analyzers;
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

            // Load all tag data
            var contractAttributes = namedTypeSymbol.GetContractAttributeData().ToList();
            var includeTags = namedTypeSymbol.GetIncludeAttributeData().ToList();
            var memberTags = namedTypeSymbol.GetMembersAttributeDate().ToList();

            foreach (var analyzer in Analyzers)
            {
                analyzer.Analyze(context, includeTags, memberTags, contractAttributes);
            }
        }


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
    }
}