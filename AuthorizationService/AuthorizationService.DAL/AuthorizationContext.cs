using System;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL.Models;

namespace AuthorizationService.DAL
{
	public class AuthorizationContext : DbContext
	{

		public DbSet<UserDAL> Users { get; set; }

		public AuthorizationContext(DbContextOptions options) : base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();
			//this.Seed();

		}
	}
}
