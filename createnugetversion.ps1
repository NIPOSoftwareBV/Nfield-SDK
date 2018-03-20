# Create NuGet Version
# The following version format will be used: Major.Minor.Patch
# The segment Major and Minor should be configured in version.txt (see root of Nfield-SDK repo)
# The 3rd segment (Patch) will be determined by {buildId}{suffix}

if( -Not (Test-Path version.txt))
{
    Write-Error "version.txt does not exist"
    exit 1	
}
$VersionFormat = Get-Content version.txt | Where-Object { !$_.StartsWith("#") }
Write-Host "VersionFormat:" $VersionFormat
if(([regex]::Matches($VersionFormat, "\." )).count -ne 2) 
{
    Write-Error "Version is in format $VersionFormat but should be in format 0.1.{buildId}{suffix}"
    exit 1	
}

$BuildId = $env:BUILD_BUILDID
Write-Host "BuildId:" $BuildId

Write-Host "releaseversion:" $env:releaseversion
if ($env:releaseversion -eq "true")
{
# releaseversion is set to true in azure function
$Suffix = ""
}
else
{
$Branch = $env:BUILD_SOURCEBRANCHNAME
Write-Host "Branch:" $Branch
$Suffix = if ($Branch -eq "master") {"-beta"} else {"-alpha"}	
}
Write-Host "Suffix:" $Suffix
$Version = $VersionFormat.replace("{buildId}",$BuildId).replace("{suffix}",$Suffix)
Write-Host "##vso[task.setvariable variable=Version]$Version"
Write-Host "Version:" $Version