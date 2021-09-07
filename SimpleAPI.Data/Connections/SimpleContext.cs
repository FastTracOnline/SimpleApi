using Microsoft.EntityFrameworkCore;
using SimpleAPI.Common;
using SimpleAPI.Common.Extensions;
using SimpleAPI.Data.Entities;
using SimpleAPI.Data.EntityConfigurations;
using SimpleAPI.Data.Helpers;
using SimpleAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.String;

namespace SimpleAPI.Data.Connections
{
	public class SimpleContext : DbContext
	{
		public const string ConnectionName = "SimpleContext";
		private readonly string _schema = "dbo";
		private readonly string _connectionString;
		internal bool InsideLINQPad => AppDomain.CurrentDomain.FriendlyName.StartsWith("LINQPad");

		// List of field names that will be NOT be examined nor included in audit log
		private List<string> DoNotAuditFields = new List<string>()
		{
			"createdon",
			"createdby",
			"updatedon",
			"updatedby"
		};

		public SimpleContext(DbContextOptions<SimpleContext> options) : base(options)
		{ }

		public SimpleContext(string connectionString) => _connectionString = connectionString;		// LINQPad ez mode

		public DbSet<AuditLog> AuditLogs { get; set; }
		public DbSet<SimplePOCO> SimpleRecords { get; set; }
		public DbSet<SimpleChildPOCO> SimpleChildRecords { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging(true);

			if (!optionsBuilder.IsConfigured && !IsNullOrWhiteSpace(_connectionString))
				optionsBuilder.UseSqlServer(_connectionString);

			if (InsideLINQPad)
				optionsBuilder.UseLazyLoadingProxies();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(_schema);

			// Entity Configuration via Reflection
			modelBuilder.ApplyConfiguration(new AuditLogConfiguration(_schema));
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var userId = "system";                          // find identifying information for user making change
			ChangeTracker.DetectChanges();
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is IAuditEntity)
				{
					if (entry.State == EntityState.Added)
					{
						_ = entry.Entity.TrySetProperty("CreatedOn", DateTime.Now);
						_ = entry.Entity.TrySetProperty("UpdatedOn", DateTime.Now);
						_ = entry.Entity.TrySetProperty("CreatedBy", userId);
						_ = entry.Entity.TrySetProperty("UpdatedBy", userId);
					}
					else if (entry.State == EntityState.Modified)
					{
						_ = entry.Entity.TrySetProperty("UpdatedOn", DateTime.Now);
						_ = entry.Entity.TrySetProperty("UpdatedBy", userId);
					}
				}

				//TODO: Check for Properties based on IAuditEntry & update
			}

			OnBeforeSaveChanges(userId);                //  pass in identifier for user making change

			return await base.SaveChangesAsync(cancellationToken);
		}

		private void OnBeforeSaveChanges(string userId)
		{
			var auditEntries = new List<AuditEntry>();
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
					continue;

				var auditEntry = new AuditEntry(entry);
				auditEntry.TableName = entry.Entity.GetType().Name;
				auditEntry.UserId = userId;
				auditEntries.Add(auditEntry);

				foreach (var property in entry.Properties)
				{
					string propertyName = property.Metadata.Name;

					if (DoNotAuditFields.Contains(propertyName.ToLowerInvariant()))
						continue;

					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}

					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.AuditType = ChangeType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = ChangeType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;
						case EntityState.Modified:
							if (property.OriginalValue != property.CurrentValue)
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.AuditType = ChangeType.Update;
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							break;
						default:
							auditEntry.AuditType = ChangeType.LogOnly;
							break;
					}
				}
			}
			foreach (var auditEntry in auditEntries)
			{
				AuditLogs.Add(auditEntry.ToAudit());
			}
		}
	}
}
