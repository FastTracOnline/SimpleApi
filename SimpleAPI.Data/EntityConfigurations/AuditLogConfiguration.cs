using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.String;

namespace SimpleAPI.Data.EntityConfigurations
{
	public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
	{
		private readonly string _schema;
		private const string TableName = "AuditLog";

		public AuditLogConfiguration(string schema)
		{ 
			_schema = schema; 
		}

		public void Configure(EntityTypeBuilder<AuditLog> builder)
		{
			if (!IsNullOrWhiteSpace(_schema))
				builder.ToTable(TableName, _schema);
			else
				builder.ToTable(TableName);

			//builder.Property(e => e.Id).ValueGeneratedNever();      // Application responsibility (Guids)
			builder.Property(e => e.Id).ValueGeneratedOnAdd();      // Database provider responsibility (SQL Identity)
			builder.Property(e => e.TableName).IsUnicode(false).HasMaxLength(50).IsRequired();
			builder.Property(e => e.Type).IsUnicode(false).HasMaxLength(10).IsRequired();
			builder.Property(e => e.UserId).IsUnicode(false).HasMaxLength(10).IsRequired();
			builder.Property(e => e.DateTime).HasColumnType("datetime2");				// higher precision
			builder.HasKey(e => e.Id).HasName($"PK_{TableName}_Id");
		}
	}
}
