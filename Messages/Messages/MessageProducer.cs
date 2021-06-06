using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Messages
{
    public static class MessageProducer
    {

        private static ProducerConfig config;
        public static string replyTopic;

        static MessageProducer()
        {
            replyTopic = "reply_topic";
            var conf = new ClientConfig();
            conf.BootstrapServers = "kafka:9092";
            config = new ProducerConfig(conf);
        }

        public static async void SendMessageAsync(string topic, string message)
        {
            using (var prod = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await prod.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    if (dr.Status == PersistenceStatus.Persisted)
                    {
                        Console.WriteLine($"Message has been sent to topic:{topic}\nmessge:{message}");
                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
