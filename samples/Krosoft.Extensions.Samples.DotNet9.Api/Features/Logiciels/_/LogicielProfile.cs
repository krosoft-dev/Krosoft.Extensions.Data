using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public class LogicielProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="LogicielProfile" />.
    /// </summary>
    public LogicielProfile()
    {
        CreateMap<Logiciel, LogicielCsvDto>()
            .ForMember(dest => dest.Nom, o => o.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
            .ForAllOtherMembers(m => m.Ignore());
    }
}