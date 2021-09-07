using SimpleAPI.Data.Connections;
using SimpleAPI.Data.Entities;
using SimpleAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.Data.Repositories
{
	public class UoW : IUoW, IDisposable
	{
		public UoW(IServiceProvider provider)
		{
			Provider = provider;
			Context = (SimpleContext)Provider.GetService(typeof(SimpleContext));
		}

		private bool disposed { get; set; } = false;

		private IServiceProvider Provider { get; }
		private readonly SimpleContext Context;

		private IEntityRepository<SimplePOCO> _simpleRepository { get; set; }

		public IEntityRepository<SimplePOCO> SimpleRepository
		{
			get
			{
				_simpleRepository ??= (IEntityRepository<SimplePOCO>)Provider.GetService(typeof(IEntityRepository<SimplePOCO>));
				return _simpleRepository;
			}
		}

		private IEntityRepository<SimpleChildPOCO> _simpleChildRepository { get; set; }

		public IEntityRepository<SimpleChildPOCO> SimpleChildRepository
		{
			get
			{
				_simpleChildRepository ??= (IEntityRepository<SimpleChildPOCO>)Provider.GetService(typeof(IEntityRepository<SimpleChildPOCO>));
				return _simpleChildRepository;
			}
		}

		public IEntityRepository<TEntity> GetRepository<TEntity>() where TEntity : class
		{
			if (typeof(TEntity) == typeof(SimplePOCO))
				return (IEntityRepository<TEntity>)SimpleRepository;

			if (typeof(TEntity) == typeof(SimpleChildPOCO))
				return (IEntityRepository<TEntity>)SimpleChildRepository;

			return null;
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await Context.SaveChangesAsync()) > 0;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_simpleChildRepository = null;
					_simpleRepository = null;
				}

				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
