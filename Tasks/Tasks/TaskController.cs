using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tasks.Services;
using Tasks.Exceptions;
using Tasks.Interfaces;
using AuthorizationService.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks
{
	[Route("api/task")]
	[ApiController]
	public class TaskController : ControllerBase
	{

		private ITaskService service { get; set; }

		public TaskController(ITaskService service)
		{
			this.service = service;
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try
			{
				var task = service.GetItem(id);
				if (task == null) return new NotFoundResult();
				return new JsonResult(task);
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
		public IActionResult GetByUser([FromQuery] string email)
		{
			try
			{
				var task = service.GetItems(new UserDAL() { EMail = email });
				if (task == null) return new NotFoundResult();
				return new JsonResult(task);
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

		[HttpPost]
		public IActionResult PostByEmail([FromQuery] string email, string content)
		{
			try
			{
				service.AddItem(new TaskDAL()
				{
					User = new UserDAL { EMail = email },
					Content = content
				});
			}
			catch (ValidationException e)
			{
				string error = e.Message;
				if (e.Property != null) error += " " + e.Property;
				return new BadRequestObjectResult(new { errorText = error });
			}

			return new OkResult();
		}

		[HttpDelete]
		public IActionResult Delete([FromQuery] string email, string content)
		{
			try
			{
				//var userDTO = service.GetItem(id);
				service.DeleteItem(new TaskDAL()
				{
					User = new UserDAL { EMail = email },
					Content = content
				});
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
