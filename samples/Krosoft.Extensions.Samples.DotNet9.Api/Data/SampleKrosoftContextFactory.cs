using Krosoft.Extensions.Data.EntityFramework.PostgreSql.Services;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data;

internal class SampleKrosoftContextFactory : PostgreSqlDesignTimeDbContextFactory<SampleKrosoftContext>;