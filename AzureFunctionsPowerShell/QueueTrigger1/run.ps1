# Input bindings are passed in via param block.
param($QueueItem, $TriggerMetadata)

# Write out the queue message and insertion time to the information log.
Write-Host "PowerShell queue trigger function processed work item: $($QueueItem['Username'])"
Write-Host "Queue item insertion time: $($TriggerMetadata.InsertionTime)"

$UserResult = Get-MgUser -UserId $($QueueItem['Username'])

Write-Host "User Result: $($UserResult.DisplayName)"