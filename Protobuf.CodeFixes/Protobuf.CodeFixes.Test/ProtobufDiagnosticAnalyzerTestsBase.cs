using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.Test.Verifiers;
using ProtoBuf;
using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public abstract class ProtobufDiagnosticAnalyzerTestsBase<T> : DiagnosticVerifier where T: DiagnosticAnalyzer, new()
    {
        protected override IEnumerable<MetadataReference> GetAdditionalReferences()
        {
            yield return MetadataReference.CreateFromFile(typeof(ProtoContractAttribute).Assembly.Location);
            yield return MetadataReference.CreateFromFile(typeof(DataContractAttribute).Assembly.Location);
        }

        [Fact]
        public void No_diagnostic_on_empty()
        {
            VerifyCSharpDiagnostic(@"");
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new T();
        }

        protected abstract string DiagnosticId { get; }
        protected abstract string MessageFormat { get; }

        private DiagnosticResult GetExpectedResult(int line, int column, DiagnosticSeverity diagnosticSeverity, object[] formatParameters)
        {
            return new DiagnosticResult
            {
                Id = DiagnosticId,
                Message = string.Format(MessageFormat, formatParameters),
                Severity = diagnosticSeverity,
                Locations = new[] {new DiagnosticResultLocation("Test0.cs", line, column)}
            };
        }

        protected DiagnosticResult GetExpectedError(int line, int column, params object[] formatParameters)
        {
            return GetExpectedResult(line, column, DiagnosticSeverity.Error, formatParameters);
        }

        protected DiagnosticResult GetExpectedWarning(int line, int column, params object[] formatParameters)
        {
            return GetExpectedResult(line, column, DiagnosticSeverity.Warning, formatParameters);
        }
    }
}