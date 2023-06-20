param(
    [Parameter(Mandatory=$true)] $AccessToken
    [Parameter(Mandatory=$true)] $CommitHash
    [Parameter(Mandatory=$true)] $VersionName
    [Parameter(Mandatory=$true)] $BranchName
)

Write-Host Publishing release $VersionName on GitHub
Write-Host Commit Hash: $CommitHash
Write-Host Version Name: $VersionName
Write-Host Branch Name: $BranchName

# https://docs.github.com/en/rest/releases/releases?apiVersion=2022-11-28#create-a-release

$organization = "NIPOSoftware"
$repository = "Nfield-SDK"

$Headers = @{
    Accept = 'application/vnd.github+json';
    Authorization = 'Bearer {0}' -f $AccessToken;
}

$apiUrl = "https://api.github.com/repos/{0}/{1}/releases" -f  $organization, $repository,
$description = "Package version: " + $VersionName
$patchBody = @{ 
    prerelease = 'true';
    name =;
    body = $description;
    tag_name = $BranchName;
    target_commitish = $CommitHash;
} | ConvertTo-Json

Write-Host Url: $apiUrl

try
{
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-RestMethod -Uri $apiUrl -Method Patch -Headers $Headers -Body $patchBody
}
catch
{
    Write-Host Error when trying to publish the release: $_
    exit 1
}

Write-Host Release $release.name published!
