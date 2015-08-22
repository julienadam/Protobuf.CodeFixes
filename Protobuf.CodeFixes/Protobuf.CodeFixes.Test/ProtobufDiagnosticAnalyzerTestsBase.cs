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
        private const string Placeholder = "$tag$";
        private const string Placeholder2 = "$tag2$";
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


        private const string TwoTagClassSource = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(" + Placeholder + @")]
            public string SomeProperty { get; set; }

            [ProtoMember(" + Placeholder2 + @")]
            public string SomeField;
        }
    }";

        protected string GetTwoTagClassSource(int tag1, int tag2)
        {
            return TwoTagClassSource.Replace(Placeholder, tag1.ToString()).Replace(Placeholder2, tag2.ToString());
        }


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

        protected DiagnosticResult GetExpectedError(int line, int column, params object[] formatParameters)
        {
            return new DiagnosticResult
            {
                Id = DiagnosticId,
                Message = string.Format(MessageFormat, formatParameters),
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", line, column) }
            };
        }

        protected DiagnosticResult GetExpectedErrorOnSingleTag(params object[] formatParameters)
        {
            return GetExpectedError(8, 26, formatParameters);
        }
    }
}