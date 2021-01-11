using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Helpers;

namespace Backend.Emails
{
    public class EmailConfiguration
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    } 
}