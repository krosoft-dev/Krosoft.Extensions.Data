using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.GetAll;

public class LogicielsProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="LogicielsProfile" />.
    /// </summary>
    public LogicielsProfile()
    {
        CreateMap<Logiciel, LogicielDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Nom))
            .ForMember(dest => dest.GroupeNom, o => o.MapFrom(src => src.Groupe!.Name))
            .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
            .ForMember(dest => dest.StatutCode, o => o.MapFrom(src => src.StatutCode))
            .ForMember(dest => dest.CreationDate, o => o.MapFrom(src => src.CreatedAt))
            .ForAllOtherMembers(m => m.Ignore());
    }
}