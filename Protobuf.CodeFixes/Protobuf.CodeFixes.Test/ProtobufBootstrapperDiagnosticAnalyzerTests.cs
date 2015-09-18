using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NFluent;
using Protobuf.CodeFixes.AttributeData;
using Xunit;

namespace Protobuf.CodeFixes.Test
{
    public class TestAnalyzer : ProtobufDiagnosticAnalyzerBase
    {
        public override string DiagnosticId => "aDiagnosticId";
        public override string Title => "aTitle";
        public override string MessageFormat => "aMessageFormat";
        public override string Description => "aDescription";
        public override DiagnosticSeverity Severity => DiagnosticSeverity.Error;
        public override void Analyze(SymbolAnalysisContext context, List<IncludeAttributeData> includeTags, List<ProtobufAttributeData> memberTags)
        {
            MemberTags.Add(context.Symbol.Name, memberTags);
            IncludeTags.Add(context.Symbol.Name, includeTags);
        }

        public Dictionary<string, List<ProtobufAttributeData>> MemberTags { get; private set; } = new Dictionary<string, List<ProtobufAttributeData>>();

        public Dictionary<string, List<IncludeAttributeData>> IncludeTags { get; private set; } = new Dictionary<string, List<IncludeAttributeData>>();
    }

    public class ProtobufBootstrapperDiagnosticAnalyzerTests : ProtobufBootstrappedDiagnosticAnalyzerTestsBase<TestAnalyzer>
    {
        private TestAnalyzer GetTestAnalyze(DiagnosticAnalyzer verifyCSharpDiagnostic)
        {
            var analyzer = (ProtobufBootstrapperDiagnosticAnalyzer) verifyCSharpDiagnostic;
            return (TestAnalyzer)analyzer.Analyzers.First();
        }

        [Fact]
        public void Empty_class_has_no_member_and_no_includes()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
           public string SomeField;
        }
    }";
            var testAnalyzer = GetTestAnalyze(VerifyCSharpDiagnostic(source));
            Check.That(testAnalyzer.IncludeTags["SampleType"]).IsEmpty();
            Check.That(testAnalyzer.MemberTags["SampleType"]).IsEmpty();
        }

        [Fact]
        public void Proto_include_is_passed_to_analyzers()
        {
            const string source = @"    using System;
    using ProtoBuf;

    namespace Samples
    {
        [ProtoInclude(1, typeof(SubType1))]
        [ProtoInclude(2, typeof(SubType2))]
        class SampleType {}
        class SubType1 : SampleType { }
        class SubType2 : SampleType { }
    }";
            var testAnalyzer = GetTestAnalyze(VerifyCSharpDiagnostic(source));
            Check.That(testAnalyzer.IncludeTags["SampleType"]).HasSize(2);
            Check.That(testAnalyzer.MemberTags["SampleType"]).IsEmpty();
        }

        [Fact]
        public void Proto_member_is_passed_to_analyzers()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;
    using ProtoBuf;

    namespace Samples
    {
        class SampleType
        {   
            [ProtoMember(1)]
            public string SomeField;

            [ProtoMember(2)]
            public string SomeProperty {get; set; }
        }
    }";
            var testAnalyzer = GetTestAnalyze(VerifyCSharpDiagnostic(source));
            var members = testAnalyzer.MemberTags;
            Check.That(testAnalyzer.IncludeTags["SampleType"]).IsEmpty();
            Check.That(members["SampleType"]).HasSize(2);
            Check.That(members["SampleType"][0].Tag).IsEqualTo(1);
            Check.That(members["SampleType"][0].AttributeData.AttributeClass.Name).IsEqualTo("ProtoMemberAttribute");
            Check.That(members["SampleType"][0].Symbol.Name).IsEqualTo("SomeField");
            Check.That(members["SampleType"][1].Tag).IsEqualTo(2);
            Check.That(members["SampleType"][1].AttributeData.AttributeClass.Name).IsEqualTo("ProtoMemberAttribute");
            Check.That(members["SampleType"][1].Symbol.Name).IsEqualTo("SomeProperty");
        }

        [Fact]
        public void Data_member_is_passed_to_analyzers()
        {
            const string source = @"    using System;
    using System.Runtime.Serialization;

    namespace Samples
    {
        class SampleType
        {   
            [DataMember(Order = 1)]
            public string SomeField;

            [DataMember(Order = 2)]
            public string SomeProperty {get; set; }
        }
    }";
            var testAnalyzer = GetTestAnalyze(VerifyCSharpDiagnostic(source));
            var members = testAnalyzer.MemberTags;
            Check.That(testAnalyzer.IncludeTags["SampleType"]).IsEmpty();
            Check.That(members["SampleType"]).HasSize(2);
            Check.That(members["SampleType"][0].Tag).IsEqualTo(1);
            Check.That(members["SampleType"][0].AttributeData.AttributeClass.Name).IsEqualTo("DataMemberAttribute");
            Check.That(members["SampleType"][0].Symbol.Name).IsEqualTo("SomeField");
            Check.That(members["SampleType"][1].Tag).IsEqualTo(2);
            Check.That(members["SampleType"][1].AttributeData.AttributeClass.Name).IsEqualTo("DataMemberAttribute");
            Check.That(members["SampleType"][1].Symbol.Name).IsEqualTo("SomeProperty");
        }
    }
}