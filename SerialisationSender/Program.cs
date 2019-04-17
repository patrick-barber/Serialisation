using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedObjects;
using System;
using System.Text;

namespace SerialisationSender
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonService commonService = new CommonService();
            IConnection connection = commonService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            //SetupSerialisationMessageQueue(model);
            RunSerialisationDemo(model);
        }
        private static void SetupSerialisationMessageQueue(IModel model)
        {
            model.QueueDeclare(CommonService.SerialisationQueueName, true, false, false, null);
        }

        private static void RunSerialisationDemo(IModel model)
        {
            Console.WriteLine("Enter a name. Quit with 'q'.");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "q") break;
                Name name = new Name() { name = input };
                IBasicProperties basicProperties = model.CreateBasicProperties();
                basicProperties.Persistent = true;
                String jsonified = JsonConvert.SerializeObject(name);
                byte[] customerBuffer = Encoding.UTF8.GetBytes(jsonified);
                model.BasicPublish("", CommonService.SerialisationQueueName, basicProperties, customerBuffer);
            }
        }
    }
}
