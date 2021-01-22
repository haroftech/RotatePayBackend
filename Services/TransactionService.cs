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
    public interface ITransactionService
    {
        Task<User> Add(Transaction transaction);
        Task<List<Transaction>> GetByHiDee(string transactionType,string hiDee);
        Task Delete(string reference);
    }

    public class TransactionService : ITransactionService
    {
        private DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMemoryCache memoryCache;  
        private IConfiguration _configuration;

        public TransactionService(DataContext context,IWebHostEnvironment environment,
            IEmailSenderService emailSenderService,IMemoryCache memoryCache,IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _emailSenderService = emailSenderService;
            this.memoryCache = memoryCache;
            _configuration = configuration;
        }

        public async Task<User> Add(Transaction transaction)
        {
            var user = await _context.Users.Where(x => x.Email == transaction.Email).FirstOrDefaultAsync();
            if (user == null) {
                throw new AppException("User is not found");
            }      

            DateTime transactionLocalDate_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            transactionLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            transaction.DateAdded = transactionLocalDate_Nigeria;         
            
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            //ThreadPool.QueueUserWorkItem(o => {
                string body = "Dear " + user.FirstName + ",<br/><br/>We have confirmed your payment of <b>" + transaction.AmountPaid + "</b>.<br/><br/>" +
                    "For more information, check the transactions section of your online profile.<br/><br/>" + 
                    "Thanks,<br/>The RotatePay Team<br/>";          
                var message = new Message(new string[] { transaction.Email }, "[RotatePay] Payment Notification Confirmed", body, null);
                _emailSenderService.SendEmail(message);
            //});   

            //await _logService.Create(log);
            return user;            
        }    

        public async Task<List<Transaction>> GetByHiDee(string transactionType,string hiDee)
        {
            var user = await _context.Users.Where(x => x.HiDee == hiDee).FirstOrDefaultAsync();
            if (user == null) {
                throw new AppException("User is not found");
            }              

            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                if (transactionType == "All") {
                    return await _context.Transactions.OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.Transactions.Where(x => x.TransactionType == transactionType)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }                
            } else {
                if (transactionType == "All") {
                    return await _context.Transactions.Where(x => x.Email == user.Email)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.Transactions.Where(x => (x.Email == user.Email) && (x.TransactionType == transactionType))
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }             
            }               
        }       

        public async Task Delete(string reference)
        {                  
            var transaction = await _context.Transactions.Where(x => x.Reference == reference).FirstOrDefaultAsync();
            if (transaction != null) {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }                                 
    }
}