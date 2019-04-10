using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Z.Dapper.Plus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;
using WebScraperModularized.data;
using WebScraperModularized.helpers;
using WebScraperModularized.parsers;
using WebScraperModularized.queue;
using WebScraperModularized.wrappers;
using Message = WebScraperModularized.data.Message;

namespace WebScraperModularized
{
    class Program
    {
        static void Main(string[] args)
        {
            doDapperMapping();        
            IQueueService queueService = AzureServiceBusService.Instance;
            List<string> zipcodes = File.ReadAllLines("zipcode.dat").ToList();
            List<Message> messages = new List<Message>();
            foreach (var zipcode in zipcodes)
            {
                Message message = new Message();
                message.zipcode = zipcode;
                Console.WriteLine("Going to send zipcode "+zipcode + " to zipcode queue");
                messages.Add(message);
            }      
            queueService.sendListOfMessagesToQueue(messages,"zipcodequeue");
            String input = "process";
            while (!input.SequenceEqual("exit"))
            {
                Console.WriteLine("Type exit to exit processing");
                input = Console.ReadLine();
            }
        }

        private static void doDapperMapping()
        {
            DapperPlusManager.Entity<URL>().Table("url").Identity(x => x.id);
            DapperPlusManager.Entity<Property>().Table("PROPERTY").Identity(x => x.id);
            DapperPlusManager.Entity<PropertyType>().Table("PROPERTY_TYPE").Identity(x => x.id);
            DapperPlusManager.Entity<School>().Table("School").Identity(x => x.id);
            DapperPlusManager.Entity<Review>().Table("review").Identity(x => x.id);
            DapperPlusManager.Entity<NTPI>().Table("NearestTransitPointInterest").Identity(x => x.id);
            DapperPlusManager.Entity<Expenses>().Table("Expenses").Identity(x => x.id);
            DapperPlusManager.Entity<Expensetype>().Table("Expense_Type").Identity(x => x.id);
            DapperPlusManager.Entity<Apartments>().Table("APARTMENT").Identity(x => x.id);
            DapperPlusManager.Entity<Amenity>().Table("AMENITY").Identity(x => x.id);
            DapperPlusManager.Entity<Amenitytype>().Table("AMENITY_TYPE").Identity(x => x.id);
            DapperPlusManager.Entity<PropertyAmenityMapping>().Table("PROPERTY_AMENITY_MAP").Identity(x => x.Id);
            DapperPlusManager.Entity<PropertySchoolMapping>().Table("Property_school").Identity(x => x.Id);
            DapperPlusManager.Entity<NTPICategory>().Table("NearestTransitPoint_Category").Identity(x => x.Id);
            DapperPlusManager.Entity<PropertyNTPIMapping>().Table("NTPI_Property").Identity(x => x.Id);
        }
    }
}