#r "packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.Testing

let buildDir = "bin/release"

let appReferences  = 
    !! "**/*.csproj"

Target "paket" (fun _ ->
    ()
)

Target "build" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)


Target "test" (fun _ ->
    // define test dlls
    let testDlls = !! (buildDir @@ "**/*.Test.dll")
    
    testDlls
        |> xUnit (fun p -> 
        { 
            p with 
                ShadowCopy = true
                ToolPath = "packages/xunit.runner.console/tools/xunit.console.x86.exe"
        })
)

Target "nuget" (fun _ -> 
    let version = 
        match buildVersion with
        | "LocalBuild" -> "0.0.0.0"
        | _ -> buildVersion

    NuGet (fun p -> 
        {p with
            OutputPath = buildDir
            WorkingDir = buildDir
            Version = version
            Publish = false 
        }) ("Protobuf.CodeFixes" @@ "Protobuf.CodeFixes" @@ "Diagnostic.nuspec")
    
    ()
)

"paket"
    ==> "build"
    ==> "test"
    ==> "nuget"
    
RunTargetOrDefault "build"