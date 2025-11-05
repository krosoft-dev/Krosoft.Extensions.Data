using AutoMapper;
using Krosoft.Extensions.Mapping.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Langues;

public class LanguesProfile : Profile
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="LanguesProfile" />.
    /// </summary>
    public LanguesProfile()
    {
        CreateMap<Langue, LangueDto>()
            .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
            .ForAllOtherMembers(m => m.Ignore());
    }
}