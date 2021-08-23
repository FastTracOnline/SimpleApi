using SimpleAPI.Common;
using SimpleAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleAPI.Data.Entities
{
	public partial class SimplePOCO : VersionedEntity, IAuditEntity
	{
		public SimplePOCO() : base()
		{
			Id = Guid.NewGuid();
			MyEnumField = SimpleEnum.Unknown;
			//MyChildren = new HashSet<SimpleChildPOCO>();
		}

		[Display(Name="Primary Key")]
		public Guid Id { get; set; }			// Surrogate keys should be unguessable and not reveal the data storage
		public string MyField { get; set; }
		public SimpleEnum MyEnumField { get; set; }

		// EF Navigation Fields
		//public virtual ICollection<SimpleChildPOCO> MyChildren { get; set; }

		// Calculated Properties
		//public int ChildrenCount => MyChildren?.Count ?? 0;
	}
}
