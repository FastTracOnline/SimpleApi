using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.Tests
{
	public class SharedDatabaseFixture : IDisposable
	{
		private static readonly object _lock = new object();
		private static bool _databaseInitialized;
		private static string connectionString = @"data source=(localdb)\MSSQLLocalDB;initial catalog=SimpleTests;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

		public SharedDatabaseFixture()
		{
			var services = new ServiceCollection();

		}
	}
}
