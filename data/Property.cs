/*
This class will be used to return property data from the parser.
*/
namespace WebScraperModularized.data{
    
    public class Property{
        public int id{get; set;}
        
        public string name{get; set;}

        public string zip{get; set;}

        public string City {get; set;}

        public string State {get; set;}

        public string Country {get; set;}

        public string Description {get; set;}

        public string Price_range {get; set;}

        public double minprice{get; set;}

        public double maxprice{get; set;}

        public string Phone{get; set;}

        public string Email{get; set;}

        public string address{get; set;}

        public int PROPERTY_TYPE{get; set;}

        public int SOUND_SCORE{get; set;}

        public URL url {get; set;}

        public string SOUNDSCORE_TEXT{get; set;}

        public bool reinforcement{get; set;}
    }
}