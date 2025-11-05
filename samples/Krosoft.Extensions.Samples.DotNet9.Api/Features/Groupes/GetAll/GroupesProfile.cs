using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.GetAll;

public class GroupesProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="GroupesProfile" />.
    /// </summary>
    public GroupesProfile()
    {
        CreateMap<Groupe, GroupeDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Name))
            .ForMember(dest => dest.LogicielsCount, o => o.MapFrom(src => src.Logiciels.Count()))
            .ForMember(dest => dest.CreationDate, o => o.MapFrom(src => src.CreatedAt))
            .ForAllOtherMembers(m => m.Ignore());
    }
}