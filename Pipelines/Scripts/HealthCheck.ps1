# This scripts checks the health status by calling to the health check endpoint
# it is called from the deploy pipeline and it is a pre-swap validation
Param(
  [string] [Parameter(Mandatory = $true)] $Uri,
  [int] $MaxRetries = 10,
  [int] $InitialDelay = 30,
  [int] $DelayBetweenRetrays = 10
)

[System.Net.ServicePointManager]::DnsRefreshTimeout = 0

$RetriesCounter = 0;

# initial delay for giving time to the service to be ready to receive requests
Start-Sleep -Seconds $InitialDelay


Write-Host "Health check status started..."
do{
    # Send a request to the health check endpoint
    try{
        $response = Invoke-RestMethod -Uri $Uri;
    }
    catch {
        $err=$_.Exception

        if($_.Exception.Response -ne $null){
            $result = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($result)
            $reader.BaseStream.Position = 0
            $reader.DiscardBufferedData()
            $responseBody = $reader.ReadToEnd();
        }
    }

    if($response.Status -ne "Healthy"){
        $RetriesCounter++;
        Write-Host "...";
		$err="Error: Servece Unhealthy"
        Start-Sleep -Seconds $DelayBetweenRetrays
    }

    if($response.Status -eq "Healthy"){
        Write-Host "Service status healthy" -ForegroundColor Green;
        exit 0;
    }

}while($response.Status -ne "Healthy" -and $RetriesCounter -lt $MaxRetries)

#In case it's unhealthy print the reason and the health check report
Write-Error "Error: $($err)";
Write-Host ($responseBody);
exit 1;