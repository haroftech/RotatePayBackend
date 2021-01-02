using System;
using Backend.Helpers;

namespace Backend.Entities
{
    public class UserUpload
    {
        public int Id { get; set; }       
        public int UserId { get; set; }   
        public string ProfilePicture { get; set; }  
        public string BankStatement { get; set; }  
        public string OfficialIDCard { get; set; } 
        public string WorkIDCard { get; set; }  // Not used for now but do not delete           
        public string UtilityBill { get; set; }  // Not used for now but do not delete  
        public string ImageNames { get; set; }  
        public DateTime DateAdded { get; set; } 
        public string DateAddedDisplay 
        {
            get
            {
                return RelativeTime.getRelativeTime(DateAdded);
            }            
        }                       
        public string MyProperty0 { get; set; }
        public string MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
        public string MyProperty3 { get; set; }
        public string MyProperty4 { get; set; }
        public string MyProperty5 { get; set; }
        public string MyProperty6 { get; set; }
        public string MyProperty7 { get; set; }
        public string MyProperty8 { get; set; }
        public string MyProperty9 { get; set; }

        public User User { get; set; }
    }
}