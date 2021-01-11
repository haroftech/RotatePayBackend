using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Backend.Helpers;
using Backend.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;
using Backend.Emails;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Configuration;

namespace Backend.Services
{
    public interface IPaymentNotificationService
    {
        Task<User> Add(PaymentNotification paymentNotification,IFormFile[] images);
        Task<PaymentNotification> Update(PaymentNotification paymentNotification,IFormFile[] images);
        Task<List<PaymentNotification>> GetByHiDee(string type,string hiDee);
    }

    public class PaymentNotificationService : IPaymentNotificationService
    {
        private DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMemoryCache memoryCache;  
        private IConfiguration _configuration;

        public PaymentNotificationService(DataContext context,IWebHostEnvironment environment,
            IEmailSenderService emailSenderService,IMemoryCache memoryCache,IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _emailSenderService = emailSenderService;
            this.memoryCache = memoryCache;
            _configuration = configuration;
        }           

        public async Task<User> Add(PaymentNotification paymentNotification,IFormFile[] images)
        {
            var user = await _context.Users.Where(x => x.Email == paymentNotification.Email).FirstOrDefaultAsync();
            if (user == null) {
                throw new AppException("User is not found");
            }      

            if (images != null) {
                for (int i = 0; i < images.Length; i++) {
                    if (images[i] != null && images[i].Length > 0) {                        
                        var file = images[i];
                        if (file.Length > 0) {
                            if (!file.ContentType.StartsWith("image")) {
                                var fileLength = file.Length/1024;
                                var maxFileLength = 5120;
                                if (fileLength > maxFileLength) {
                                    throw new AppException("Uploaded file must not be more than 5MB!");
                                }
                            }
                        }
                    }
                }
            }                   

            if (images != null) {
                for (int i = 0; i < images.Length; i++) {
                    Bitmap originalFile = null;
                    Bitmap resizedFile = null;
                    int imgWidth = 0;
                    int imgHeigth = 0;               
                    if ((i == 0) && (images[i] != null) && (images[i].Length > 0))
                    {    
                        var uploads = Path.GetFullPath(Path.Combine(GlobalVariables.ImagePath, @"images\payment"));                  
                        var file = images[i];
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
                            string uniqueFileName = paymentNotification.FirstName.Substring(0,5) + i + DateTime.Now + file.FileName;
                            string uniqueFileNameTrimmed = uniqueFileName.Replace(":", "").Replace("-", "").Replace(" ", "").Replace("/", "");

                            using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFileNameTrimmed), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                paymentNotification.ImageNames = uniqueFileNameTrimmed;

                                if (file.ContentType.StartsWith("image")) {
                                    int width = 200;
                                    int height = 200;
                                    originalFile = new Bitmap(fileStream);
                                    resizedFile = ResizeImage.GetResizedImage(fileStream,width,height,width,height);
                                }   
                            }

                            if (resizedFile != null)
                            {
                                imgWidth = resizedFile.Width;
                                imgHeigth = resizedFile.Height;
                                using (var fileStreamUp = new FileStream(Path.Combine(uploads, uniqueFileNameTrimmed), FileMode.Create))
                                {
                                    resizedFile.Save(fileStreamUp, ImageFormat.Jpeg);
                                }
                            }
                        }
                    }                    
                }                             
            }   

            DateTime paymentNotificationLocalDate_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            paymentNotificationLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            paymentNotification.DateAdded = paymentNotificationLocalDate_Nigeria;

            int length = GlobalVariables.RandomStringLengthShort();
            var randomKey = "";
            var keyIsAlreadyPresent = true;
            do
            {
                randomKey = GlobalVariables.RandomString(length);
                keyIsAlreadyPresent = _context.PaymentNotifications.Any(x => x.Reference == randomKey); 
            } while (keyIsAlreadyPresent);
            paymentNotification.Reference = randomKey;               
            
            paymentNotification.Confirmed = "No";
            paymentNotification.UpdateAllowed = true;
            await _context.PaymentNotifications.AddAsync(paymentNotification);

            user.ContributionLimitRequested = true;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            //ThreadPool.QueueUserWorkItem(o => {
                string body = "Dear " + paymentNotification.FirstName + ",<br/><br/>Thank you for submitting your payment notification.<br/><br/>" +
                    "We will confirm your payment and update your online profile.<br/><br/>" + 
                    "You will also receive an email from us as soon as we have confirmed your payment<br/><br/>" +
                    "Thanks,<br/>The RotatePay Team<br/>";          
                var message = new Message(new string[] { paymentNotification.Email }, "[RotatePay] Payment Notification Received", body, null);
                _emailSenderService.SendEmail(message);     

                string body1 = paymentNotification.FirstName + "(" + paymentNotification.Email + ") has submitted the payment notification form.<br/><br/><br/>";
                var message1 = new Message(new string[] { GlobalVariables.AdminEmail }, "[RotatePay] Payment document by " + paymentNotification.Email, body1, images);
                _emailSenderService.SendEmail(message1);
            //});   

            //await _logService.Create(log);
            return user;            
        }        

        public async Task<PaymentNotification> Update(PaymentNotification paymentNotificationParam,IFormFile[] images)
        {
            var paymentNotification = await _context.PaymentNotifications.Where(x => x.Reference == paymentNotificationParam.Reference).FirstOrDefaultAsync();
            if (paymentNotification == null) {
                throw new AppException("Payment notification not found");
            }      

            if (!paymentNotification.UpdateAllowed) {
                throw new AppException("Invalid payment notification update attempted");
            }

            if (images != null) {
                for (int i = 0; i < images.Length; i++) {
                    if (images[i] != null && images[i].Length > 0) {                        
                        var file = images[i];
                        if (file.Length > 0) {
                            if (!file.ContentType.StartsWith("image")) {
                                var fileLength = file.Length/1024;
                                var maxFileLength = 5120;
                                if (fileLength > maxFileLength) {
                                    throw new AppException("Uploaded file must not be more than 5MB!");
                                }
                            }
                        }
                    }
                }
            }        

            paymentNotification.AmountPaid = paymentNotificationParam.AmountPaid; 
            paymentNotification.PaymentChannel = paymentNotificationParam.PaymentChannel; 
            paymentNotification.PaymentDateAndTime = paymentNotificationParam.PaymentDateAndTime;           
            paymentNotification.DepositorName = paymentNotificationParam.DepositorName; 
            paymentNotification.AdditionalDetails = paymentNotificationParam.AdditionalDetails; 

            if (images != null) {
                for (int i = 0; i < images.Length; i++) {
                    Bitmap originalFile = null;
                    Bitmap resizedFile = null;
                    int imgWidth = 0;
                    int imgHeigth = 0;               
                    if ((i == 0) && (images[i] != null) && (images[i].Length > 0))
                    {    
                        var uploads = Path.GetFullPath(Path.Combine(GlobalVariables.ImagePath, @"images\payment"));                  
                        var file = images[i];
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
                            string uniqueFileName = paymentNotification.FirstName.Substring(0,5) + i + DateTime.Now + file.FileName;
                            string uniqueFileNameTrimmed = uniqueFileName.Replace(":", "").Replace("-", "").Replace(" ", "").Replace("/", "");

                            using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFileNameTrimmed), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                paymentNotification.ImageNames = uniqueFileNameTrimmed;

                                if (file.ContentType.StartsWith("image")) {
                                    int width = 200;
                                    int height = 200;
                                    originalFile = new Bitmap(fileStream);
                                    resizedFile = ResizeImage.GetResizedImage(fileStream,width,height,width,height);
                                }   
                            }

                            if (resizedFile != null)
                            {
                                imgWidth = resizedFile.Width;
                                imgHeigth = resizedFile.Height;
                                using (var fileStreamUp = new FileStream(Path.Combine(uploads, uniqueFileNameTrimmed), FileMode.Create))
                                {
                                    resizedFile.Save(fileStreamUp, ImageFormat.Jpeg);
                                }
                            }
                        }
                    }                    
                }                             
            }   

            DateTime paymentNotificationLocalDate_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            paymentNotificationLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            paymentNotification.DateEdited = paymentNotificationLocalDate_Nigeria;

            _context.PaymentNotifications.Update(paymentNotification);
            await _context.SaveChangesAsync();

            //ThreadPool.QueueUserWorkItem(o => {
                string body = "Dear " + paymentNotification.FirstName + ",<br/><br/>Thank you for updating your payment notification.<br/><br/>" +
                    "We will check your updated payment notification and update your online profile.<br/><br/>" + 
                    "You will also receive an email from us as soon as we have taken any action on your updated payment notification<br/><br/>" +
                    "Thanks,<br/>The RotatePay Team<br/>";          
                var message = new Message(new string[] { paymentNotification.Email }, "[RotatePay] Updated Payment Notification Received", body, null);
                _emailSenderService.SendEmail(message);     

                string body1 = paymentNotification.FirstName + "(" + paymentNotification.Email + ") has updated their payment notification with reference " + paymentNotification.Reference + ".<br/><br/><br/>";
                var message1 = new Message(new string[] { GlobalVariables.AdminEmail }, "[RotatePay] Updated Payment Notification by " + paymentNotification.Email, body1, images);
                _emailSenderService.SendEmail(message1);
            //});   

            //await _logService.Create(log);
            return paymentNotification;            
        }        


        public async Task<List<PaymentNotification>> GetByHiDee(string type,string hiDee)
        {
            var user = await _context.Users.Where(x => x.HiDee == hiDee).FirstOrDefaultAsync();
            if (user == null) {
                throw new AppException("User is not found");
            }              

            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                if (type == "All") {
                    return await _context.PaymentNotifications.OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.PaymentNotifications.Where(x => x.Type == type)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }                
            } else {
                if (type == "All") {
                    return await _context.PaymentNotifications.Where(x => x.Email == user.Email)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.PaymentNotifications.Where(x => (x.Email == user.Email) && (x.Type == type))
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }             
            }               
        }          
    }
}