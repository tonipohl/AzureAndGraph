#-------------------------------------------
# 3-GetUsers.ps1
#-------------------------------------------

$result = Invoke-RestMethod `
        -Method GET `
        -Uri "https://graph.microsoft.com/v1.0/users?$top=2" `
        -ContentType 'application/json' `
        -Headers $script:APIHeader `
        -ErrorAction Stop

# Show the result
# $result
$result.value
