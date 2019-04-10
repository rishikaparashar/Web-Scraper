/*
This class will be used as a wrapper to return data from the property parser.
*/
using WebScraperModularized.data;
using System.Collections.Generic;

namespace WebScraperModularized.wrappers{

    public class PropertyData{

        public List<PropertyType> urlList;

        public void setUrlList(List<PropertyType> urlList){
            this.urlList = urlList;
        }

        public List<PropertyType> getUrlList(){
            return this.urlList;
        }


    }
}