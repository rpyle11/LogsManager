using Log.Models;
using LogApi.Entities;

namespace LogApi.Profiles
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<GetLogsView, LogsDto>().ReverseMap();
            CreateMap<GetMinMaxDatesView, MinMaxDatesDto>().ReverseMap();
            CreateMap<Applications, AddUpdateApplicationParameters>().ReverseMap();
            CreateMap<Applications, ApplicationDto>()
                .ForMember(dest => dest.ActiveString, opt => opt.MapFrom(src => src.Active ? "Yes" : "No")).ReverseMap();
        }
    }
}
