using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL.Models;
using AuthorizationService.Interfaces;
using AuthorizationService.Services;

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

		private IUserService service { get; set; }

		public AuthorizationController(IUserService service)
		{
			this.service = service;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] int id)
		{
			try
			{
				var user = service.GetItem(id);
				if (user == null) return new NotFoundResult();
				return new JsonResult(user);
			}
			catch (ValidationException e)
			{
				return new BadRequestObjectResult(new
				{
					errorText = e.Message
				});
			}
		}

		[Route("test")]
		[HttpGet]
		public IActionResult GeTest()
		{
			Console.WriteLine("test");
			return new OkResult();
		}

		[HttpPost]
		public IActionResult Post([FromQuery] string login, string email, string password)
		{
			try
			{
				service.AddItem(new UserDAL()
				{
					Login = login,
					Password = password,
					EMail = email
				});
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += ": " + e.Property;

				return new BadRequestObjectResult(new { errorText = error });
			}

			return new OkResult();
		}

		[HttpDelete]
		public IActionResult Delete([FromQuery]  int id)
		{
			try
			{
				var userDTO = service.GetItem(id);
				service.DeleteItem(userDTO);
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += ": " + e.Property;
				return new NotFoundObjectResult(new { errorText = error });
			}

			return Ok();
		}

	}
}
