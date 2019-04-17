using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedObjects;

namespace SerialisationReceiver
{
    public class MessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        public MessageReceiver(IModel channel)
        {
            _channel = channel;
        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            String jsonified = Encoding.UTF8.GetString(body);
            Name name = JsonConvert.DeserializeObject<Name>(jsonified);
            Console.WriteLine("Pure json: {0}", jsonified);
            Console.WriteLine("Name: {0}", name.name);
            _channel.BasicAck(deliveryTag, false);
        }
    }
}
