using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL.Models;

namespace AuthorizationService.Interfaces
{
	public interface IUserService
	{
		UserDAL GetItem(int? id);
		void AddItem(UserDAL item);
		void DeleteItem(UserDAL item);
		public UserDAL Authentificate(string login, string password);
	}
}
