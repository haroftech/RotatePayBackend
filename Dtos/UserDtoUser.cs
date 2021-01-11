using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Dtos
{
    public class UserDtoUser
    {
        public int Id { get; set; }
        public string HiDee { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }   
        public string HomeAddress { get; set; } 
        public string Lga { get; set; } 
        public string State { get; set; } 
        public string Country { get; set; }      
        public string WorkStatus { get; set; }           
        public string PlaceOfWorkName { get; set; }         
        public string PlaceOfWorkAddress { get; set; }                  
        public string BankName { get; set; } 
        public string AccountNumber { get; set; } 
        public string BankCode { get; set; }      
        public string ImageNames { get; set; }   
        public string Role { get; set; }     
        public DateTime DateOfBirth { get; set; }         
        public double ContributionAmount { get; set;}   
        public double ContributionAmountLocked { get; set;}   
        public Boolean CanGuaranteeRequested { get; set; }
        public Boolean GuaranteeLocked { get; set; }
        public Boolean CanGuarantee { get; set; }
        //public string BVN { get; set; } // Do not delete     
        //public double DesiredContributionAmount { get; set;} // Do not delete                 
        //public Boolean ContributionLimitSet { get; set;} // Do not delete  
        //public Boolean OptOutOfContributionLimit { get; set;} // Do not delete  
        //public double ContributionLimit { get; set;} // Do not delete  
        //public Boolean ContributionLimitRequested { get; set;} // Do not delete  
        public Boolean EmailConfirmed { get; set; }
        public string Token { get; set; }      
        public string LastSeenDisplay { get; set; }       
        public string DateAddedDisplay { get; set; }        
    }
}