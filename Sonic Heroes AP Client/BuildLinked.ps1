# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/Sonic_Heroes_AP_Client/*" -Force -Recurse
dotnet publish "./Sonic_Heroes_AP_Client.csproj" -c Release -o "$env:RELOADEDIIMODS/Sonic_Heroes_AP_Client" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location