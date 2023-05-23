#-------------------------------------------
# 5-Execute-Invoke.ps1
#-------------------------------------------

# Note: To get the lastsignin date, we need MSGraph permissions AuditLog.Read.All
# GetUsers tpe5
$uri = "<your-endpoint>"
Invoke-WebRequest -Uri $uri -Method Post

# GetUsers176
# $uri = "https://prod-126.westeurope.logic.azure.com:443/workflows/35e56e57fa2644a69fdc3d27bbd1e5b0/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=tr-FdgpRWOT7yy9eO7y4UAa5f2e0Kw046URhmsxZNKs"
# Invoke-WebRequest -Uri $uri -Method Post

# Execute the deployed Azure Function with a parameter
$uri = '<your-endpoint>'
$body = '{"username": "AdeleV@<your-tenant>.onmicrosoft.com"}'
$result = Invoke-RestMethod -Uri $uri -Method Post -Body $body -ContentType 'application/json'
$result

# Execute the deployed Azure Function with a queue item...

