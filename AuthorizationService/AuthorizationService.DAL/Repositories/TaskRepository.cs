using System;
using System.Collections.Generic;
using AuthorizationService.DAL.Models;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.DAL.Repositories
{
	class TaskRepository : IRepository<TaskDAL>
	{
		protected AuthorizationContext db { get; set; }

		public TaskRepository(AuthorizationContext context)
		{
			db = context;
		}

		public TaskDAL GetItem(int id)
		{
			return db.Tasks
				.FirstOrDefault(x => x.Id == id);
		}

		public void Create(TaskDAL item)
		{
			db.Tasks.Add(item);
		}

		public void Update(TaskDAL item)
		{
			db.Entry(item).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			TaskDAL item = db.Tasks.Find(id);
			if (item != null)
				db.Tasks.Remove(item);
		}

		public void Save() => db.SaveChanges();

		public IEnumerable<TaskDAL> GetItems(Expression<Func<TaskDAL, bool>> predicate)
		{
			return db.Tasks.Where(predicate);
		}

	}
}
