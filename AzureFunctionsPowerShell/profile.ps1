#-------------------------------------------
# profile.ps1
# runs automatically when the PowerShell session starts
#-------------------------------------------

# Use this line for system managed identity
Write-output 'Connecting to Azure using system managed identity'
Connect-AzAccount -Identity

# use this line for user managed identity (Client ID - is set through application settings)
# Connect-AzAccount -Identity -AccountId $env:CLIENTID

# Get the Azure token of the manage identity and exchange it against a Graph token with some reflection magix
$context = [Microsoft.Azure.Commands.Common.Authentication.Abstractions.AzureRmProfileProvider]::Instance.Profile.DefaultContext
$graphToken = [Microsoft.Azure.Commands.Common.Authentication.AzureSession]::Instance.AuthenticationFactory.Authenticate($context.Account, $context.Environment, $context.Tenant.Id.ToString(), $null, [Microsoft.Azure.Commands.Common.Authentication.ShowDialog]::Never, $null, "https://graph.microsoft.com").AccessToken
$aadToken = [Microsoft.Azure.Commands.Common.Authentication.AzureSession]::Instance.AuthenticationFactory.Authenticate($context.Account, $context.Environment, $context.Tenant.Id.ToString(), $null, [Microsoft.Azure.Commands.Common.Authentication.ShowDialog]::Never, $null, "https://graph.windows.net").AccessToken
    
Write-Output "Hi I'm $($context.Account.Id)"
    
# Connect to AAD to use Azure AD Graph
# Connect-AzureAD -AadAccessToken $aadToken -AccountId $context.Account.Id -TenantId $context.tenant.id

# To use MS Graph use the below line
Connect-MgGraph -AccessToken $graphToken
