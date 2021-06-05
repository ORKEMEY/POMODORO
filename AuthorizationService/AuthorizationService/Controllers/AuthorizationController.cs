using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.DAL.Models;
using AuthorizationService.Interfaces;
using AuthorizationService.Services;
using Prometheus;

namespace AuthorizationService.Controllers
{
	[ApiController]
	[Route("api/authorization")]
	public class AuthorizationController : ControllerBase
	{
		private IUserService service { get; set; }
		public static List<Client> Clients;

		public Counter PostCounter { get; set; }
		

		static AuthorizationController()
		{
			Clients = new List<Client>();
		}

		public AuthorizationController(IUserService service)
		{
			this.service = service;
			this.PostCounter = Metrics.CreateCounter("post_counter", "Registrations counter");
			Console.WriteLine($"Counter value: {PostCounter.Value}");
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try
			{
				var user = service.GetItem(id);
				if (user == null) return new NotFoundResult();
				return new JsonResult(user);
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += " " + e.Property;
				return new BadRequestObjectResult(new
				{
					errorText = error
				});
			}
		}
		
		[HttpGet]
		public IActionResult Authentificate([FromQuery] string email, string password)
		{
			try
			{
				var user = service.Authentificate(email, password);
				var res = new JsonResult(new { login = user.Login, email = user.EMail });
				res.StatusCode = 200;
				return res;
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += " " + e.Property;
				return new BadRequestObjectResult(new
				{
					errorText = error
				});
			}
			catch (NotFoundException e){
				return new NotFoundObjectResult(new
					{
						errorText = e.Message
					});
			}
			
		}

		[Route("test")]
		[HttpGet]
		public IActionResult GetTest()
		{
			Console.WriteLine("test");
			return new OkResult();
		}

		[HttpPost]
		public IActionResult Post([FromQuery] string login, string email, string password)
		{
			try
			{
				Console.WriteLine($"user:{login} {email} {password}");
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
				if (e.Property != null) error += " " + e.Property;
				return new BadRequestObjectResult(new { errorText = error });
			}
			PostCounter.Inc();
			
			var res = new JsonResult(new { login = login, email = email });
			res.StatusCode = 200;
			return res;
		}

		[HttpDelete]
		public IActionResult DeleteAccount([FromQuery] string email, string password)
		{
			try
			{
				var user = service.Authentificate(email, password);
				if (user == null) return new NotFoundResult();
				service.DeleteItem(user);
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += " " + e.Property;
				return new BadRequestObjectResult(new
				{
					errorText = error
				});
			}
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var userDTO = service.GetItem(id);
				service.DeleteItem(userDTO);
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += " " + e.Property;
				return new NotFoundObjectResult(new { errorText = error });
			}

			return Ok();
		}

	}
}
