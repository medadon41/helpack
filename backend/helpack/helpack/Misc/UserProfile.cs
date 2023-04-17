using helpack.Data.Entities;
using helpack.DTO;
using Profile = AutoMapper.Profile;

namespace helpack.Misc;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<HelpackUser, UserLoginViewModel>();
    }
}