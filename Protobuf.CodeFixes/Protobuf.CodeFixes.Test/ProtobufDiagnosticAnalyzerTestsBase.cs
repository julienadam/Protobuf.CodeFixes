using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Protobuf.CodeFixes.Test.Verifiers;
using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public abstract class ProtobufDiagnosticAnalyzerTestsBase<T> : DiagnosticVerifier where T: DiagnosticAnalyzer, new()
    {
        private const string Placeholder = "$tag$";
        private const string OneTagPropertyClassSource = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember("+ Placeholder + @")]
            public string SomeProperty { get; set; }
        }
    }";

        protected string GetOneTagPropertyClassSource(int tag)
        {
            return OneTagPropertyClassSource.Replace(Placeholder, tag.ToString());
        }

        private const string OneTagFieldClassSource = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(" + Placeholder + @")]
            public string SomeField;
        }
    }";

        protected string GetOneTagFieldClassSource(int tag)
        {
            return OneTagFieldClassSource.Replace(Placeholder, tag.ToString());
        }

        protected override IEnumerable<MetadataReference> GetAdditionalReferences()
        {
            yield return MetadataReference.CreateFromFile("protobuf-net.dll");
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

        protected DiagnosticResult GetExpectedError(params object[] formatParameters)
        {
            var expected = new DiagnosticResult
            {
                Id = DiagnosticId,
                Message = string.Format(MessageFormat, formatParameters),
                Severity = DiagnosticSeverity.Error,
                Locations = new[] {new DiagnosticResultLocation("Test0.cs", 8, 26)}
            };
            return expected;
        }
    }
}