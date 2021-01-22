using System;
using Backend.Helpers;
using System.Collections.Generic;

namespace Backend.Dtos
{
    public class TransactionDtoUser
    {            
        public string Reference { get; set; }         
        public string TransactionType { get; set; } 
        public string Email { get; set; }
        public double AmountPaid { get; set; }         
        public string PaymentChannel { get; set; }           
        public DateTime DateAdded { get; set; }           
    }
}