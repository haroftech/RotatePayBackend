using System;
using Backend.Helpers;

namespace Backend.Entities
{
    public class Log
    {

        public Log()
        {
            Id = 0;
            Owner = null;
            Detail = null;
            DateAdded = DateTime.Now;
            DateAddedDisplay = null;
        }

        public Log(int Id, string Owner, string Detail, DateTime DateAdded, string DateAddedDisplay)
        {
            this.Id = Id;
            this.Owner = Owner;
            this.Detail = Detail;
            this.DateAdded = DateAdded;
            this.DateAddedDisplay = DateAddedDisplay;
        }
                
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Detail { get; set; }
        public DateTime DateAdded { get; set; }                     
        public string DateAddedDisplay 
        {
            get
            {
                return RelativeTime.getRelativeTime(DateAdded);
            }        
            set { ; }    
        }
    }
}