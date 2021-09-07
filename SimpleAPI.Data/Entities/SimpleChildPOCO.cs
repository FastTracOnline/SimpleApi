using SimpleAPI.Common;
using SimpleAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SimpleAPI.Data.Entities
{
	public partial class SimpleChildPOCO : VersionedEntity, IAuditEntity
	{
		public SimpleChildPOCO() : base()
		{
			Id = Guid.NewGuid();
			MyEnumField = SimpleEnum.Unknown;
		}

		[Display(Name="Primary Key")]
		public Guid Id { get; set; }			// Surrogate keys should be unguessable and not reveal the data storage
		public string MyField { get; set; }
		public SimpleEnum MyEnumField { get; set; }

        // EF Navigation Fields
		public Guid MyParentId { get; set; }
		[ForeignKey("MyParentId")]
        public virtual SimplePOCO MyParent { get; set; }

        // Calculated Properties
    }
}
