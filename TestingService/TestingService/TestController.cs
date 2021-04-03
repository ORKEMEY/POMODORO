using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TestingService
{
	[Route("api/test")]
	[ApiController]
	public class TestController : ControllerBase
	{


		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var response = RequestAsync().Result;

				if (response.IsSuccessStatusCode)
				{
					return new OkObjectResult(response.Content.ReadAsStringAsync().Result);
				}
				else return new BadRequestObjectResult(response.Content.ReadAsStringAsync().Result);
			}
			catch (HttpRequestException e)
			{
				return new NotFoundObjectResult(e.Message);
			}

		}


		private async Task<HttpResponseMessage> RequestAsync()
		{
			HttpClient client = new HttpClient();
			return await client.GetAsync($"http://authorization-service/api/authorization/test");
		}


	}
}
