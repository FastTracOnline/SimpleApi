using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAPI.Business
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			AllowNullCollections = true;
			AllowNullDestinationValues = true;
		}
	}
}
