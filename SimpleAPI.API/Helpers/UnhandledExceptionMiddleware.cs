using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAPI.API.Helpers
{
	public class UnhandledExceptionMiddleware
	{
		private readonly ILogger<UnhandledExceptionMiddleware> logger;
		private readonly RequestDelegate next;

		public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware> logger, RequestDelegate next)
		{
			this.logger = logger;
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await next(context);

				// You can also examine the Request object, Response object / log / reformat
			}
			catch (Exception ex)
			{
				logger.LogError(ex,
					$"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
				await HandleExceptionAsync(context, ex, logger);
			}
		}

		public static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<UnhandledExceptionMiddleware> _logger)
		{
			var response = context.Response;
			const string description = "Unexpected error";
			string message = "";

			switch (ex)
			{
				case ApplicationException e:
					{
						response.StatusCode = (int)StatusCodes.Status400BadRequest;
						message = e?.Message;
					}
					break;
				case KeyNotFoundException e:
					{
						response.StatusCode = (int)StatusCodes.Status404NotFound;
						message = e?.Message;
					}
					break;
				default:
					{
						response.StatusCode = (int)StatusCodes.Status500InternalServerError;
						message = ex.Message;
					}
					break;
			}

			_logger.LogError(ex, $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");

			response.ContentType = "application/json";
			response.Headers.Add("exception", "messageException");

			response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse { Message = message, Description = description });
		}
	}

	public class CustomErrorResponse
	{
		public string Message { get; set; }
		public string Description { get; set; }
	}
}
