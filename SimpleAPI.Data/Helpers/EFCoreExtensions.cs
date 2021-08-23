using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SimpleAPI.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleAPI.Data.Helpers
{
	public static class EFCoreExtensions
	{
		public static IEnumerable<object> FindPrimaryKeyValues<T>(this DbContext dbContext, T entity)
		{
			return from p in dbContext.FindPrimaryKeyProperties(entity)
				   select entity.GetPropertyValue(p.Name);
		}

		public static IEnumerable<string> FindPrimaryKeyNames<T>(this DbContext dbContext, T entity)
		{
			return from p in dbContext.FindPrimaryKeyProperties(entity)
				   select p.Name;
		}

		public static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T entity)
		{
			return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
		}
	}
}
