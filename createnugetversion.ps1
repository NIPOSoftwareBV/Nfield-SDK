#    This file is part of Nfield.SDK.
#
#    Nfield.SDK is free software: you can redistribute it and/or modify
#    it under the terms of the GNU Lesser General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Nfield.SDK is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU Lesser General Public License for more details.
#
#    You should have received a copy of the GNU Lesser General Public License
#    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

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

if ($env:releaseId)
{
    Write-Host "Building for release" $env:releaseId
    $Suffix = ""
}
else
{
    $Branch = $env:BUILD_SOURCEBRANCHNAME
    Write-Host "Branch:" $Branch
    $Suffix = if ($Branch -eq "master") {"-beta"} else {"-alpha"}	
    Write-Host "Suffix:" $Suffix
}

$Version = $VersionFormat.replace("{buildId}",$BuildId).replace("{suffix}",$Suffix)
Write-Host "##vso[task.setvariable variable=Version]$Version"
Write-Host "Version:" $Version
