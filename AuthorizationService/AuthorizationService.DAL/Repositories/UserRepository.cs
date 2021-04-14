using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL.Models;
using System.Linq.Expressions;
using System.Linq;

namespace AuthorizationService.DAL.Repositories
{
	class UserRepository : IRepository<UserDAL>
	{

		protected AuthorizationContext db { get; set; }

		public UserRepository(AuthorizationContext context)
		{
			db = context;
		}

		public UserDAL GetItem(int id)
		{
			return db.Users
				.FirstOrDefault(x => x.Id == id);

		}

		public void Create(UserDAL item)
		{
			db.Users.Add(item);
		}

		public void Update(UserDAL item)
		{
			db.Entry(item).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			UserDAL item = db.Users.Find(id);
			if (item != null)
				db.Users.Remove(item);
		}

		public void Save() => db.SaveChanges();

		public IEnumerable<UserDAL> GetItems(Expression<Func<UserDAL, bool>> predicate)
		{
			return db.Users.Where(predicate);
		}
	}
}
