using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data;

public class SampleKrosoftTenantAuditableContext : KrosoftTenantAuditableContext 
{
    public SampleKrosoftTenantAuditableContext(DbContextOptions options,
                                               ITenantDbContextProvider tenantDbContextProvider,
                                               IAuditableDbContextProvider auditableDbContextProvider)
        : base(options,
               tenantDbContextProvider,
               auditableDbContextProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDataJson<Statistique>();
        modelBuilder.HasDataJson<Logiciel>();
        modelBuilder.HasDataJson<Langue>();
        modelBuilder.HasDataJson<Pays>();
    }
}