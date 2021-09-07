using SimpleAPI.Common;
using System;

namespace SimpleAPI.Public.DTO
{
    public partial class SimpleChildPOCO 
	{
		public SimpleChildPOCO() 
		{
			Id = Guid.NewGuid();
			MyEnumField = SimpleEnum.Unknown;
		}

		public Guid Id { get; set; }			// Surrogate keys should be unguessable and not reveal the data storage
		public string MyField { get; set; }
		public SimpleEnum MyEnumField { get; set; }
		public Guid MyParentId { get; set; }
    }
}
