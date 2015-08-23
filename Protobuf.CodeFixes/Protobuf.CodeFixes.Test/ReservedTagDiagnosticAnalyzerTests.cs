using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ReservedTagDiagnosticAnalyzerTests : ProtobufDiagnosticAnalyzerTestsBase<ReservedTagDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = ReservedTagDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = ReservedTagDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void No_error_on_non_reserved_tag_for_property()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void No_error_on_non_reserved_tag_for_field()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Protomember_with_tag_on_property_in_reserved_range_show_as_error(int tag)
        {
           var source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(" + tag + @")]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, tag, "SomeProperty"));
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Datamember_with_tag_on_property_in_reserved_range_show_as_error(int tag)
        {
            var source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = " + tag + @")]
            public string SomeProperty { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, tag, "SomeProperty"));
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Protomember_with_tag_on_field_in_reserved_range_show_as_error(int tag)
        {
            var source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(" + tag + @")]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 26, tag, "SomeField"));
        }

        [Theory]
        [InlineData(19000)]
        [InlineData(19500)]
        [InlineData(19999)]
        public void Datamember_with_tag_on_field_in_reserved_range_show_as_error(int tag)
        {
            var dataContractPropertyClass = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = " + tag + @")]
            public string SomeField;
        }
    }";

            var source = dataContractPropertyClass.Replace("$tag$", tag.ToString());
            VerifyCSharpDiagnostic(source, GetExpectedError(8, 33, tag, "SomeField"));
        }
    }
}