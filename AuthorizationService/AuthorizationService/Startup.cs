using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using AuthorizationService.DAL;
using AuthorizationService.Interfaces;
using AuthorizationService.Services;
using Prometheus;


namespace AuthorizationService
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
			services.AddDbContext<AuthorizationContext>();
			services.AddControllers();
			services.AddTransient<IUserService, UserService>();
		#if DEBUG
			services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(ServiceModule.GetDbContextOptions("DevConnection")));
		#else
			services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(ServiceModule.GetDbContextOptions())); 
		#endif
			//services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(ServiceModule.GetDbContextOptions()));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseMetricServer(url: "/metrics");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
