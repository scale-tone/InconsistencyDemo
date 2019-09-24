[CmdletBinding()]
param()

Write-Verbose "Starting the applications.."

Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\WcfService\bin\Debug\net471\WcfService.exe"
Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\DemoService\bin\Debug\net471\NServiceBus.Host.exe"  -Verb runas
Start-Process cmd -ArgumentList "/c $PSScriptRoot\..\MessageCreator\bin\Debug\net471\MessageCreator.exe" -Verb runas
