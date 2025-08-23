using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.NewFolder1;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Messages;
using Krosoft.Extensions.Testing.Cqrs.Extensions;
using Krosoft.Extensions.Testing.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Testing.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Unit.Handlers.Commands;

[TestClass]
public class UpdateStatLogicielCommandHandlerTests : SampleBaseTest<Krosoft.Extensions.Samples.DotNet9.Api.Program>
{
    //TestInitialize
    private Mock<ILogger<UpdateStatLogicielCommandHandler>> _mockLogger = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        _mockLogger = new Mock<ILogger<UpdateStatLogicielCommandHandler>>();
        services.SwapTransient(_ => _mockLogger.Object);
        services.AddRepositories();
        services.AddScoped<ITenantDbContextProvider<string>, FakeTenantDbContextProvider>();
        services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
        services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
        services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();

        base.AddServices(services, configuration);
    }

    [TestMethod]
    public async Task Handle_Empty()
    {
        await using var serviceProvider = CreateServiceCollection();
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);

        var command = new UpdateStatLogicielCommand(string.Empty);
        await this.SendCommandAsync(serviceProvider, command);

        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques...", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques pour le tenant", Times.Never());
        _mockLogger.VerifyWasCalled(LogLevel.Error, "Impossible de mettre à jour les statitisques à partir du payload : ", Times.Once());
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);
    }

    [TestMethod]
    public async Task Handle_NoLogicielWithTenant_Ok()
    {
        await using var serviceProvider = CreateServiceCollection();
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);

        var message = new UpdateStatLogicielMessage
        {
            TenantId = "Fake_Tenant_Id",
            UtilisateurId = "utilisateur_1"
        };

        var payload = JsonConvert.SerializeObject(message);
        var command = new UpdateStatLogicielCommand(payload);
        await this.SendCommandAsync(serviceProvider, command);

        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques...", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Information, $"Mise à jour des statistiques pour le tenant {message.TenantId}", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Error, "Impossible de mettre à jour les statitisques à partir du payload : ", Times.Never());

        var statistiques = this.GetDb<Statistique>(serviceProvider).IgnoreQueryFilters().ToList();
        Check.That(statistiques).HasSize(1);

        var statistique = statistiques.First();
        Check.That(statistique).IsNotNull();
        Check.That(statistique.Nom).IsEqualTo("Fake_Tenant_Id");
        Check.That(statistique.Nombre).IsEqualTo(5);
    }

    [TestMethod]
    public async Task Handle_Null()
    {
        await using var serviceProvider = CreateServiceCollection();
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);

        var command = new UpdateStatLogicielCommand(null!);
        await this.SendCommandAsync(serviceProvider, command);

        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques...", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques pour le tenant", Times.Never());
        _mockLogger.VerifyWasCalled(LogLevel.Error, "Impossible de mettre à jour les statitisques à partir du payload : ", Times.Once());
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);
    }

    [TestMethod]
    public async Task Handle_Ok()
    {
        await using var serviceProvider = CreateServiceCollection();
        Check.That(this.GetDb<Statistique>(serviceProvider).Count()).IsEqualTo(0);

        Check.That(this.GetDb<Logiciel>(serviceProvider).Count()).IsEqualTo(5);

        var tenantId = new FakeTenantDbContextProvider().GetTenantId();
        var message = new UpdateStatLogicielMessage
        {
            TenantId = tenantId,
            UtilisateurId = "utilisateur_1"
        };

        var payload = JsonConvert.SerializeObject(message);
        var command = new UpdateStatLogicielCommand(payload);
        await this.SendCommandAsync(serviceProvider, command);

        _mockLogger.VerifyWasCalled(LogLevel.Information, "Mise à jour des statistiques...", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Information, $"Mise à jour des statistiques pour le tenant {message.TenantId}", Times.Once());
        _mockLogger.VerifyWasCalled(LogLevel.Error, "Impossible de mettre à jour les statitisques à partir du payload : ", Times.Never());

        var statistiques = this.GetDb<Statistique>(serviceProvider).IgnoreQueryFilters().ToList();
        Check.That(statistiques).HasSize(1);

        var statistique = statistiques.First();
        Check.That(statistique).IsNotNull();
        Check.That(statistique.Nom).IsEqualTo(tenantId);
        Check.That(statistique.Nombre).IsEqualTo(5);
    }
}