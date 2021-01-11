using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class GlobalVariables
    {
        private static Random random = new Random();

        public static string URL
        {
            get
            {
                //return "https://www.rotatepay.com";
                return "http://localhost:3000";
            }
        }   

        public static string ImagePath
        {
            get
            {
                //return @"C:\inetpub\wwwroot\RotatePay\Frontend\";
                return  @"C:\Users\new\Desktop\Files - New\HAROFT TECHNOLOGIES\Products\RotatePay\Frontend\public";
            }
        }         

        public static string AdminEmail
        {
            get
            {
                return "email@rotatepay.com";
                //return "info@haroftech.com";
            }
        }  

        public static string DocumentEmail
        {
            get
            {
                return "email@rotatepay.com";
                //return "document@rotatepay.com";
                //return "info@haroftech.com";
            }
        }  

        public static string MyKey
        {
            get
            {
                return "Wp1R7YbdCvFg9Ob4A";
            }
        }

        public static int RandomStringLength()
        {
            return 31;
        }  
        
        public static int RandomStringLengthShort()
        {
            return 15;
        }                   

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }        

        public static string BaseKey()
        {
            return "tEAyznj3kucbinvrQcIhxA8WLPiHgHb";
        }            
    }
}
