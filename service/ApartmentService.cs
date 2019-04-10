using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using WebScraperModularized.data;
using WebScraperModularized.helpers;
using WebScraperModularized.parsers;
using WebScraperModularized.wrappers;

namespace WebScraperModularized.queue
{
    public class ApartmentService
    {
        private static readonly HttpClient client = new HttpClient();

        public void processAndSaveApartmentsFromPropertyPage(WebScraperModularized.data.Message propertyMessage)
        {
            URL myUrl = new URL();
            myUrl.url = propertyMessage.url;
            myUrl.property = propertyMessage.id;
            
            var response = client.GetAsync(myUrl.url).Result; //make an HTTP call and get the html for this URL

            string content = response.Content.ReadAsStringAsync().Result; //save HTML into string

            ApartmentParser parser = new ApartmentParser(content, myUrl);

            //call the parse method
            ApartmentData apartmentData = parser.parse();

            DBHelper.insertParsedApartment(apartmentData);

            Console.WriteLine("Stored data for property id {0}!", myUrl.property);
        }
    }
}