﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BooksApi.Models;
using BooksApi.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace BooksApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime to add services to the container
		public void ConfigureServices(IServiceCollection services)
		{
			// Register in the dependency injection container the configuration instance that appsettings.json's BookstoreDatabaseSettings section binds to
			services.Configure<BookstoreDatabaseSettings>(Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

			// Register IBookstoreDatabaseSettings in DI container with a singleton service lifetime
			services.AddSingleton<IBookstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime to configure the HTTP request pipeline
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{	
				app.UseHsts(); // Default HSTS value is 30 days
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
