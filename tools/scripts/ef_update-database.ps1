$context = "SampleKrosoftContext"
$startupProject = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$projet = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$verbose = $true

Write-Host -fore Green "=========================================="
Write-Host -fore Green "EF : Update Database"
Write-Host -fore Green "==========================================" 
Write-Host -fore Blue "Projet         : "$projet
Write-Host -fore Blue "StartupProject : "$startupProject
Write-Host -fore Blue "Context        : "$context
Write-Host -fore Green "=========================================="

$verboseFlag = if ($verbose -eq $true) { "--verbose" } else { "" }
dotnet ef database update --context $context -s $startupProject -p $projet $verboseFlag
 