param (
    [string]$sourceFolder = "..\src",
    [string]$buildFolder = ".\build",
    [string]$apiKey = "",
    [string]$nugetUrl = "https://www.nuget.org/",
    [string]$packagePattern = "Bdd.Uwp",
    [bool]$buildOnly = $false
)

$ErrorActionPreference = "Stop"

write-host "Running uploading tool. Source folder is '$($sourceFolder)'" -foreground "green"

write-host Checking source folder exists
if (!(test-path $sourceFolder))
{
    write-host "Source folder doens't exist" -foreground "red"
    exit
}

write-host Creating or clearing destination folder -foreground "green"
if (!(test-path $buildFolder))
{
    write-host Creating build folder -foreground "green"
    new-item -path $buildFolder -type directory
}
else
{
    write-host Removing items from build folder -foreground "green"
    Remove-Item (join-path $buildFolder '*')
}

write-host "Getting package list for packagePattern '$($packagePattern)'" -foreground "green"
$packages = Invoke-Expression "& '..\tools\nuget.exe' 'list' '-AllVersions' '$($packagePattern)'"

write-host Collecting csproj files -foreground "green"
$csprojFiles = Get-ChildItem $sourceFolder -Include *.csproj -Recurse

write-host Collecting nuspecs from scproj files -foreground "green"
$changedPackages = New-Object System.Collections.ArrayList

for ($i = 0; $i -lt $csprojFiles.Length; $i++)
{
    $csprojFolder = Split-Path $csprojFiles[$i] -resolve
    $csprojFile = Split-Path $csprojFiles[$i] -leaf -resolve
    $nuspecFile = ($csprojFiles[$i] -replace ".csproj", ".nuspec")

    if (test-path $nuspecFile)
    {
        $csprojFileNoExtension = $csprojFile -replace '.csproj', ''
        $assemblyInfoFile = Join-Path $csprojFolder -ChildPath "Properties" | Join-Path -ChildPath "AssemblyInfo.cs"
        $assemblyInfoContent = Get-Content $assemblyInfoFile
        $version = [regex]::Match($assemblyInfoContent, 'AssemblyVersion\("([0-9.]+)"\)').Captures.Groups[1].Value
        $packageSearchPatter = "$($csprojFileNoExtension) $($version)"

        if (-Not $packages.Contains($packageSearchPatter))
        {
            $obj = @{}
            $obj.csprojFile = $csprojFiles[$i]
            $obj.name = $csprojFileNoExtension
            $obj.version = $version
            $obj.nuspecFile = $nuspecFile

            $changedPackages.Add($obj)
        }
    }
}

write-host Building and uploading changed packages -foreground "green"
for ($i = 0; $i -lt $changedPackages.Count; $i++)
{
    $changedPackage = $changedPackages[$i]

    $params = @('pack', $changedPackage.csprojFile, '-IncludeReferencedProjects', '-Prop',
        'Configuration=Release;Platform=x64', '-Build', '-OutputDirectory', $buildFolder)

    $outputPath = Join-Path (Split-Path $changedPackage.csprojFile -resolve) "bin\Release"

    write-host "Rebuilding '$($changedPackage.csprojFile)'" -foreground "green"
    & "..\tools\nuget.exe" $params

    $nupkgFile = Join-Path $buildFolder "$($changedPackage.name).$($changedPackage.version).nupkg"

    if (-not $buildOnly)
    {
        write-host "Uploading '$($changedPackage.name).$($changedPackage.version)'" -foreground "green"
            & "..\tools\nuget.exe" @('push', $nupkgFile, '-s', $nugetUrl, '-ApiKey', $apiKey)
    }
}
