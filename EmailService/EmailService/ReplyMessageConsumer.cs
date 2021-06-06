using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace EmailService
{
	public static class ReplyMessageConsumer
	{
        private static ConsumerConfig config;
        public static string replyTopic;

        static ReplyMessageConsumer()
        {

            replyTopic = "reply_topic";
            var conf = new ClientConfig();
            conf.BootstrapServers = "kafka:9092";

            config = new ConsumerConfig(conf);
            config.GroupId = "reply_service";
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
        }


        public static async void ProcessMessageAsync()
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(replyTopic);
                ConsumeResult<Ignore, string> consumeResult = null;
                try
                {
                    while (true)
                    {
                        try
                        {
                            consumeResult = consumer.Consume(cancellationTokenSource.Token);
                            EmailSender.SendReplyNotification(consumeResult.Value);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred (replying message): {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    if (consumeResult != null) consumer.Commit(consumeResult);
                    consumer.Close();
                }

            }
        }
    }
}
