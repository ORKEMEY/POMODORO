using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL.Models;

namespace Tasks.Interfaces
{
	public interface ITaskService
	{
		TaskDAL GetItem(int? id);
		public IEnumerable<TaskDAL> GetItems(UserDAL user);
		void AddItem(TaskDAL item);
		void DeleteItem(TaskDAL item);
	}
}
