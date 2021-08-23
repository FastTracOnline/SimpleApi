using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using SimpleAPI.Business;
using SimpleAPI.Data.Connections;
using SimpleAPI.Data.Entities;
using SimpleAPI.Data.Interfaces;
using SimpleAPI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleAPI.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<APISettings>(Configuration.GetSection("APIConfiguration"));
			services.Configure<JwtBearerTokenSettings>(Configuration.GetSection("JwtTokenConfiguration"));

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleAPI.API", Version = "v1" });

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = $"{AppContext.BaseDirectory}{xmlFile}";

				c.IncludeXmlComments(xmlPath);
			});

			services.AddAutoMapper(typeof(AutoMapperProfiles));

			services.AddDbContext<SimpleContext>(opts =>
			{
				opts.UseSqlServer(Configuration.GetConnectionString(SimpleContext.ConnectionName),
					options => { options.MigrationsHistoryTable("DBMigrationsHistory", "dbo"); });
			});

			services.TryAddScoped<IUoW, UoW>();
			services.TryAddScoped<IEntityRepository<SimplePOCO>, EntityRepository<SimplePOCO>>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleAPI.API v1"));
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseStaticFiles();

			app.UseSerilogRequestLogging();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
