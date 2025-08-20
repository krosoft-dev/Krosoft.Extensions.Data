using Bogus;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Factories;

public static class LogicielFactory
{
    public static IEnumerable<Logiciel> GetRandom(int nb, string? tenantId)
    {
        var faker = new Faker<Logiciel>()
                    .RuleFor(p => p.Id, _ => SequentialGuid.NewGuid())
                    .RuleFor(u => u.Nom, (f, _) => f.Company.CompanyName())
                    .RuleFor(u => u.Description, (f, _) => f.Company.CompanyName())
                    .RuleFor(u => u.StatutCode, f => f.PickRandom<StatutCode>())
                    .RuleFor(u => u.DateCreation, f => f.Date.Past())
                    .RuleFor(u => u.TenantId, _ => tenantId);

        return faker.Generate(nb);
    }
}