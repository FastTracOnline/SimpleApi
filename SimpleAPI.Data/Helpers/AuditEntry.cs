using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SimpleAPI.Common;
using SimpleAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAPI.Data.Helpers
{
	public class AuditEntry
	{
		public AuditEntry(EntityEntry entry)
		{
			Entry = entry;
		}

		public EntityEntry Entry { get; set; }
		public string UserId { get; set; }
		public string TableName { get; set; }
		public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
		public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
		public ChangeType AuditType { get; set; }
		public List<string> ChangedColumns { get; } = new List<string>();

		public AuditLog ToAudit()
		{
			var audit = new AuditLog
			{
				UserId = UserId,
				Type = AuditType.ToString(),
				TableName = TableName,
				DateTime = DateTime.Now,
				PrimaryKey = JsonConvert.SerializeObject(KeyValues),
				OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
				NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
				AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)
			};
			return audit;
		}
	}
}
