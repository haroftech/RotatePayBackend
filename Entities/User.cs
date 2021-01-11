using System;
using Backend.Helpers;
using System.Collections.Generic;

namespace Backend.Entities
{
    public class User
    {
        public int Id { get; set; }       
        public string HiDee { get; set; }   
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }         
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
        public string BVN { get; set; } // Not used for now but do not delete                  
        public string Integration { get; set; } // Not used for now but do not delete     
        public string SubAccountCode { get; set; } // Not used for now but do not delete   
        public double DesiredContributionAmount { get; set;} // Not used for now but do not delete                
        public Boolean ContributionLimitSet { get; set;} // Not used for now but do not delete     
        public Boolean OptOutOfContributionLimit { get; set;} // Not used for now but do not delete     
        public double ContributionLimit { get; set;} // Not used for now but do not delete     
        public Boolean ContributionLimitRequested { get; set;} // Not used for now but do not delete     
        public int EmailConfirmationCode { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public int EmailConfirmationAttempts { get; set; }        
        public string UserCookie { get; set; }
        public string UserCookieChangeCounter { get; set; }
        public string UserNumberOfRelatedAccounts { get; set; }
        public string RelatedAccounts { get; set; }                
        public DateTime LastSeen { get; set; }      
        public DateTime DateAdded { get; set; }
        public DateTime DateEdited { get; set; }     
        public string LastSeenDisplay 
        {
            get
            {
                return RelativeTime.getRelativeTime(LastSeen);
            }            
        }        
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

        public ICollection<UserUpload> UserUpload { get; set; }
    }
}