using namespace System.Net
# Input bindings are passed in via param block.
param($Request, $TriggerMetadata)

# Write to the Azure Functions log stream.
Write-Host "PowerShell HTTP trigger function processed a request."

# Interact with query parameters or the body of the request.
Write-Host "Request: $($Request.Body.UserName)"

# Get a specific user object
$UserResult = Get-MgUser -UserId $Request.body.UserName

# Get a list of all groups in the tenant
# $GroupResult = Get-MgGroup -All

# Associate values to output bindings by calling 'Push-OutputBinding'.
Push-OutputBinding -Name Response -Value ([HttpResponseContext]@{
    StatusCode = [HttpStatusCode]::OK
    Body = $UserResult
})
