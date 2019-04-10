/*
This class will be used to return amenity type data from the parser.
This will also be used to save Amenitytype data to the db.
*/

using System.Collections.Generic;

namespace WebScraperModularized.data{

    public class Amenitytype{
        
        public int id {get; set;}

        public string Name {get; set;}

        public List<Amenity> amenityList;
    }
}