using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationService.DAL.Models
{
	public class TaskDAL
	{
		public int Id { get; set; }
		public UserDAL User { get; set; }
		public string Content { get; set; }
	}
}
