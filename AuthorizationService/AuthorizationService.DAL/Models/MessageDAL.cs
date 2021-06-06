using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationService.DAL.Models
{
	public class MessageDAL
	{
		public int Id { get; set; }
		public UserDAL User { get; set; }
		public int? ReplyOnId { get; set; }
		public MessageDAL ReplyOn { get; set; }
		public string Content { get; set; }

		public MessageDAL CloneWithoutPasswords()
		{
			var message = new MessageDAL()
			{
				Id = this.Id,
				ReplyOnId = this.ReplyOnId,
				Content = this.Content,
				ReplyOn = ReplyOn == null ? null : new MessageDAL()
				{
					Id = this.ReplyOn.Id,
					Content = this.ReplyOn.Content,
					User = this.ReplyOn.User == null ? null : new UserDAL()
					{
						Id = this.ReplyOn.User.Id,
						EMail = this.ReplyOn.User.EMail,
						Login = this.ReplyOn.User.Login,
					}

				},
				User = this.User == null ? null : new UserDAL()
				{
					Id = this.User.Id,
					EMail = this.User.EMail,
					Login = this.User.Login,
				}

			};

			return message;
		}

	}
}
