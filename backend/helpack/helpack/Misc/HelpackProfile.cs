using helpack.Data.Entities;
using helpack.DTO;
using Profile = AutoMapper.Profile;

namespace helpack.Misc;

public class HelpackProfile : Profile
{
    public HelpackProfile()
    {
        CreateMap<Data.Entities.Profile, ProfileCardViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.Reached, opt => opt.MapFrom(src => (int)((src.DonationsRaised ?? 0) / src.Goal * 100)))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (src.Category ?? ProfileCategory.Unset).GetDisplayName()));
        
        CreateMap<Data.Entities.Profile, ProfileInsightsViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.Author.RegistrationDate.ToString("dd.MM.yyyy")));

        CreateMap<Data.Entities.Profile, ProfileSettingsViewModel>().ReverseMap();

        CreateMap<Data.Entities.Profile, ProfileViewModel>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (src.Category ?? ProfileCategory.Unset).GetDisplayName()));
        
        CreateMap<Data.Entities.Profile, ProfileDonateViewModel>();

        CreateMap<Data.Entities.Profile, ProfileUpdateModel>().ReverseMap();
    }
}