#-------------------------------------------
# 4-MgGraph-Sample.ps1
#-------------------------------------------

# https://learn.microsoft.com/en-us/powershell/microsoftgraph/authentication-commands?view=graph-powershell-1.0

Connect-MgGraph -Scopes "User.Read.All", "Group.ReadWrite.All" -UseDeviceAuthentication

# Open https://microsoft.com/devicelogin and enter the code

# Connect with certificate
# Connect-MgGraph -ClientId "YOUR_APP_ID" -TenantId "YOUR_TENANT_ID" -CertificateThumbprint "YOUR_CERT_THUMBPRINT"

# Connect-MgGraph -AccessToken $AccessToken

Get-MgContext

Get-mguser -Top 5

# Disconnect-MgGraph