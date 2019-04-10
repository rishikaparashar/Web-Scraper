using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraperModularized.data;

namespace WebScraperModularized.queue
{
    public interface IQueueService
    {
        Task sendMessageToQueue(Message message, String queueName);
        Task sendListOfMessagesToQueue(List<Message> messages, String queueName);
    }
}