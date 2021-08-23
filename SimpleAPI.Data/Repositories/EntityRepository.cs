using Microsoft.EntityFrameworkCore;
using SimpleAPI.Data.Connections;
using SimpleAPI.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace SimpleAPI.Data.Repositories
{
	public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
	{
		public EntityRepository(SimpleContext dbContext)
		{
			if (dbContext == null)
				throw new ArgumentNullException(nameof(dbContext));

			_dbContext = dbContext;
		}

		protected SimpleContext _dbContext { get; set; }
		protected DbCommand _dbCommand { get; set; }
		protected DbSet<TEntity> _dbSet { get; set; }

		#region Retrieve List
		public virtual async Task<ICollection<TEntity>> GetAll()
		{
			var result = await _dbSet.ToListAsync();
			return result;
		}

		public virtual async Task<ICollection<TEntity>> GetMatching(Expression<Func<TEntity, bool>> predicate,
			string included, string orderBy)
		{
			var query = _dbSet.Where(predicate);

			if (!IsNullOrWhiteSpace(included))
			{
				var children = included.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var item in children)
				{
					if (typeof(TEntity).GetProperty(item) != null)
						query.Include(item);
				}
			}


			if (!IsNullOrWhiteSpace(orderBy))
			{
				var sortBy = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
				int i = 0;
				foreach (var item in sortBy)
				{
					//if (typeof(TEntity).GetProperty(item) != null)
					//{
					//	if (i == 0)
					//		result = result.OrderBy(item);
					//	else
					//		result = result.ThenBy(item);
					//}
					i++;
				}
			}

			var result = await query.ToListAsync();
			return result;
		}
		#endregion Retrieve List

		#region Get Single Record
		public virtual async Task<TEntity> GetOne(params object[] key)
		{
			var result = await _dbSet.FindAsync(key);
			return result;
		}

		public virtual async Task<TEntity> GetFirstMatch(Expression<Func<TEntity, bool>> predicate,
			string included)
		{
			var recordSet = await GetMatching(predicate, included, null);

			if (recordSet != null && recordSet.Count() > 0)
				return recordSet.First();
			else
				return null;
		}
		#endregion Get Single Record

		public virtual async Task<TEntity> Create(TEntity newRecord)
		{
			var result = await _dbSet.AddAsync(newRecord);
			return result.Entity;
		}

		public virtual async Task<bool> Update(TEntity changes)
		{
			var keyFields = GetPrimaryKey(changes);
			TEntity oldRecord = await GetOne(keyFields.ToArray());

			if (oldRecord != null)
				_dbContext.Entry(oldRecord).CurrentValues.SetValues(changes);
			else
				throw new KeyNotFoundException(nameof(keyFields));

			return true;
		}

		public virtual async Task<bool> Delete(params object[] key)
		{
			TEntity oldRecord = await GetOne(key);

			if (oldRecord != null)
			{
				_dbSet.Remove(oldRecord);
				return true;
			}
			else
				throw new KeyNotFoundException(nameof(key));
		}

		#region Helper Function(s)
		public IEnumerable<object> GetPrimaryKey(TEntity entity)
		{
			var KeyValues = _dbContext.FindPrimaryKeyValues(entity);

			return KeyValues;
		}
		#endregion Helper Function(s)

		#region IDisposable
		public void Dispose()
		{
			_dbCommand.Dispose();
			_dbContext.Dispose();
		}
		#endregion IDisposable
	}
}
