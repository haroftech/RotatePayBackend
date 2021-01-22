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
    public interface IGuarantorService
    {
        Task<Guarantor> Add(Guarantor guarantor);
        Task<IList<Guarantor>> GetGuarantorByHiDee(GuarantorDto guarantorDto);  
        Task<IList<Guarantor>> GetGuaranteeByHiDee(GuarantorDto guarantorDto);  
        //Task<Guarantor> Update(Guarantor guarantor);
        Task Delete(int id);
    }

    public class GuarantorService : IGuarantorService
    {
        private DataContext _context;
        private readonly IWebHostEnvironment _environment;
         private readonly IEmailSenderService _emailSenderService;
        private readonly IMemoryCache memoryCache;  

        public GuarantorService(DataContext context,IWebHostEnvironment environment,
            IEmailSenderService emailSenderService,IMemoryCache memoryCache)
        {
            _context = context;
            _environment = environment;
            _emailSenderService = emailSenderService;
            this.memoryCache = memoryCache;
        }

        public async Task<Guarantor> Add(Guarantor guarantor)
        {
            var guarantorFromDb = await _context.Users.Where(x => x.Email == guarantor.GuarantorEmail)
                .FirstOrDefaultAsync();
            var guaranteeFromDb = await _context.Users.Where(x => x.Email == guarantor.GuaranteeEmail)
                .FirstOrDefaultAsync();       

            if (guarantorFromDb  == null || (!guaranteeFromDb.CanGuarantee) || ((guarantorFromDb != null) && 
                (guarantorFromDb.Email == guarantor.GuaranteeEmail))) {
                throw new AppException("Guarantor is not found");
            }        

            if (guaranteeFromDb  == null) {
                throw new AppException("User is not found");
            }                     

            if (_context.Guarantors.Any(x => (x.GuaranteeEmail == guarantor.GuaranteeEmail) && 
                x.GuarantorEmail == guarantor.GuarantorEmail)) {
                throw new AppException("Guarantor has already been added");
            }

            guarantor.Status = "Awaiting approval";

            DateTime guarantorLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            guarantorLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            guarantor.DateAdded = guarantorLocalTime_Nigeria;          
                
            await _context.Guarantors.AddAsync(guarantor);
            await _context.SaveChangesAsync();

            string body = "Dear " + guaranteeFromDb.FirstName + ",<br/><br/>Your request for a guarantee has been sent to " + guarantorFromDb.Email +" .<br/><br/>" +
                "You will receive an email from us when your guarantor provides a guarantee for you.<br/><br/>" +
                "Thanks,<br/>The RotatePay Team<br/>";          
            var message = new Message(new string[] { guaranteeFromDb.Email }, "[RotatePay] Request for Guarantee", body, null);
            _emailSenderService.SendEmail(message);     

            string body1 = "Dear " + guarantorFromDb.FirstName + ",<br/><br/> " + guaranteeFromDb.FirstName + "(" + guaranteeFromDb.Email  + ")" +" has requested that you provide a guarantee for their RotatePay profile.<br/><br/>" +
                "You can either accept or decline to provide a guarantee.<br/><br/>" +
                "Before you accept to provide guarantee for anyone, please ensure that you know them very well and that they can easily afford to pay their proposed contribution amount monthly.<br/><br/>" +
                "To provide a guarantee, please login to your RotatePay profile and check the Gaurantor secction for more information.<br/><br/>" +
                "Thanks,<br/>The RotatePay Team<br/>";          
            var message1 = new Message(new string[] { guarantorFromDb.Email }, "[RotatePay] Request for Guarantee", body1, null);
            _emailSenderService.SendEmail(message1);    
            //await _logService.Create(log);
            return guarantor;
        }        

        public async Task<IList<Guarantor>> GetGuarantorByHiDee(GuarantorDto guarantorDto)
        {
            var user = await _context.Users.Where(x => x.HiDee == guarantorDto.UserHiDee).FirstOrDefaultAsync();
            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                return await _context.Guarantors.OrderByDescending(x => x.DateAdded).ToListAsync();    
            } else {
                return await _context.Guarantors.Where(x => x.GuarantorEmail == user.Email)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();       
            }               
        }        

        public async Task<IList<Guarantor>> GetGuaranteeByHiDee(GuarantorDto guarantorDto)
        {
            var user = await _context.Users.Where(x => x.HiDee == guarantorDto.UserHiDee).FirstOrDefaultAsync();            
            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                return null;    
            } else {
                return await _context.Guarantors.Where(x => x.GuaranteeEmail == user.Email)
                        .OrderByDescending(x => x.DateAdded).ToListAsync();   
            }                         
        }        

        /*public async Task<Guarantor> Update(Guarantor guarantorParam)
        {
            var guarantor = await _context.Guarantors.FindAsync(guarantorParam.Id);
            if (guarantor == null) {
                throw new AppException("Guarantor is not found");
            }
    
            if (_context.Guarantors.Any(x => (x.GuaranteeEmail == guarantorParam.GuaranteeEmail) && 
                x.GuarantorEmail == guarantorParam.GuarantorEmail) && (guarantor.Id != guarantorParam.Id)) {
                throw new AppException("Guarantor has already been added");
            }            

            if (!guarantor.UpdateAllowed) {
                throw new AppException("Invalid guarantor update detected");
            }

            DateTime guarantorLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            guarantorLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            guarantor.DateEdited = guarantorLocalTime_Nigeria;              

            _context.Guarantors.Update(guarantor);
            await _context.SaveChangesAsync();
          
            //await _logService.Create(log);
            return guarantor;
        }*/


        public async Task Delete(int id)
        {                      
            var guarantor = await _context.Guarantors.FindAsync(id);
            if (guarantor != null) {
                _context.Guarantors.Remove(guarantor);
                await _context.SaveChangesAsync();
                //await _logService.Create(log); 
            }
        }                 
    }
}