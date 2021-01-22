using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Dtos
{
    public class UserDtoUser
    {
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
        public string ImageNames { get; set; }   
        public string Role { get; set; }     
        public DateTime DateOfBirth { get; set; }         
        public double ContributionAmount { get; set;}   
        public double ContributionAmountLocked { get; set;} // Locked when guarantee is requested and remains locked no guarantee is requested              
        public Boolean CanGuaranteeRequested { get; set; }
        public Boolean CanGuarantee { get; set; }            
        public Boolean GuaranteeLocked { get; set; } // Locked after all guarantors agrees to give a guarantee and remains locked until a guarantor withdraws (after the contribution cycle)                  
        public Boolean GuaranteeSuccessful { get; set; }  
        public Boolean ActivationRequested { get; set; }
        public Boolean ActivationFeePaid { get; set; }  
        public Boolean EmailConfirmed { get; set; }
        public string Token { get; set; }        
    }
}