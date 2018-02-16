[CmdletBinding()]
Param(
	[Parameter(Position=0,Mandatory=$false)]
    [string]$Target = "default",
    [Parameter(Position=1)]
    $File = ".\default.ps1",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Debug",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Minimal",
    [switch]$SkipPackageRestore,
    [Parameter(Position=2,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
$IB = Join-Path $TOOLS_DIR "Invoke-Build/tools/Invoke-Build.ps1"
$MSBuild = Join-Path $TOOLS_DIR "Invoke-Build/tools/Resolve-MSBuild.ps1"
$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -and !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}


# Try find NuGet.exe in path if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Trying to find nuget.exe in PATH..."
    $existingPaths = $Env:Path -Split ';' | Where-Object { (![string]::IsNullOrEmpty($_)) -and (Test-Path $_) }
    $NUGET_EXE_IN_PATH = Get-ChildItem -Path $existingPaths -Filter "nuget.exe" | Select -First 1
    if ($NUGET_EXE_IN_PATH -ne $null -and (Test-Path $NUGET_EXE_IN_PATH.FullName)) {
        Write-Verbose -Message "Found in PATH at $($NUGET_EXE_IN_PATH.FullName)."
        $NUGET_EXE = $NUGET_EXE_IN_PATH.FullName
    }
}

# Try download NuGet.exe if not exists
if (!(Test-Path $NUGET_EXE)) {
    Write-Verbose -Message "Downloading NuGet.exe..."
    try {
        (New-Object System.Net.WebClient).DownloadFile($NUGET_URL, $NUGET_EXE)
    } catch {
        Throw "Could not download NuGet.exe."
    }
}

# Save nuget.exe path to environment to be available to child processed
$ENV:NUGET_EXE = $NUGET_EXE


Push-Location
Set-Location $TOOLS_DIR


Write-Verbose -Message "Restoring tools from NuGet..."
$NuGetOutput = Invoke-Expression "&`"$NUGET_EXE`" install -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""

if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Throw "An error occured while restoring NuGet tools."
}
Write-Verbose -Message ($NuGetOutput | out-string)
Pop-Location


# Make sure that Cake has been installed.
if (!(Test-Path $IB)) {
    Throw "Could not find Invoke-Build.ps1 at $IB"
}

Set-Alias Nuget ($NUGET_EXE)
Set-Alias MSBuild ($MSBuild)
Set-Alias ib $IB

if ($env:TEAMCITY_VERSION) {
	# When PowerShell is started through TeamCity's Command Runner, the standard
	# output will be wrapped at column 80 (a default). This has a negative impact
	# on service messages, as TeamCity quite naturally fails parsing a wrapped
	# message. The solution is to set a new, much wider output width. It will
	# only be set if TEAMCITY_VERSION exists, i.e., if started by TeamCity.
    $host.UI.RawUI.BufferSize = New-Object System.Management.Automation.Host.Size(8192,50)
}

$params = @{}

if ($ScriptArgs.Length -cgt 0) {

    $ScriptArgs | ForEach-Object {
        if($_ -match '^-') {
            #New parameter
            $lastvar = $_ -replace '^-'
            $params[$lastvar] = $null
        } else {
            #Value
            $params[$lastvar] = $_
        }
    }
}

# Start Invoke-Build
Write-Host "Running build script..."
ib $Target $File @params