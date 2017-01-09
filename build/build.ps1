param(
    [Parameter()]
    [switch] $pushPackage
)

$ErrorActionPreference = "Stop"

function Get-IncrementedVersion(){
    $version = Get-Content version
    $splitVersion = $version.Split(".")
    $splitVersion[2] = ([int]::Parse($splitVersion[2]) + 1).ToString()
    return [string]::Join(".", $splitVersion)
}

function Set-Version($version){
    Set-Content -Path version -Value $version
}

$root = (Resolve-Path ../)

Push-Location "$root/src/OctoSearch"
Remove-Item -Recurse -Force "bin","obj"
$version = Get-IncrementedVersion
Set-Version $version
dotnet restore ./OctoSearch.Package.csproj
dotnet publish ./OctoSearch.Package.csproj --runtime win7-x64 --configuration Release

Remove-Item *.nupkg,*.zip
nuget pack OctoSearch.nuspec -NoPackageAnalysis -Properties "version=$version"

$zipPath = [System.IO.Path]::Combine((resolve-path .), "OctoSearch.$version.zip")
Add-Type -As System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory(
    (resolve-path "bin\Release\netcoreapp1.1\win7-x64\"), 
    $zipPath,
    "Optimal", 
    $false)

if($pushPackage)
{
    $tag = "v$version"
    git tag $tag ; git push --tags
    ..\..\build\tools\github-release.exe release `
                               --user naeemkhedarun `
                               --repo OctoSearch `
                               --tag $tag
    
    Start-Sleep -Seconds 5;

    ..\..\build\tools\github-release.exe upload `
                               --user naeemkhedarun `
                               --repo OctoSearch `
                               --tag $tag `
                               --name "windows-x64-octosearch-$version" `
                               --file $zipPath

    nuget push OctoSearch*.nupkg -Source https://www.nuget.org/api/v2/package
}

popd
