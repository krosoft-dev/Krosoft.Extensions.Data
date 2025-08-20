using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Samples.Library.Factories;

public static class AddresseFactory
{
    public static IEnumerable<Addresse> GetAdresses()
    {
        var address1 = new Addresse("street1Line1", "street1Line2", "city1", "zipcode1");
        var address2 = new Addresse("street2Line1", "street2Line2", "city2", "zipcode2");
        var address3 = new Addresse("street3Line1", "street3Line2", "city3", "zipcode3");
        var address4 = new Addresse("street4Line1", "street4Line2", "city4", "zipcode4");
        var address5 = new Addresse("street5Line1", "street5Line2", "city", "zipcode5");
        var address6 = new Addresse("street6Line1", "street6Line2", "city", "zipcode6");

        IEnumerable<Addresse> adresses = new List<Addresse> { address3, address4, address5, address1, address2, address6 };
        return adresses;
    }
}