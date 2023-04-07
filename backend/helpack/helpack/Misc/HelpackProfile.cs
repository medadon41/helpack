using AutoMapper;
using helpack.DTO;

namespace helpack.Misc;

public class HelpackProfile : Profile
{
    public HelpackProfile()
    {
        CreateMap<Data.Entities.Profile, ProfileCardViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.ProgressReached, opt => opt.MapFrom(src => (int)(src.DonationsRaised / src.Goal)));
        CreateMap<Data.Entities.Profile, ProfileInsightsViewModel>();
        CreateMap<Data.Entities.Profile, ProfileSettingsViewModel>();
        CreateMap<Data.Entities.Profile, ProfileViewModel>();
    }
}