﻿using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class MemberWithoutContractDiagnosticAnalyzerTests :
        ProtobufDiagnosticAnalyzerTestsBase<MemberWithoutContractDiagnosticAnalyzer>
    {
        protected override string DiagnosticId { get; } = MemberWithoutContractDiagnosticAnalyzer.DiagnosticId;
        protected override string MessageFormat { get; } = MemberWithoutContractDiagnosticAnalyzer.MessageFormat;

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
        public void Class_with_protomember_but_no_protocontract_causes_error()
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
            VerifyCSharpDiagnostic(source, GetExpectedError(6, 15, "Proto", "SomeProperty", "SampleType"));
        }

        [Fact]
        public void Class_with_datamember_but_no_datacontract_causes_error()
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
            VerifyCSharpDiagnostic(source, GetExpectedError(6, 15, "Data", "SomeProperty", "SampleType"));
        }
    }
}