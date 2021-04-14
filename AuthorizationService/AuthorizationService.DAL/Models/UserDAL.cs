using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationService.DAL.Models
{
	public class UserDAL
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public string EMail { get; set; }
	}

}
