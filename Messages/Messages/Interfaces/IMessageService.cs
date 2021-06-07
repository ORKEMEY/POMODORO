using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL.Models;

namespace Messages.Interfaces
{
	public interface IMessageService
	{
		MessageDAL GetItem(int? id);
		public IEnumerable<MessageDAL> GetItems();
		void AddItem(MessageDAL item);
		void DeleteItem(MessageDAL item);
		void DeleteItem(int id);
	}
}
