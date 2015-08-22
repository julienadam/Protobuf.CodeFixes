using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtobufTagZeroDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ProtobufTagZeroDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = ProtobufTagZeroDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = ProtobufTagZeroDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_property()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(1));
        }

        [Fact]
        public void No_error_for_tag_set_to_non_zero_on_field()
        {
            VerifyCSharpDiagnostic(GetOneTagFieldClassSource(1));
        }

        [Fact]
        public void Datamember_tag_set_to_zero_on_property_triggers_error()
        {
            var dataContractPropertyClass = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 0)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(dataContractPropertyClass, GetExpectedError(8, 33, "SomeProperty"));
        }

        [Fact]
        public void Datamember_tag_set_to_zero_on_field_triggers_error()
        {
            var dataContractPropertyClass = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 0)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(dataContractPropertyClass, GetExpectedError(8, 33, "SomeField"));
        }

        [Fact]
        public void Tag_set_to_zero_on_property_triggers_error()
        {
            VerifyCSharpDiagnostic(GetOneTagPropertyClassSource(0), GetExpectedErrorOnSingleTag("SomeProperty"));
        }

        [Fact]
        public void Tag_set_to_zero_on_field_triggers_error()
        {
            VerifyCSharpDiagnostic(GetOneTagFieldClassSource(0), GetExpectedErrorOnSingleTag("SomeField"));
        }
    }
}