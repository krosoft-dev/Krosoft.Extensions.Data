$context = "SampleKrosoftContext"
$startupProject = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$projet = ".\samples\Krosoft.Extensions.Samples.DotNet9.Api"
$output = ".\temp\migrations.sql"
$verbose = $true

Write-Host -fore green "=========================================="
Write-Host -fore green "EF : Update Database"
Write-Host -fore Green "=========================================="
Write-Host -fore Blue "Projet         : "$projet
Write-Host -fore Blue "StartupProject : "$startupProject
Write-Host -fore Blue "Context        : "$context
Write-Host -fore Green "=========================================="

$verboseFlag = if ($verbose -eq $true) { "--verbose" } else { "" }
dotnet ef migrations script -v --idempotent --output $output --context $context -s $startupProject -p $projet $verboseFlag

 