/*
This class will be used to return Apartments data from the parser.
It will also be used to insert values into DB.
*/
namespace WebScraperModularized.data{

    public class Apartments{

        public int id {get; set;}

        public double NO_OF_BEDS {get; set;}

        public double NO_OF_BATHS {get; set;}
        
        public double MIN_PRICE {get; set;}
        
        public double MAX_PRICE {get; set;}

        public double area {get; set;}

        public int property {get; set;}

        public string availability {get; set;}

        public string Unit {get; set;}

        public string Name {get; set;}
    }
}