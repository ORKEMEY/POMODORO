using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
	[ApiController]
	[Route("api/authorization")]
	public class AuthorizationController : ControllerBase
	{

		public static List<Client> Clients;

		private static bool IsBrokenController { get; set; }

		static AuthorizationController()
		{
			Clients = new List<Client>();
			IsBrokenController = false;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string login, string password)
		{

			var client = Clients.Find(c => c.Login == login && c.Password == password);
			if (IsBrokenController) Thread.Sleep(5000);
			if (client != default)
			{
				return new JsonResult( new { client.Id, client.Login });
			}
			else
			{
				return new NotFoundObjectResult("Account not found");
			}
				
		}
		[Route("test")]
		[HttpGet]
		public IActionResult GeTest()
		{
			if (IsBrokenController) Thread.Sleep(5000);
			return new OkResult();
		}

		[Route("break")]
		[HttpGet]
		public IActionResult GetBreak()
		{
			if(IsBrokenController)
			return new OkObjectResult("Pod is already broken");

			IsBrokenController = true;
			return new OkObjectResult("Pod has been broken");
		}

		[Route("fix")]
		[HttpGet]
		public IActionResult GetRepair()
		{
			if(!IsBrokenController) 
			return new OkObjectResult("Pod is serviceable");

			IsBrokenController = false;
			return new OkObjectResult("Pod has been repaired");
		}

		[HttpPost]
		public IActionResult Post([FromQuery] string login, string password)
		{
			if (IsBrokenController) Thread.Sleep(5000);
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
