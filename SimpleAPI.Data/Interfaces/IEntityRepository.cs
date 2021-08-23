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
	public interface IEntityRepository<TEntity> where TEntity : class
	{
		Task<ICollection<TEntity>> GetAll();
		Task<ICollection<TEntity>> GetMatching(Expression<Func<TEntity, bool>> predicate, string included, string orderBy);

		Task<TEntity> GetOne(params object[] key);
		Task<TEntity> GetFirstMatch(Expression<Func<TEntity, bool>> predicate, string included);

		Task<TEntity> Create(TEntity newRecord);
		Task<bool> Update(TEntity changes);
		Task<bool> Delete(params object[] key);
	}
}
