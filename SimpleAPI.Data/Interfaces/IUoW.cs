using SimpleAPI.Data.Entities;
using SimpleAPI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.Data.Interfaces
{
	public interface IUoW
	{
		IEntityRepository<SimplePOCO> SimpleRepository { get; }
		IEntityRepository<SimpleChildPOCO> SimpleChildRepository { get; }

		Task<bool> SaveChangesAsync();
		IEntityRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
	}
}
