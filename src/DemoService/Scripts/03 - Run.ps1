[CmdletBinding()]
param()

Write-Verbose "Starting the applications.."

Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\WcfService\bin\Debug\WcfService.exe"
Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\DemoService\bin\Debug\Demoservice.exe"  -Verb runas
Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\MessageCreator\bin\Debug\MessageCreator.exe" -Verb runas
