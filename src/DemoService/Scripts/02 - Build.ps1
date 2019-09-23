[CmdletBinding()]
param()

Write-Verbose "Building applications.."

Push-Location $PSScriptRoot\..\..\..\scripts

& .\build.ps1

Pop-Location
