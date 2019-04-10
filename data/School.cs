/*
This class will be used to return nearby schools data from the parser.
This will also be used to save data to the DB.
*/
using System.Collections.Generic;
namespace WebScraperModularized.data{
    
    public class School{


        public enum SchoolType : int{
            PUBLIC = 0,
            PRIVATE = 1
        }
        public int id {get; set;}

        public string name {get; set;}

        public string Type_text {get; set;}

        public string grades {get; set;}

        public int No_of_students {get; set;}

        public string Contact_number {get; set;}

        public int rating {get; set;}//out of 10

        public int property {get; set;}

        public int schooltype;

        public List<PropertySchoolMapping> PropSchoolMapping {get; set;}
        
    }
}