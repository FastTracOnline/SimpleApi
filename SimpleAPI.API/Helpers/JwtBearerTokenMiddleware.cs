using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleAPI.Business;
using SimpleAPI.Data.Connections;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.API.Helpers
{
	public class JwtBearerTokenMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly JwtBearerTokenSettings _jwtSettings;
		private readonly SimpleContext _dbContext;

		public JwtBearerTokenMiddleware(RequestDelegate next, IOptions<JwtBearerTokenSettings> jwtSettings, SimpleContext dbContext)
		{
			_next = next;
			_jwtSettings = jwtSettings.Value;
			_dbContext = dbContext;
		}

		public async Task Invoke(HttpContext context)
		{
			var token = context.Request.Headers["authorization"].FirstOrDefault()?.Split(" ").Last();

			// Commented to allow for local testing
			//if (!context.User.Identity.IsAuthenticated)
			//	token = null;

			if (token != null)
				await attachAccountToContext(context, token);

			await _next(context);
		}

		private async Task attachAccountToContext(HttpContext context, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = _jwtSettings.Issuer,
					ValidateAudience = true,
					ValidAudience = _jwtSettings.Audience,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				// Extract information here using the validated token

				// Attach information about logged in user to context.  If SimpleUser isn't attached, no further access allowed
				context.Items["SimpleUser"] = "stuff";
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
