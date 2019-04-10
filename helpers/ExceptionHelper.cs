/*
This class will be used to help print exceptions
*/
using System;

namespace WebScraperModularized.helpers{
    public class ExceptionHelper{
        public static void printException(Exception e){
            Console.WriteLine("Exception while parsing : {0}", e.ToString());
        }
    }
}