using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Dtos
{
    public class PaymentNotificationDtoUser
    {
        public string Reference { get; set; } 
        public string TransactionType { get; set; }    
        public string Email { get; set; }
        public double AmountPaid { get; set; }         
        public string PaymentChannel { get; set; }     
        public DateTime PaymentDateAndTime { get; set; } 
        public string DepositorName { get; set; }
        public string AdditionalDetails { get; set; }     
        public string Confirmed { get; set; }       
        public string ImageNames { get; set; }               
        public DateTime DateAdded { get; set; }      
    }
}