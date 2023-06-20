param(
    [Parameter(Mandatory=$true)] 
    [string] $AccessToken,
    [Parameter(Mandatory=$true)] 
    [string] $CommitHash,
    [Parameter(Mandatory=$true)]
    [string] $VersionName,
    [Parameter(Mandatory=$true)]
    [string] $BranchName
)

$organization = "NIPOSoftware"
$repository = "Nfield-SDK"

Write-Host Publishing release $organization/$repository/$BranchName/$VersionName on GitHub
Write-Host Commit Hash: $CommitHash
Write-Host Version Name: $VersionName
Write-Host Branch Name: $BranchName

# https://docs.github.com/en/rest/releases/releases?apiVersion=2022-11-28#create-a-release
try
{

    $releaseName = $BranchName.Split("/");
    if ($releaseName.count -eq 2)
    {
        $releaseName = $releaseName[1].Replace("-", " ")
    }
    else
    {
        $releaseName = $releaseName[0].Replace("-", " ")
    }
    Write-Host Release Name: $releaseName

    $Headers = @{
        Accept = 'application/vnd.github+json';
        Authorization = 'Bearer {0}' -f $AccessToken;
    }

    $apiUrl = "https://api.github.com/repos/{0}/{1}/releases" -f  $organization, $repository
    $description = "Package version: " + $VersionName

    $patchBody = @{ 
        prerelease = 'true';
        name = $releaseName;
        body = $description;
        tag_name = $BranchName;
        target_commitish = $CommitHash;
    } | ConvertTo-Json

    Write-Host Url: $apiUrl

    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-RestMethod -Uri $apiUrl -Method Patch -Headers $Headers -Body $patchBody
}
catch
{
    Write-Host Error when trying to publish the release: $_
    exit 1
}

Write-Host Release $releaseName published!
