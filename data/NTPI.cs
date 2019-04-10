/*
This class will be used to return nearby transit and Points of interest data from the parser.
This will also be used to save data to the DB.
*/
using System.Collections.Generic;
namespace WebScraperModularized.data{

    public class NTPI{

        public int id {get; set;}

        public string name{get; set;}

        public double Drive {get; set;}//drive time in minutes

        public double distance {get; set;}//distance in miles

        public int NTPC {get; set;}

        public List<PropertyNTPIMapping> PropNTPIMapping;

    }
}