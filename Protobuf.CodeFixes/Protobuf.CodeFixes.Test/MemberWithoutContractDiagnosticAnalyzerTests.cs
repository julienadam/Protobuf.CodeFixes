using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class MemberWithoutContractDiagnosticAnalyzerTests
         : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<MemberWithoutContractDiagnosticAnalyzer>
    {
        [Fact]
        public void No_error_for_class_with_protomember_and_protocontract()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoContract]
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void No_error_for_class_with_datamember_and_datacontract()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        [DataContract]
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void Class_with_protomember_but_no_protocontract_causes_warning()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedWarning(6, 15, "SampleType"));
        }

        [Fact]
        public void Class_with_datamember_but_no_datacontract_causes_warning()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedWarning(6, 15, "SampleType"));
        }

        [Fact]
        public void Class_with_two_datamember_but_no_datacontract_causes_only_one_warning()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeProperty { get; set}

            [ProtoMember(2)]
            public string SomeProperty { get; set}
        }
    }";
            VerifyCSharpDiagnostic(source, GetExpectedWarning(7, 15, "SampleType"));
        }
    }
}