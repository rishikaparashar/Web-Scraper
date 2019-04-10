/*
This class will be used to return expense type data from parser.
This will also be used to save data to DB.
*/
using System.Collections.Generic;

namespace WebScraperModularized.data{

    public class Expensetype{

        public int id {get; set;}

        public string Name {get; set;}

        public List<Expenses> expensesList;
    }
}