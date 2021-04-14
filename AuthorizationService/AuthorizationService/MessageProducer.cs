using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace AuthorizationService
{
    public static class MessageProducer
    {

        private static ProducerConfig config;
        public static string emailTopic;

        static MessageProducer(){

            emailTopic = "email_topic";
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
                   
                }
                catch (ProduceException<Null, string> e)
                {
                   
                }

            }
        }
    }
}
