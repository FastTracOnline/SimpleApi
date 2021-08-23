using System;
using System.Collections.Generic;
using System.Text;
using static System.String;

namespace SimpleAPI.Data.Entities
{
	public abstract class VersionedEntity
	{
		public VersionedEntity(string userId = null)
		{
			CreatedOn = DateTime.Now;
			UpdatedOn = DateTime.Now;

			if (!IsNullOrWhiteSpace(userId))
			{
				CreatedBy = userId;
				UpdatedBy = userId;
			}
		}

		public bool Archived { get; set; }						// for "soft" deletes or marking a record to be archived
		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public byte[] Version { get; set; }						// For concurrency checks
	}
}
