using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
	public class Client
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
	}
}
