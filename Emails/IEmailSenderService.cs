using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Helpers;
using MimeKit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Emails
{
    public interface IEmailSenderService
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}