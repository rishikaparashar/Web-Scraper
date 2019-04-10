using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using WebScraperModularized.helpers;
using WebScraperModularized.parsers;
using WebScraperModularized.wrappers;

namespace WebScraperModularized.queue
{
    public class AzureServiceBusService : IQueueService
    {
        private static AzureServiceBusService instance = null;
        private static readonly object padlock = new object();


        private readonly IQueueClient zipcodeQueueClient;
        private readonly IQueueClient propertyQueueClient;
        private readonly IQueueClient apartmentQueueClient;


        private readonly ApartmentService _apartmentService;
        private readonly PropertyService _propertyService;
        private readonly ZipcodeService _zipcodeService;


        private readonly Hashtable mapOfQueues;

        public static AzureServiceBusService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new AzureServiceBusService();
                        }
                    }
                }

                return instance;
            }
        }

        private AzureServiceBusService()
        {
            string ServiceBusConnectionString = new MyConfigurationHelper().getServiceBusConnectionString();
            string zipcodequeue = "zipcodequeue";
            string propertyqueue = "propertyqueue";
            string apartmentqueue = "apartmentqueue";
            zipcodeQueueClient = new QueueClient(ServiceBusConnectionString, zipcodequeue);
            propertyQueueClient = new QueueClient(ServiceBusConnectionString, propertyqueue);
            apartmentQueueClient = new QueueClient(ServiceBusConnectionString, apartmentqueue);
            mapOfQueues = new Hashtable();
            mapOfQueues.Add(zipcodequeue, zipcodeQueueClient);
            mapOfQueues.Add(propertyqueue, propertyQueueClient);
            mapOfQueues.Add(apartmentqueue, apartmentQueueClient);
            _zipcodeService = new ZipcodeService();
            _propertyService = new PropertyService();
            _apartmentService = new ApartmentService();
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        public async Task sendMessageToQueue(WebScraperModularized.data.Message message, String queueName)
        {
            var azureMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            await SendMessageAsync((IQueueClient) mapOfQueues[queueName], azureMessage);
        }

        public async Task sendListOfMessagesToQueue(List<WebScraperModularized.data.Message> messages, String queueName)
        {
            List<Message> listOfMessages = new List<Message>();
            foreach (var each in messages)
            {
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(each)));
                listOfMessages.Add(message);
            }

            await SendListOfMessageAsync((IQueueClient) mapOfQueues[queueName], listOfMessages);
        }

        private static async Task SendMessageAsync(IQueueClient queueClient, Message message)
        {
            try
            {
                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        private static async Task SendListOfMessageAsync(IQueueClient queueClient, List<Message> listOfMessages)
        {
            try
            {
                await queueClient.SendAsync(listOfMessages);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 10,

                // False below indicates the Complete will be handled by the User Callback manually.
                AutoComplete = false
            };

            // Register the function that will process messages
            zipcodeQueueClient.RegisterMessageHandler(ProcessZipcodeMessagesAsync,
                messageHandlerOptions);
            propertyQueueClient.RegisterMessageHandler(ProcessPropertyMessagesAsync,
                messageHandlerOptions);
            apartmentQueueClient.RegisterMessageHandler(ProcessApartmentMessagesAsync,
                messageHandlerOptions);
        }

        private async Task ProcessZipcodeMessagesAsync(Message message, CancellationToken token)
        {
            String messageContent = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Received message " + messageContent);

            WebScraperModularized.data.Message zipcodeMessage =
                JsonConvert.DeserializeObject<WebScraperModularized.data.Message>(
                    messageContent);

            List<WebScraperModularized.data.Message> listOfUrlsToBeParsedForTheGivenPostcode =
                _zipcodeService.ProcessZipcodeMessagesAsync(zipcodeMessage.zipcode);
            await sendListOfMessagesToQueue(listOfUrlsToBeParsedForTheGivenPostcode, "propertyqueue");
            await zipcodeQueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }


        private async Task ProcessPropertyMessagesAsync(Message message, CancellationToken token)
        {
            String messageContent = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Received message " + messageContent);
            WebScraperModularized.data.Message propertyMessage =
                JsonConvert.DeserializeObject<WebScraperModularized.data.Message>(
                    messageContent);

            List<WebScraperModularized.data.Message> listOfPropertyUrl =
                _propertyService.getPropertyUrlsFromPropertyListPage(propertyMessage);

            await sendListOfMessagesToQueue(listOfPropertyUrl, "apartmentqueue");

            await propertyQueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private async Task ProcessApartmentMessagesAsync(Message message, CancellationToken token)
        {
            String messageContent = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Received message " + messageContent);

            WebScraperModularized.data.Message apartmentMessage =
                JsonConvert.DeserializeObject<WebScraperModularized.data.Message>(
                    messageContent);

            _apartmentService.processAndSaveApartmentsFromPropertyPage(apartmentMessage);

            await apartmentQueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}