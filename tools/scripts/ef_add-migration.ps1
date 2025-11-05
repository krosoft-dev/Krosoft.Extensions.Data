$migration = Read-Host "Nom de la migration"
$context = "SampleKrosoftContext"
$startupProject = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$projet = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$verbose = $true

Write-Host -fore Green "=========================================="
Write-Host -fore Green "EF : Add Migration"
Write-Host -fore Green "=========================================="
Write-Host -fore Blue "Projet         : "$projet
Write-Host -fore Blue "StartupProject : "$startupProject
Write-Host -fore Blue "Context        : "$context
Write-Host -fore Blue "Migration      : "$migration
Write-Host -fore Green "=========================================="

if ([string]::IsNullOrEmpty($migration )) {
    Write-Host -fore red "ERROR : Migration is empty !"
    exit
}  

$verboseFlag = if ($verbose -eq $true) { "--verbose" } else { "" }
dotnet ef migrations add $migration --context $context -s $startupProject -p $projet $verboseFlag