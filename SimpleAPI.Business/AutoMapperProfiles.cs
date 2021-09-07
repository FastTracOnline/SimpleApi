using AutoMapper;
using SimpleAPI.Data.Entities;
using DTO = SimpleAPI.Public.DTO;

namespace SimpleAPI.Business
{
    public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			AllowNullCollections = true;
			AllowNullDestinationValues = true;

			CreateMap<SimplePOCO, DTO.SimplePOCO>().ReverseMap();
			CreateMap<SimpleChildPOCO, DTO.SimpleChildPOCO>().ReverseMap();
		}
	}
}
