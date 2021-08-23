using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAPI.API.Helpers
{
	public static class StartupExtensions
	{
		public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<JwtBearerTokenMiddleware>();
		}

		public static IApplicationBuilder UseUnhandledExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<UnhandledExceptionMiddleware>();
		}
	}
}
