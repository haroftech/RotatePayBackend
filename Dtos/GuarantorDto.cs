using System;
using Backend.Helpers;
using System.Collections.Generic;

namespace Backend.Dtos
{
    public class GuarantorDto
    {
        public string Type { get; set; }
        public string UserHiDee { get; set; }  
        public int Id { get; set; }
        public string Status { get; set; }
        public string GuarantorEmail { get; set; }
        public string GuaranteeEmail { get; set; }
        public double ContributionAmount { get; set; }
        public Boolean DeleteAllowed { get; set; }    
        public DateTime DateAdded { get; set; }
        public DateTime DateEdited { get; set; }   
        public string DateAddedDisplay  { get; set; }        
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
    }
}