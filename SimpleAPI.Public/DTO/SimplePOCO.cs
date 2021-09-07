using SimpleAPI.Common;
using System;
using System.Collections.Generic;

namespace SimpleAPI.Public.DTO
{
    public partial class SimplePOCO
	{
		public SimplePOCO() 
		{
			Id = Guid.NewGuid();
			MyEnumField = SimpleEnum.Unknown;
            MyChildren = new List<SimpleChildPOCO>();
        }

		public Guid Id { get; set; }			// Surrogate keys should be unguessable and not reveal the data storage
		public string MyField { get; set; }
		public SimpleEnum MyEnumField { get; set; }

        // EF Navigation Fields
        public List<SimpleChildPOCO> MyChildren { get; set; }

        // Calculated Properties
        public int ChildrenCount => MyChildren?.Count ?? 0;
    }
}
