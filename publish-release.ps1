param(
    [Parameter(Mandatory=$true)] $AccessToken
)

# TODO: put back SDK params

$organization = marcoNipo #"NIPOSoftware"
$repository = releases-test #"Nfield-SDK"

$Headers = @{
    Authorization = 'Basic {0}' -f [System.Convert]::ToBase64String([char[]]($AccessToken));
}

$apiUrl = "https://api.github.com/repos/{0}/{1}/releases/{2}" -f  $organization, $repository, $env:releaseId
$description = "Package version: " + $env:Version
$patchBody = @{ 
    prerelease = 'false';
    body = $description
} | ConvertTo-Json

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
