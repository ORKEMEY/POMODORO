using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
	[ApiController]
	[Route("api/authorization")]
	public class AuthorizationController : ControllerBase
	{

		public static List<Client> Clients;

		static AuthorizationController()
		{
			Clients = new List<Client>();
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string login, string password)
		{

			var client = Clients.Find(c => c.Login == login && c.Password == password);
			if(client != default)
			{
				return new JsonResult( new { client.Id, client.Login });
			}
			else
			{
				return new NotFoundObjectResult("Account not found");
			}
				
		}

		[HttpPost]
		public IActionResult Post([FromQuery] string login, string password)
		{
			try
			{
				if (Clients.Find(c => c.Login == login) != default)
					throw new Exception("This Login is already taken");
				Clients.Add(new Client()
				{
					Id = Clients.Count,
					Login = login,
					Password = password
				});
			}
			catch (Exception e)
			{
				return new BadRequestObjectResult(new { errorText = e.Message });
			}

			return new OkResult();
		}



	}
}
