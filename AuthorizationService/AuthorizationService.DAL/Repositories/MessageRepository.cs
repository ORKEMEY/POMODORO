using System;
using System.Collections.Generic;
using AuthorizationService.DAL.Models;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.DAL.Repositories
{
	class MessageRepository : IRepository<MessageDAL>
	{
		protected AuthorizationContext db { get; set; }

		public MessageRepository(AuthorizationContext context)
		{
			db = context;
		}

		public MessageDAL GetItem(int id)
		{
			return db.Messages.Include(x => x.User).Include(x => x.ReplyOn)
				.FirstOrDefault(x => x.Id == id);
		}

		public void Create(MessageDAL item)
		{
			db.Messages.Add(item);
		}

		public void Update(MessageDAL item)
		{
			db.Entry(item).State = EntityState.Modified;
		}

		public void Delete(int id)
		{
			MessageDAL item = db.Messages.Find(id);
			if (item != null)
				db.Messages.Remove(item);
		}

		public void Save() => db.SaveChanges();

		public IEnumerable<MessageDAL> GetItems(Expression<Func<MessageDAL, bool>> predicate)
		{
			return db.Messages.Include(x => x.User).Where(predicate);
		}

	}
}
