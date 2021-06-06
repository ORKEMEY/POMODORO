using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL.Repositories;
using Messages.Interfaces;
using Messages.Services;

namespace Messages
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AuthorizationContext>();
			services.AddControllers();
			services.AddTransient<IMessageService, MessageService>();
#if DEBUG
			services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(ServiceModule.GetDbContextOptions("DevConnection")));
#else
			services.AddTransient<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(ServiceModule.GetDbContextOptions())); 
#endif
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
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
