<#
.SYNOPSIS
  Creates an App Registration for dev environments (ex. `yellow-domain-app`) with an associated Service Principal. 
  The service principal will have an app role "NfieldService"
.Notes
Before executing the script locally we need to connect to the MS Graph:
Connect-MgGraph -TenantId "TenantId" -Scopes "User.ReadWrite.All","Group.ReadWrite.All","Domain.Read.All","RoleManagement.ReadWrite.Directory","Directory.AccessAsUser.All", "Directory.ReadWrite.All", "Application.ReadWrite.All" -DeviceCode
#>

Param(
  [string] [Parameter(Mandatory = $true)] $keyVaultName,
  [string] [Parameter(Mandatory = $true)] $domainApiAppRegistrationName,
  [string] [Parameter(Mandatory = $true)] $azureEnvironment # In case the environment is not the default one i.e Azure China
)

Write-Host "##[section] Params"
Write-Host "Domain Api AppRegistration Name: $domainApiAppRegistrationName" -ForegroundColor "Green"
Write-Host "KeyVault Name: $keyVaultName" -ForegroundColor "Green"

Write-Host "##[section] Connnect to MgGraph API"
# Connnect to MgGraph API. 
# When this script is run in the pipeline, the token belongs to the managed identity of service connection used to connect the Azure DevOps service and the Azure subsctription
# When this script is run locally, the token belongs to the user
Write-Host "Getting AzAccessToken and conectiong to MgGraph"
$token = (Get-AzAccessToken -ResourceTypeName MSGraph).Token
Connect-MgGraph -AccessToken $token -Environment $azureEnvironment


# In the follwing lines, we will need to know if the Domain API app registration client id is in a keyvault secret called 'PublicApiDomainApiAppRegistrationClientId' in the Nfield{environment}KV keyvault
$secretName = "PublicApiDomainApiAppRegistrationClientId"
$secretvalue = Get-AzKeyVaultSecret -VaultName $keyVaultName -Name $secretName

# We might need to add conditions for building the name for RC and production environments
Write-Host "##[section] Domain API App Registration"
if (!($domainApiAppRegistration = Get-MgApplication -Filter "DisplayName eq '$($domainApiAppRegistrationName)'"  -ErrorAction SilentlyContinue))
{
    # If the app registration does not exist, but the secret that stores its id exists, then an old app registration has been deleted but not cleaned
    # Creating a new app registration would require to update the secret, however, because updating this secret automatically might bring other unexpected issues,
    # we prefer to solve this weird situation manually
    if($secretvalue){
        Write-Error "$($domainApiAppRegistrationName) Domain API app registration not found but the $($secretName) secret already exists in the KeyVault.
                    To continue, the secret $($secretName) needs to be removed and purged from the $($keyVaultName) KeyVault. Then you can redeploy both the Domain API and Public API to provision the new Domain API app registration client id"

        Exit "Execution aborted"
    }

    Write-Host "Domain API App Registration will be provisioned"
        
    $domainApiAppRegistration = New-MgApplication -DisplayName $domainApiAppRegistrationName -SignInAudience "AzureADMyOrg"
    Write-Host "Domain API Registration provisioned successfully" -ForegroundColor "Green"
}
else
{
    Write-Host "Domain API Registration has already been provisioned."
}

# In case it does not exits. Store the Domain API Registration client id in a keyvault secret called 'PublicApiDomainApiAppRegistrationClientId' in the Nfield{environment}KV keyvault
# It will be used in the Public API deployment nfield-source\Build-Templates\PublicApi\deploy.yml
Write-Host "##[section] Store $($secretName) secret in $($keyVaultName) keyvault"
if(!$secretvalue){
    $secretvalue = ConvertTo-SecureString $domainApiAppRegistration.AppId -AsPlainText -Force
    Set-AzKeyVaultSecret -VaultName $keyVaultName -Name $secretName -SecretValue $secretValue

    Write-Host "$($secretName) secret stored successfully" -ForegroundColor "Green"
}
else
{
    Write-Host $secretNamet "secret already exists."
}

Write-Host "##[section] Domain API App Service Principal"
if (!($domainAppServicePrincipal = Get-MgServicePrincipal -Filter "DisplayName eq '$($domainApiAppRegistrationName)'"  -ErrorAction SilentlyContinue))
{
    Write-Host "Domain API App Service Principal will be provisioned"
    # Provisions the domain  app service principal. It will be used to assign roles to the Public API's App Service.
    $domainAppServicePrincipal = New-MgServicePrincipal -AppId $domainApiAppRegistration.AppId -AccountEnabled
    Write-Host "Domain API App ServicePrincipal has been provisioned" -ForegroundColor "Green"
}
else
{
    Write-Host "Domain API App Service Principal has already been provisioned."
}

# Updates the application to expose its API publicly and to add an Application Role.
Write-Host "##[section] Domain API App's NfieldService App Role"
if ($domainApiAppRegistration.AppRoles.Where({ $_.Value -eq "NfieldService" })) {
    Write-Host "Domain API App's NfieldService App Role and IdentifierUris have already been provisioned."
}
else{
    Write-Host "Domain API App IdentifierUris and Roles will be added"
    if (!($domainApiAppRegistration.AppRoles))
    {
        $appRoles = New-Object System.Collections.Generic.List[Microsoft.Graph.PowerShell.Models.IMicrosoftGraphAppRole]
    }
    else
    {
        $appRoles= $domainApiAppRegistration.AppRoles
    }

    $role = New-Object Microsoft.Graph.PowerShell.Models.MicrosoftGraphAppRole 
    $role.Id = [Guid]::NewGuid().ToString()
    $role.DisplayName = "NfieldService"
    $role.Value ="NfieldService"
    $role.IsEnabled = $true
    $role.Description="Allows Nfield Services to connect to Domain API Service"
    # Allows only Applications to register to this App Role (not User)
    $role.AllowedMemberTypes = @("Application")
    $appRoles += $role

    # Updates the application to expose its API publicly and to add an Application Role.
    Update-MgApplication -ApplicationId $domainApiAppRegistration.Id -IdentifierUris @("api://$($domainApiAppRegistration.AppId)") -AppRoles $appRoles
    Write-Host "Domain API App IdentifierUris and Nfield Service Role have been added" -ForegroundColor "Green"
}