using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AuthorizationService.Interfaces;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL;

namespace AuthorizationService.Services
{
	public class UserService : IUserService
	{

		protected IUnitOfWork uof { get; set; }
		private string patternEMail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
				@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

		public UserService(IUnitOfWork uof)
		{
			this.uof = uof;
		}
		public UserDAL GetItem(int? id)
		{
			if (id == null) throw new ValidationException("Id of user isn't set", "id");
			var user = uof.Users.GetItem(id.Value);

			if (user == null) throw new ValidationException("No user was found");
			return user;
		}

		public void AddItem(UserDAL item)
		{
			if (String.IsNullOrEmpty(item.Login))
				throw new ValidationException("Wrong or empty properties", "Login");
			if (String.IsNullOrEmpty(item.Password))
				throw new ValidationException("Wrong or empty properties", "Password");

			bool isEMail = Regex.IsMatch(item.EMail, patternEMail, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if (String.IsNullOrEmpty(item.EMail) | !isEMail)
				throw new ValidationException("Wrong or empty properties", "EMail");
			
			var user = uof.Users.GetItems(u => u.Login == item.Login).FirstOrDefault();
			if (user != null)
				throw new ValidationException("This Login is already taken", "Login");

			uof.Users.Create(item);
			uof.Save();
			MessageProducer.SendMessageAsync(MessageProducer.emailTopic, item.EMail);
		}

		public void DeleteItem(UserDAL item)
		{
			if (item.Id <= 0)
				throw new ValidationException("Wrong or empty property Id");
			uof.Users.Delete(item.Id);
			uof.Save();
		}

		public UserDAL Authentificate(string login, string password)
		{
			UserDAL item = uof.Users.GetItems(u => u.Login == login & u.Password == password).FirstOrDefault();
			return item;
		}

	}
}
