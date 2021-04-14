using System;
using System.Threading;
using System.Net.Mail;
using Confluent.Kafka;
using System.Net;

namespace EmailService
{
	class Program
	{
		static void Main(string[] args)
		{
			MessageConsumer.ProcessMessageAsync();
		}
	}

}
