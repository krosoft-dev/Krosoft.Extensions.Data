using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.Abstractions.Extensions;

public static class CrudBusinessExtensions
{
    public static bool Any<T>(this CrudBusiness<T>? crudBusiness)
        => crudBusiness != null &&
           (
               crudBusiness.ToAdd.Any() ||
               crudBusiness.ToUpdate.Any() ||
               crudBusiness.ToDelete.Any());
}