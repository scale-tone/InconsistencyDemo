[CmdletBinding()]
param()

& "$PSScriptRoot\Scripts\01 - DropAndCreateDatabase.ps1"
& "$PSScriptRoot\Scripts\02 - Build.ps1"

& "$PSScriptRoot\Scripts\03 - Run.ps1"
