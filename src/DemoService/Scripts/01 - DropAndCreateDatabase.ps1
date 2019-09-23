[CmdletBinding()]
param()

Write-Verbose "Creating database.."

<#
# Propmt only if database already exists
#>
if ($null -eq (Invoke-Sqlcmd -Query "SELECT name FROM sys.databases WHERE name = N'InconsistencyDemo'"))
{
    Invoke-Sqlcmd -InputFile $PSScriptRoot\..\Database\DropAndCreateDatabase.sql
}
else
{
    $message = 'This will drop and recreate the existing database.'
    $question = 'Are you sure you want to proceed?'
    $choices = '&Yes', '&No'
    
    $decision = $Host.UI.PromptForChoice($message, $question, $choices, 1)
    if ($decision -eq 0)
    {
        Write-Host 'Dropping and recreating the database...'
        Invoke-Sqlcmd -InputFile $PSScriptRoot\..\Database\DropAndCreateDatabase.sql
    }
    else
    {
        Write-Host 'Skipped database drop and create...'
    }
}

