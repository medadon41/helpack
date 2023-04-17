using helpack.Data.Entities;
using helpack.DTO;
using Profile = AutoMapper.Profile;

namespace helpack.Misc;

public class DonationProfile : Profile
{
    public DonationProfile()
    {
        CreateMap<Donation, DonationScoreboardViewModel>();
        CreateMap<Donation, DonationViewModel>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")));
    }
}