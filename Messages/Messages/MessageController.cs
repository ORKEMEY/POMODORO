using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Messages.Services;
using Messages.Exceptions;
using Messages.Interfaces;
using AuthorizationService.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages
{
	[Route("api/message")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private IMessageService service { get; set; }

		public MessageController(IMessageService service)
		{
			this.service = service;
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try
			{
				var message = service.GetItem(id);
				if (message == null) return new NotFoundResult();

				return new JsonResult(message);
					
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
		public IActionResult GetAll()
		{
			try
			{
				var messages = service.GetItems();
				if (messages == null) return new NotFoundResult();

				return new JsonResult(messages);
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
				service.AddItem(new MessageDAL()
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

		[HttpPost("{reply}")]
		public IActionResult Reply([FromQuery] string email, string content, string emailReply, string contentReply)
		{
			try
			{
				service.AddItem(new MessageDAL()
				{
					User = new UserDAL { EMail = email },
					ReplyOn = new MessageDAL 
					{ 
						User = new UserDAL { EMail = emailReply },
						Content = contentReply
					},
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
				service.DeleteItem(new MessageDAL()
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
