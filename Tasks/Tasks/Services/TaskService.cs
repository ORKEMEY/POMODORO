using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.Interfaces;
using System.Text.RegularExpressions;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL;

namespace Tasks.Services
{
	public class TaskService : ITaskService
	{
		protected IUnitOfWork uof { get; set; }

		public TaskService(IUnitOfWork uof)
		{
			this.uof = uof;
		}
		public TaskDAL GetItem(int? id)
		{
			if (id == null) throw new ValidationException("Id of task isn't set");
			var user = uof.Tasks.GetItem(id.Value);

			if (user == null) throw new ValidationException("No task was found");
			return user;
		}

		public IEnumerable<TaskDAL> GetItems(UserDAL user)
		{
			if (user == null) throw new ValidationException("User isn't specified");
			if (String.IsNullOrEmpty(user.EMail)) throw new ValidationException("Email isn't specified");

			var curUser = uof.Users.GetItems(x => x.EMail == user.EMail).FirstOrDefault();
			if (curUser == null) throw new ValidationException("Email isn't specified");

			var task = uof.Tasks.GetItems(x => x.User.Id == curUser.Id);

			if (task == null) throw new ValidationException("No task was found");
			return task;
		}

		public void AddItem(TaskDAL item)
		{
			if (String.IsNullOrEmpty(item.Content))
				throw new ValidationException("Wrong or empty", "content");
			if (item.User == null)
				throw new ValidationException("Wrong or empty", "user");
			UserDAL user = null;

			if (!String.IsNullOrEmpty(item.User.EMail))
				user = uof.Users.GetItems(u => u.EMail == item.User.EMail).FirstOrDefault();
			else user = uof.Users.GetItems(u => u.Id == item.User.Id).FirstOrDefault();

			if (user == null)
				throw new ValidationException("Specified user doesn't exist");
			item.User = user;
			uof.Tasks.Create(item);
			uof.Save();
		}

		public void DeleteItem(TaskDAL item)
		{
			if (item.Id <= 0)
				throw new ValidationException("Wrong or empty property Id");

			uof.Tasks.Delete(item.Id);
			uof.Save();
		}

	}
}
