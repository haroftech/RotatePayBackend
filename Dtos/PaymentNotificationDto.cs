using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Dtos
{
    public class PaymentNotificationDto
    {
        public string UserHiDee { get; set; }     
                
        public int Id { get; set; }       
        public string Reference { get; set; } 
        public string Type { get; set; }    
        public string Email { get; set; }
        public string FirstName { get; set; }
        public double AmountPaid { get; set; }         
        public string PaymentChannel { get; set; }     
        public DateTime PaymentDateAndTime { get; set; } 
        public string DepositorName { get; set; }
        public string AdditionalDetails { get; set; }     
        public string Confirmed { get; set; }      
        public Boolean UpdateAllowed { get; set; }    
        public IFormFile[] Images { get; set; }    
        public string ImageNames { get; set; }               
        public DateTime DateAdded { get; set; }      
        public DateTime DateEdited { get; set; }      
        public string DateAddedDisplay { get; set; } 
    
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