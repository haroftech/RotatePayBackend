using System;

namespace Backend.Dtos
{
    public class LogDto
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Detail { get; set; }
        public DateTime DateAdded { get; set; }  
        public string DateAddedDisplay { get; set; }      
    }
}