# Nuget package deployment

## Steps

1. Create folder '..\tools' if not exist and download nuget.exe there
2. Open .\upload-updated-packages.ps1 and set api key
3. Open PowerShell
4. Navigate to 'nuget scripts' folder of repository
5. Use following command .\upload-updated-packages.ps1 it checks whether package version has been updated if yes then builds it and uploads to nuget
