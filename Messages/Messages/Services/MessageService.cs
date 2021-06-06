using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages.Interfaces;
using Messages.Exceptions;
using System.Text.RegularExpressions;
using AuthorizationService.DAL.Models;
using AuthorizationService.DAL;

namespace Messages.Services
{
	public class MessageService : IMessageService
	{
		protected IUnitOfWork uof { get; set; }

		public MessageService(IUnitOfWork uof)
		{
			this.uof = uof;
		}
		public MessageDAL GetItem(int? id)
		{
			if (id == null) throw new ValidationException("Id of message isn't set");

			var message = uof.Messages.GetItem(id.Value);
	
			if (message == null) throw new ValidationException("No message was found");

			if (message.ReplyOnId.HasValue)
			{
				var messageReply = uof.Messages.GetItem(message.ReplyOnId.Value);
				if (messageReply == null) throw new ValidationException("Reply message was not found");
				message.ReplyOn = messageReply;
			}

			return message.CloneWithoutPasswords();
		}

		public IEnumerable<MessageDAL> GetItems()
		{

			var messages = uof.Messages.GetItems(x => true);

			foreach(var mes in messages)
			{
				if (mes.ReplyOnId.HasValue)
				{
					var messageReply = uof.Messages.GetItem(mes.ReplyOnId.Value);
					if (messageReply == null) throw new ValidationException("Reply message was not found");
					mes.ReplyOn = messageReply;
				}
			}

			List<MessageDAL> copy = new List<MessageDAL>();

			foreach(var mes in messages)
			{
				copy.Add(mes.CloneWithoutPasswords());
			}

			return copy;
		}

		public void AddItem(MessageDAL item)
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

			if(item.ReplyOn != null)
			{
				
				if (String.IsNullOrEmpty(item.ReplyOn.Content))
					throw new ValidationException("Wrong or empty", "content reply");
				if (item.ReplyOn.User == null)
					throw new ValidationException("Wrong or empty", "user reply");
				user = uof.Users.GetItems(u => u.EMail == item.ReplyOn.User.EMail).FirstOrDefault();
				if (user == null)
					throw new ValidationException("Specified replying user doesn't exist");
				var message = uof.Messages.GetItems(u => u.User == user).FirstOrDefault();
				if (message == null)
					throw new ValidationException("Reply message doesn't exist");

				item.ReplyOn = message;
				MessageProducer.SendMessageAsync(MessageProducer.replyTopic, item.ReplyOn.User.EMail);
			}

			uof.Messages.Create(item);
			uof.Save();
		}

		public void DeleteItem(MessageDAL item)
		{

			if (String.IsNullOrEmpty(item.Content))
				throw new ValidationException("Wrong or empty property content");

			if (item.User == null)
				throw new ValidationException("Wrong or empty property user");

			if (String.IsNullOrEmpty(item.User.EMail))
				throw new ValidationException("Wrong or empty property email");

			var user = uof.Users.GetItems(u => u.EMail == item.User.EMail).FirstOrDefault();
			if (user == null)
				throw new ValidationException("Specified user doesn't exist");

			var message = uof.Messages.GetItems(u => u.User == user && u.Content == item.Content).FirstOrDefault();

			if (message == null)
				throw new ValidationException("No message was found");

			uof.Tasks.Delete(message.Id);
			uof.Save();
		}

	}
}
