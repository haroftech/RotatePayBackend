using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Helpers;
using MimeKit;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Backend.Emails
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFile[] Attachments { get; set; }
    
        public Message(IEnumerable<string> to, string subject, string content, IFormFile[] attachments)
        {
            To = new List<MailboxAddress>();
    
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));
            Subject = subject;
            Content = content;      
            Attachments = attachments;  
        }
    }
}