/*
This class will contain utility methods.
*/

using System;
using System.Text.RegularExpressions;

namespace WebScraperModularized.helpers{

    public class Util{

        /*
        This method will help us in splitting the rent string found in various places on the website.
        Input format: $850.5 - 1,165.5
        Ouputs double[] with min rent at zeroth position and max rent at first position
        */
        public static double[] splitRentString(string rentString){
            double[] rents = {0,0};
            try{
                if(rentString!=null && rentString.Length!=0){
                    //do some cleanup
                    rentString = rentString.Trim();
                    rentString = Regex.Replace(rentString, "[^0-9.-]", "");

                    //check if the string contains two different values
                    if(rentString.Contains("-")){
                        //min price is the value before -
                        rents[0] = parseDouble(rentString.Split("-")[0].Trim(), 0);
                        //max price is the value after -
                        rents[1] = parseDouble(rentString.Split("-")[1].Trim(), 0);
                    }
                    else {
                        //both min price and maxprice are same;
                        rents[0] = parseDouble(rentString.Trim(), 0);
                        rents[1] = parseDouble(rentString.Trim(), 0);
                    }
                }
            }
            catch(Exception e){
                ExceptionHelper.printException(e);
            }
            return rents;
        }

        /*
        This method will help parsing integers.
        */
        public static double parseDouble(string intString, double def){
            double output;
            if(Double.TryParse(intString, out output)){
                return output;
            }
            else{
                if(intString.Length>0) ExceptionHelper.printException(new Exception("Info(Ignore this): Unable to parse string " + intString));
                return def;
            }
        }

        /*
        Helper method to check if a given url is a valid http or https url
        */
        public static bool isUrlValid(string url){
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static int parseInt(string text, int def){
            int output = 0;
            if(int.TryParse(text, out output)){
                return output;
            }
            else{
                if(text.Length>0) ExceptionHelper.printException(new Exception("Info(Ignore this): Unable to parse string " + text));
            }
            return def;
        }


    }
}