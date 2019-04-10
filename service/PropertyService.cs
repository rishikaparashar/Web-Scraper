using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using WebScraperModularized.data;
using WebScraperModularized.helpers;
using WebScraperModularized.parsers;
using WebScraperModularized.wrappers;
using Message = WebScraperModularized.data.Message;

namespace WebScraperModularized.queue
{
    public class PropertyService
    {
        private static readonly HttpClient client = new HttpClient();

        public List<Message> getPropertyUrlsFromPropertyListPage(WebScraperModularized.data.Message propertyPageMessage)
        {

            URL myUrl = new URL();
            myUrl.url = propertyPageMessage.url;
            myUrl.zip_code = int.Parse(propertyPageMessage.zipcode);
            
            var response = client.GetAsync(myUrl.url).Result; //make an HTTP call and get the html for this URL


            string content = response.Content.ReadAsStringAsync().Result; //save HTML into string
            PropertyParser parser = new PropertyParser(content, myUrl);

            //parse the html
            PropertyData propData = parser.parse();

            //insert into DB
            DBHelper.insertParsedProperties(propData);

            Console.WriteLine("Stored {0} properties",
                (propData != null && propData.urlList != null) ? propData.urlList.Count : 0);

            List<Message> listOfPropertyUrl = new List<Message>();
           
            foreach (var each in propData.urlList)
            {
                each.properties.ForEach(eachProperty =>
                {
                    Message message = new Message();
                    message.url = eachProperty.url.url;
                    message.id = eachProperty.id;
                    listOfPropertyUrl.Add(message);
                });
            }

            return listOfPropertyUrl;
        }
    }
}