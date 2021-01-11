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
        Task<List<Transaction>> GetByHiDee(string type,string hiDee);
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

        public async Task<List<Transaction>> GetByHiDee(string type,string hiDee)
        {
            var user = await _context.Users.Where(x => x.HiDee == hiDee).FirstOrDefaultAsync();
            if (user == null) {
                throw new AppException("User is not found");
            }              

            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                if (type == "All") {
                    return await _context.Transactions.OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.Transactions.Where(x => x.Type == type)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }                
            } else {
                if (type == "All") {
                    return await _context.Transactions.Where(x => x.Email == user.Email)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                } else {
                    return await _context.Transactions.Where(x => (x.Email == user.Email) && (x.Type == type))
                        .OrderByDescending(x => x.DateAdded).ToListAsync();
                }             
            }               
        }                                
    }
}