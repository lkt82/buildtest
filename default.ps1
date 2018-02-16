#globals
$GitVersion = Join-Path $TOOLS_DIR GitVersion.CommandLine\tools\GitVersion.exe

$TEMP_Dir = "$BuildRoot\temp"
$ARTIFACTS_Dir = "$BuildRoot\artifacts"

Set-Alias GitVersion ($GitVersion)

$Authors = "lkt"
$Company = "Intelligine"

#tasks
task Restore -If {$SkipPackageRestore -eq $false} {

    exec { & dotnet restore }
}

task Version {
    $versionJson = exec { GitVersion } | Out-String
    $script:Version = ConvertFrom-Json $versionJson

    $buildNumber = $script:Version.FullSemVer + ".build."  + $env:APPVEYOR_BUILD_NUMBER

    $script:buildNumber = $buildNumber

}, PublishVersion


task Clean {
    exec { & dotnet clean }

    rm $TEMP_Dir -Recurse -Force -ErrorAction 0
    rm $ARTIFACTS_Dir -Recurse -Force -ErrorAction 0
}

task Build Version, Restore, Clean, {
    $AssemblySemFileVer = $script:Version.AssemblySemFileVer
    $AssemblySemVer = $script:Version.AssemblySemVer
    $InformationalVersion = $script:Version.InformationalVersion
    exec { & dotnet build -v $Verbosity -c $Configuration --no-restore /p:AssemblyVersion=$AssemblySemVer /p:Authors=$Authors /p:Company=$Company /p:FileVersion=$AssemblySemFileVer /p:InformationalVersion=$InformationalVersion}
}

task Pack Build, {

    mkdir $ARTIFACTS_Dir -Force | Out-Null
    mkdir $TEMP_Dir -Force | Out-Null

    $packageVersion = $script:Version.FullSemVer
    $script:packageVersion = $packageVersion

    exec { & dotnet pack src\Dbc --no-build --no-restore --output "$ARTIFACTS_Dir/lib" /p:Authors="$Authors" /p:Company="$Company" /p:PackageVersion=$script:packageVersion /p:NoPackageAnalysis=true -c $Configuration}

}

task Push -If $env:APPVEYOR {

    Get-Item "$ARTIFACTS_Dir/lib/*.nupkg" | % {
        $path = $_.FullName
        #exec { Nuget push $_.FullName $env:Nuget_Api_Key -Source $env:Nuget_Feed_Push }
    }
}


task Release -If $env:APPVEYOR Push, {
}

task CI Pack, Release

task default Build

task PublishVersion -If $env:APPVEYOR {
    Update-AppveyorBuild -Version $script:buildNumber
}
