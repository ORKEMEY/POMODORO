using System;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL.Models;

namespace AuthorizationService.DAL
{
	public class AuthorizationContext : DbContext
	{

		public DbSet<UserDAL> Users { get; set; }
		public DbSet<TaskDAL> Tasks { get; set; }
		public DbSet<MessageDAL> Messages { get; set; }

		public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();
			//this.Seed();

		}

		public void Seed()
		{

			var firstUser = new UserDAL()
			{
				Login = "first",
				EMail = "pomodoro.kpi.team@gmail.com",
				Password = "first"
			};

			var secondUser = new UserDAL()
			{
				Login = "second",
				EMail = "pomodoro.team@ukr.net",
				Password = "first"
			};

			this.Users.AddRange(firstUser, secondUser);

		}
	}
}
