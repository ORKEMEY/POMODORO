using System;
using System.Collections.Generic;
using System.Text;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL.Repositories;
namespace AuthorizationService.DAL
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<UserDAL> Users { get; }
		void Save();
	}
}
