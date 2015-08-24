using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzerTests :
        ProtobufDiagnosticAnalyzerTestsBase<DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = DuplicateTagBetweenMemberAndIncludeDiagnosticAnalyzer.MessageFormat;

        [Fact]
        public void Duplicate_tags_between_include_and_protomember_on_property_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        class SampleType 
        {
            [ProtoMember(1)]
            public int SomeProperty { get; set; }
        }
        class SubType1 : SampleType { }
    }";

            var error1 = GetExpectedError(6, 23, 1, "SampleType", "SomeProperty, include(SubType1)");
            var error2 = GetExpectedError(9, 26, 1, "SampleType", "SomeProperty, include(SubType1)");
            VerifyCSharpDiagnostic(source, error1, error2);
        }

        [Fact]
        public void Duplicate_tags_between_include_and_protomember_on_field_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        class SampleType 
        {
            [ProtoMember(1)]
            public int SomeField
        }
        class SubType1 : SampleType { }
    }";

            var error1 = GetExpectedError(6, 23, 1, "SampleType", "SomeField, include(SubType1)");
            var error2 = GetExpectedError(9, 26, 1, "SampleType", "SomeField, include(SubType1)");
            VerifyCSharpDiagnostic(source, error1, error2);
        }
        
        [Fact]
        public void Duplicate_tags_between_include_and_data_member_on_property_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;
    using System.Runtime.Serialization;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        class SampleType 
        {
            [DataMember(Order = 1)]
            public int SomeProperty { get; set; }
        }
        class SubType1 : SampleType { }
    }";

            var error1 = GetExpectedError(7, 23, 1, "SampleType", "SomeProperty, include(SubType1)");
            var error2 = GetExpectedError(10, 33, 1, "SampleType", "SomeProperty, include(SubType1)");
            VerifyCSharpDiagnostic(source, error1, error2);
        }

        [Fact]
        public void Duplicate_tags_between_include_and_data_member_on_field_show_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;
    using System.Runtime.Serialization;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        class SampleType 
        {
            [DataMember(Order = 1)]
            public int SomeField;
        }
        class SubType1 : SampleType { }
    }";

            var error1 = GetExpectedError(7, 23, 1, "SampleType", "SomeField, include(SubType1)");
            var error2 = GetExpectedError(10, 33, 1, "SampleType", "SomeField, include(SubType1)");
            VerifyCSharpDiagnostic(source, error1, error2);
        }
    }
}
