using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
	class Program
	{
		static void Main(string[] args)
		{
			string ip = GetIp();

			Test(ip);
			Console.ReadKey();
		}

		public static async Task Test(string ip)
		{
			HttpClient client = new HttpClient();
			int errCount = 0, numberOfRequests = 10;
			DateTime now;
			TimeSpan sumTime = TimeSpan.Zero;

			for (int count = 0; count < numberOfRequests; count++)
			{
				now = DateTime.Now;
				try
				{

					HttpResponseMessage response = await client.GetAsync($"http://{ip}/api/authorization/test");

					if (!response.IsSuccessStatusCode)
					{
						errCount++;
						Console.WriteLine($"Failed to load response №{count + 1} Status code: {response.StatusCode.ToString()}");
					}
				}
				catch (HttpRequestException e)
				{
					errCount++;
					Console.WriteLine($"Failed to load response №{count + 1} Message:{e.Message}");
				}
				sumTime += DateTime.Now - now;
			}

			OutputResult(sumTime, numberOfRequests, errCount);
		}


		public static string GetIp()
		{
			var proc = new Process();
			proc.StartInfo = new ProcessStartInfo("cmd", "/c minikube ip");
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();
			proc.WaitForExit();
			return proc.StandardOutput.ReadToEnd().Trim();
		}

		public static void OutputResult(TimeSpan sumTime, int numberOfRequests, int errRequests)
		{

			Console.WriteLine($"Average duration of request is :{(sumTime/ numberOfRequests).TotalSeconds} s. \nNumber of failed requests: {errRequests}");

		}


	}
}
