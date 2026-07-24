using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits;

public class ProduitsProfile : Profile
{
    public ProduitsProfile()
    {
        CreateMap<Produit, ProduitDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Metadata!.Description))
            .ForAllOtherMembers(m => m.Ignore());
    }
}
