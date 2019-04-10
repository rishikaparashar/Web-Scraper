/*
This class will be used to return data for expenses from the parser.
This will also be used to save data into DB.
*/

namespace WebScraperModularized.data{
    
    public class Expenses{

        public int id {get; set;}

        public string Name {get; set;}

        public int expensetype {get; set;}

        public string Cost {get; set;}

        public double mincost {get; set;}

        public double maxcost {get; set;}

        public int property {get; set;}

        public int Expense_Type {get; set;}

    }
}