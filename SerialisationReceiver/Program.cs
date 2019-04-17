using RabbitMQ.Client;
using System;

namespace SerialisationReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonService commonService = new CommonService();
            IConnection connection = commonService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            ReceiveSerialisationMessages(model);
        }

        private static void ReceiveSerialisationMessages(IModel model)
        {
            model.BasicQos(0, 1, false);
            MessageReceiver consumer = new MessageReceiver(model);
            //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(CommonService.SerialisationQueueName, false, consumer);
            Console.ReadLine();

           //while (true)
           //{
           //    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
           //    String jsonified = Encoding.UTF8.GetString(deliveryArguments.Body);
           //    Customer customer = JsonConvert.DeserializeObject<Customer>(jsonified);
           //    Console.WriteLine("Pure json: {0}", jsonified);
           //    Console.WriteLine("Customer name: {0}", customer.Name);
           //    model.BasicAck(deliveryArguments.DeliveryTag, false);
           // }
        }
    }
}
