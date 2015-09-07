#r "packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.Testing

let buildDir = "bin/release"

let appReferences  = 
    !! "**/*.csproj"

Target "build" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

// define test dlls
let testDlls = !! (buildDir @@ "**/*.Test.dll")

Target "test" (fun _ ->
    testDlls
        |> xUnit (fun p -> 
        { 
            p with 
                ShadowCopy = true
                ToolPath = "packages/xunit.runner.console/tools/xunit.console.x86.exe"
        })
)

Target "package" (fun _ -> 
    ()
)

"build"
    ==> "test"
    ==> "package"
    
RunTargetOrDefault "build"