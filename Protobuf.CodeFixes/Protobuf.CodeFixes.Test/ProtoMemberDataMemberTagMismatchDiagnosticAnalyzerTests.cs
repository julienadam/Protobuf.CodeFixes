using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class ProtoMemberDataMemberTagMismatchDiagnosticAnalyzerTests 
        : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<ProtoMemberDataMemberTagMismatchDiagnosticAnalyzer>
    {
        [Fact]
        public void No_mismatch_when_the_tags_are_the_same()
        {
            const string source = @"    using System;
    using ProtoBuf;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            [DataMember(Order = 1)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Mismatch_between_datamember_and_protomember_on_field_shows_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            [DataMember(Order = 2)]
            public string SomeField;
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(9, 26, "SomeField", 1, 2), GetExpectedError(10, 33, "SomeField", 1, 2));
        }

        [Fact]
        public void Mismatch_between_datamember_and_protomember_on_property_shows_as_error()
        {
            const string source = @"    using System;
    using ProtoBuf;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            [DataMember(Order = 2)]
            public string SomeProperty { get; set; }
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedError(9, 26, "SomeProperty", 1, 2), GetExpectedError(10, 33, "SomeProperty", 1, 2));
        }
    }
}