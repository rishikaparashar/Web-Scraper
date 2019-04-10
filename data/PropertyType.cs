/*
This class will be used to return property type data from the parser.
This can have various values like:
    -House for rent
    -Conto for rent
    -Studio - 2 Bed, etc.

Each Property will have a property type.
*/

using System.Collections.Generic;
using System;

namespace WebScraperModularized.data{

    public class PropertyType{

        public int id{get; set;}

        public string PROPERTY_TYPE{get; set;}

        public List<Property> properties{get; set;}

        /*
        This method will be used to check if two property types are same.
        */
        public override bool Equals(object obj){
            if (obj == null) return false;
            PropertyType otherPropertytype = obj as PropertyType;
            if (otherPropertytype == null) return false;
            else return this.PROPERTY_TYPE.Equals(otherPropertytype.PROPERTY_TYPE);
        }

    }
}