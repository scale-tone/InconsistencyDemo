[CmdletBinding()]
param()

& "$PSScriptRoot\src\DemoService\Scripts\01 - DropAndCreateDatabase.ps1"
& "$PSScriptRoot\src\DemoService\Scripts\02 - Build.ps1"
& "$PSScriptRoot\src\DemoService\Scripts\03 - Run.ps1"
