/*using System;
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
        Task<IList<Guarantor>> GetAll();  
        Task<Guarantor> Update(Guarantor guarantor);
        Task Delete(int id);
    }

    public class GuarantorService : IGuarantorService
    {
        private DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IMemoryCache memoryCache;  

        public GuarantorService(DataContext context,IWebHostEnvironment environment,
            IMemoryCache memoryCache)
        {
            _context = context;
            _environment = environment;
            this.memoryCache = memoryCache;
        }

        public async Task<Guarantor> Add(Guarantor guarantor)
        {
            if (_context.Guarantors.Any(x => x.Amount == guarantor.Amount)) {
                throw new AppException("Guarantor '" + guarantor.Amount + "' has already been added");
            }

            DateTime guarantorLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            guarantorLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            guarantor.DateAdded = guarantorLocalTime_Nigeria;          
                
            await _context.Guarantors.AddAsync(guarantor);
            await _context.SaveChangesAsync();
            //await _logService.Create(log);
            return guarantor;
        }        

        public async Task<IList<Guarantor>> GetAll()
        {
            return await _context.Guarantors.OrderByDescending(x => x.DateAdded).ToListAsync();
        }

        public async Task<Guarantor> Update(Guarantor guarantorParam)
        {
            var guarantor = await _context.Guarantors.FindAsync(guarantorParam.Id);
            if (guarantor == null) {
                throw new AppException("Guarantor is not found");
            }
         
            if (_context.Guarantors.Any(x => x.Amount == guarantorParam.Amount) && (guarantor.Amount != guarantorParam.Amount)) {
                throw new AppException("Guarantor '" + guarantorParam.Amount + "' has already been added");
            }         

            // update guarantor properties     
            guarantor.Status = guarantorParam.Status;

            DateTime guarantorLocalTime_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            guarantorLocalTime_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            guarantor.DateEdited = guarantorLocalTime_Nigeria;              

            _context.Guarantors.Update(guarantor);
            await _context.SaveChangesAsync();
          
            //await _logService.Create(log);
            return guarantor;
        }


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
}*/