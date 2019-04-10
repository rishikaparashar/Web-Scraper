/*
This class will be used to return Amenity details from the parser.
This will also be used to save data to the DB.
*/
using System.Collections.Generic;
namespace WebScraperModularized.data{

    public class Amenity{

        public int id {get; set;}

        public string Name {get; set;}

        public string Description {get; set;}

        public int amenity_type {get; set;}

        public List<PropertyAmenityMapping> PropAmenityMapping;
        
    }
}