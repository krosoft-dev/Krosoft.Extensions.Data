using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Samples.Library.Factories;

public static class CompteFactory
{
    public static Task<IEnumerable<Compte>> ToCompteAsync(IEnumerable<string> strings)
    {
        var comptes = new List<Compte>();
        foreach (var s in strings)
        {
            comptes.Add(new Compte
            {
                Name = s
            });
        }

        return Task.FromResult(comptes.AsEnumerable());
    }

    public static IEnumerable<Compte> GetRandom(int nb)
    {
        var faker = new Faker<Compte>()
                    .RuleFor(o => o.Id, _ => Guid.NewGuid().ToString())
                    .RuleFor(u => u.Name, (f, _) => f.Company.CompanyName());

        return faker.Generate(nb);
    }
}