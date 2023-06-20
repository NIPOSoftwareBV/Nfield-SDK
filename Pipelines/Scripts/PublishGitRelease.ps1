param(
    [Parameter(Mandatory=$true)] $AccessToken
    [Parameter(Mandatory=$true)] $ReleaseId
    [Parameter(Mandatory=$true)] $Version
)

Write-Host Publishing release $Version on GitHub
Write-Host ReleaseId: $apiUrl
Write-Host Version: $Version

$organization = "NIPOSoftware"
$repository = "Nfield-SDK"

$Headers = @{
    Authorization = 'Basic {0}' -f [System.Convert]::ToBase64String([char[]]($AccessToken));
}

$apiUrl = "https://api.github.com/repos/{0}/{1}/releases/{2}" -f  $organization, $repository, $ReleaseId
$description = "Package version: " + $Version
$patchBody = @{ 
    prerelease = 'true';
    body = $description
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
