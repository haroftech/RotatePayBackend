using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Dtos
{
    public class UserDtoAdmin
    {
        public string UserHiDee { get; set; }  

        public int Id { get; set; }
        public string HiDee { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        
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
        public IFormFile[] Images { get; set; }    
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
        //public string BVN { get; set; } // Can be useful leter so do not delete         
        //public string Integration { get; set; } // Can be useful leter so do not delete      
        //public string SubAccountCode { get; set; } // Can be useful leter so do not delete           
        //public double DesiredContributionAmount { get; set;} // Can be useful leter so do not delete          
        //public Boolean ContributionLimitSet { get; set;} // Can be useful leter so do not delete    
        //public Boolean OptOutOfContributionLimit { get; set;} // Can be useful leter so do not delete    
        //public double ContributionLimit { get; set;} // Can be useful leter so do not delete        
        //public Boolean ContributionLimitRequested { get; set;} // Can be useful leter so do not delete  
        public int EmailConfirmationCode { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public int EmailConfirmationAttempts { get; set; } 
        public string UserCookie { get; set; }
        public string UserCookieChangeCounter { get; set; }
        public string UserNumberOfRelatedAccounts { get; set; }
        public string RelatedAccounts { get; set; }
        public string Token { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateEdited { get; set; }  
        public string LastSeenDisplay { get; set; }       
        public string DateAddedDisplay { get; set; }        

        public string CurrentPassword { get; set; }      
        public int ResetCode { get; set; }   
        public int VerificationCode { get; set; }   
        public string EmailLoggedIn { get; set; }         
    }
}