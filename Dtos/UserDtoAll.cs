using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Backend.Helpers;
using Backend.Entities;

namespace Backend.Dtos
{
    public class UserDtoAll
    {
        public int Id { get; set; }

        public string FirstName { get; set; }  
        public string State { get; set; } 
        public string Country { get; set; }                     
        public string ImageNames { get; set; }          
        public double ContributionAmount { get; set;}    
        public Boolean GuaranteeSuccessful { get; set; }   
        public Boolean CanGuarantee { get; set; }
        public Boolean EmailConfirmed { get; set;}                           
        public string LastSeenDisplay { get; set; }       
        public string DateAddedDisplay { get; set; }   
    }
}