using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace EmailService
{

    public static class MessageConsumer
    {

        private static ConsumerConfig config;
        public static string emailTopic;

        static MessageConsumer()
        {

            emailTopic = "email_topic";
            var conf = new ClientConfig();
            conf.BootstrapServers = "kafka:9092";

            config = new ConsumerConfig(conf);
            config.GroupId = "email_service";
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
                consumer.Subscribe(emailTopic);
                ConsumeResult<Ignore, string> consumeResult = null;
                try
                {
                    while (true)
                    {
                        try
                        {
                            consumeResult = consumer.Consume(cancellationTokenSource.Token);
                            EmailSender.SendEmail(consumeResult.Value);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
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
