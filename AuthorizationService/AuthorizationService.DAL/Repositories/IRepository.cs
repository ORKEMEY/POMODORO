using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace AuthorizationService.DAL.Repositories
{
	public interface IRepository<T> where T : class
	{
		T GetItem(int id);
		void Create(T item);
		void Update(T item);
		void Delete(int id);
		void Save();
		public IEnumerable<T> GetItems(Expression<Func<T, bool>> predicate);
	}
}
