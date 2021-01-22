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
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }  

    public interface IUserService
    {
        Task<User> Create(User user, string password);
        Task<String> RequestPasswordReset(string email,int verificationCode);
        Task<User> ResetPassword(string email,string currentPassword,string password,int resetCode);
        Task<String> RequestVerification(string email);      
        Task<User> Verify(string email,int verificationCode);
        Task<User> Authenticate(string email, string password);
        Task<User> Update(User user, IFormFile[] images, string password = null);      
        Task<IList<User>> GetByHiDee(string hiDee);
        Task<IList<User>> CanGuarantee(string hiDee,string userHiDee);
        Task<User> UpdateUserInfo( int id, string userCookie);
        Task Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMemoryCache memoryCache;  
        private IConfiguration _configuration;

        public UserService(DataContext context,IWebHostEnvironment environment,
            IEmailSenderService emailSenderService,IMemoryCache memoryCache,IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _emailSenderService = emailSenderService;
            this.memoryCache = memoryCache;
            _configuration = configuration;
        }


        public async Task<User> Create(User user,string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password)) {
                throw new AppException("Password is required");
            } 

            if (_context.Users.Any(x => x.Email == user.Email)) {
                throw new AppException("Email '" + user.Email + "' is not available");
            }         

            if (user.FirstName.Length < 5) {
                throw new AppException("First name is too short");
            }   

            if (user.FirstName.Length > 15) {
                throw new AppException("First name is too long");
            }     

            if (user.Email.ToUpper().Contains("ROTATE") || user.FirstName.ToUpper().Contains("ROTATE") 
                || user.Email.ToUpper().Contains("PAY") || user.FirstName.ToUpper().Contains("PAY")) {
                throw new AppException("Surname or Firstname is not available");
            }                                         

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;            
            user.Role = "User";
            user.ImageNames = "......profilePictureRTPay......officialIDCardRTPay......utilityBillRTPay"; //......bankStatementRTPay......workIDCardRTPay";

            DateTime userLocalDate_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            userLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            user.DateAdded = userLocalDate_Nigeria;
            user.DateEdited = userLocalDate_Nigeria;              
            user.LastSeen = userLocalDate_Nigeria;

            int length = GlobalVariables.RandomStringLength();
            var randomKey = "";
            var keyIsAlreadyPresent = true;
            do
            {
                randomKey = GlobalVariables.RandomString(length);
                keyIsAlreadyPresent = _context.Users.Any(x => x.HiDee == randomKey); 
            } while (keyIsAlreadyPresent);
            user.HiDee = randomKey;   

            //ThreadPool.QueueUserWorkItem(o => {
                var url = GlobalVariables.URL + "/vaccount?em=" + user.Email + "&vc=" + user.EmailConfirmationCode;
                var link = $"<a href='{url}'>Click here</a>";
                string body = "Dear " + user.FirstName + ",<br/><br/>Thank you for registering with RotatePay.<br/><br/>" +
                    "Click on the link below to confirm your email address. <br/><br/>" + link + "<br/><br/>" +
                    " If the link above can not be clicked, copy and paste the url below in your browser<br/>" + url + "<br/><br/><br/>" +
                    "Thanks,<br/>The RotatePay Team<br/>";                
                var message = new Message(new string[] { user.Email }, "[RotatePay] Confirm your email address", body, null);
                _emailSenderService.SendEmail(message);     
            //});  

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            //await _logService.Create(log);
            return user;
        }

        public async Task<String> RequestPasswordReset(string email,int verificationCode)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();   
            if (user == null) {
                throw new AppException("Invalid password reset attempt detected");//throw new AppException("User is not found");
            }             

            user.EmailConfirmationCode = verificationCode;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();    

            //ThreadPool.QueueUserWorkItem(o => {
                var url = GlobalVariables.URL + "/password?em=" + user.Email + "&vc=" + verificationCode;
                var link = $"<a href='{url}'>Click here</a>";
                string body = "Dear " + user.FirstName + ",<br/><br/>You have requested to change your RotatePay account password.<br/><br/>" +
                    "Click on the link below to change your account password. <br/><br/>" + link + "<br/><br/>" +
                    " If the link above can not be clicked, copy and paste the url below in your browser<br/>" + url + "<br/><br/><br/>" +
                    "Thanks,<br/>The RotatePay Team<br/>";              
                var message = new Message(new string[] { user.Email }, "[RotatePay] Change your Account Password", body, null);
                _emailSenderService.SendEmail(message);     
            //});               

            return JsonConvert.SerializeObject(user.Email);             
        } 

        public async Task<User> ResetPassword(string email,string currentPassword,string password,int resetCode)
        {
            if (string.IsNullOrWhiteSpace(password)) {
                throw new AppException("Invalid password reset attempt detected");
            }
                        
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();   
            if (user == null) {
                throw new AppException("Invalid password reset attempt detected");//throw new AppException("User is not found");
            }
            if (string.IsNullOrWhiteSpace(currentPassword)) {
                if (user.EmailConfirmationCode == resetCode) {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.EmailConfirmed = true;
                    Random rnd = new Random();
                    user.EmailConfirmationCode = new Random().Next(100000,1000000);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    //await _logService.Create(log);
                    return user;                  
                } else {
                    throw new AppException("Invalid password reset attempt detected");//throw new AppException("Wrong verification code");
                } 
            } else {
                if (!VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt)) {
                    throw new AppException("Invalid password reset attempt detected");
                } else {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.EmailConfirmed = true;
                    Random rnd = new Random();
                    user.EmailConfirmationCode = new Random().Next(100000,1000000);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    //await _logService.Create(log);localStorage.removeItem('user');
                    return user;                      
                }                        
            }
        }           

        public async Task<String> RequestVerification(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();   
            if (user == null) {
                throw new AppException("Invalid verification attempt detected");
            } 

            if (user.EmailConfirmed) {
                throw new AppException("Invalid verification attempt by user detected");
            }

            //ThreadPool.QueueUserWorkItem(o => {
                var url = GlobalVariables.URL + "/vaccount?em=" + user.Email + "&vc=" + user.EmailConfirmationCode;
                var link = $"<a href='{url}'>Click here</a>";
                string body = "Dear " + user.FirstName + ",<br/><br/>You have requested for a verification email.<br/><br/>" +
                    "Click on the link below to confirm your email address. <br/><br/>" + link + "<br/><br/>" +
                    " If the link above can not be clicked, copy and paste the url below in your browser<br/>" + url + "<br/><br/><br/>" +
                    "Thanks,<br/>The RotatePay Team<br/>";              
                var message = new Message(new string[] { user.Email }, "[RotatePay] Confirm your email address", body, null);
                _emailSenderService.SendEmail(message);       
            //});                      

            return JsonConvert.SerializeObject(user.Email);  
        }           

        public async Task<User> Verify(string email,int verificationCode)
        {
            var user = _context.Users.Where(x => x.Email == email).FirstOrDefault();   
            if (user == null) {
                throw new AppException("Invalid user attempt detected");//throw new AppException("User is not found");
            } 

            if (user.EmailConfirmed) {
                return user; 
            } else if (user.EmailConfirmationCode == verificationCode) {
                user.EmailConfirmed = true;     
                Random rnd = new Random();       
                user.EmailConfirmationCode = new Random().Next(100000,1000000);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;                 
            } else {
                throw new AppException("Invalid verification attempt detected");
            }             
        }   

        public async Task<User> Authenticate(string email,string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                return null;
            }                

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

            // check if email exists
            if (user == null) {
                return null;
            }                           

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
                return null;
            }          

            // authentication successful
            return user;
        }

        public async Task<User> Update(User userParam, IFormFile[] images, string password = null)
        {
            if (userParam.FirstName.Length < 5) {
                throw new AppException("First name is too short");
            }   

            if (userParam.FirstName.Length > 15) {
                throw new AppException("First name is too long");
            }              

            if (userParam.Surname.ToUpper().Contains("ROTATE") || userParam.FirstName.ToUpper().Contains("ROTATE") 
                || userParam.Surname.ToUpper().Contains("PAY") || userParam.FirstName.ToUpper().Contains("PAY")) {
                throw new AppException("Surname or Firstname is not available");
            }      

            var user = await _context.Users.FindAsync(userParam.Id);
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
                                    throw new AppException("Each uploaded file must not be more than 5MB!");
                                }
                            }
                        }
                    }
                }
            }                                                

            // update user properties
            user.Surname = userParam.Surname;
            user.FirstName = userParam.FirstName;
            user.PhoneNumber = userParam.PhoneNumber;            
            user.HomeAddress = userParam.HomeAddress;
            user.Lga = userParam.Lga;
            user.State = userParam.State;
            user.Country = userParam.Country;  
            user.WorkStatus = userParam.WorkStatus;  
            user.PlaceOfWorkName = userParam.PlaceOfWorkName;
            user.PlaceOfWorkAddress = userParam.PlaceOfWorkAddress;    
            user.DateOfBirth = userParam.DateOfBirth;
            user.ContributionAmount = userParam.ContributionAmount;
            user.CanGuaranteeRequested = userParam.CanGuaranteeRequested;
            //user.BVN = userParam.BVN;          
            //user.DesiredContributionAmount = userParam.DesiredContributionAmount;
            //user.OptOutOfContributionLimit = userParam.OptOutOfContributionLimit;
            user.BankName = userParam.BankName;
            user.AccountNumber = userParam.AccountNumber;
            if (!String.IsNullOrEmpty(userParam.BankCode)) {
                user.BankCode = userParam.BankCode;
            }

            string[] userImageNamesDB = new string[images.Length];
            if (user.ImageNames != null) {
                var imageNamesArray = user.ImageNames.Split("......");
                userImageNamesDB = imageNamesArray.Skip(1).ToArray();
            }             

            string profilePictureFileName = "";
            string officialIDCardFileName = "";
            string utilityBillFileName = "";
            /*string bankStatementFileName = "";
            string workIDCardFileName = "";*/
            string[] userImageNames = new string[images.Length];
            if (images != null) {
                for (int i = 0; i < images.Length; i++) {
                    Bitmap originalFile = null;
                    Bitmap resizedFile = null;
                    int imgWidth = 0;
                    int imgHeigth = 0;    
                    if (images[i] != null && images[i].Length > 0)
                    {
                        var uploads = Path.GetFullPath(Path.Combine(GlobalVariables.ImagePath, @"images\user"));
                        var file = images[i];
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
                            string uniqueFileName = userParam.FirstName + i + DateTime.Now + file.FileName;
                            string uniqueFileNameTrimmed = uniqueFileName.Replace(":", "").Replace("-", "").Replace(" ", "").Replace("/", "");

                            if (i == 0) {
                                if (file.FileName == "profilePictureRTPay") {
                                    profilePictureFileName = file.FileName;
                                    userImageNames[i] = file.FileName;
                                } else {
                                    profilePictureFileName = uniqueFileNameTrimmed;
                                    userImageNames[i] = uniqueFileNameTrimmed;
                                }                                
                            } else if (i == 1) {
                                if (file.FileName == "officialIDCardRTPay") {
                                    officialIDCardFileName = file.FileName;
                                    userImageNames[i] = file.FileName;
                                } else {
                                    officialIDCardFileName = uniqueFileNameTrimmed;
                                    userImageNames[i] = uniqueFileNameTrimmed;
                                }                                                                
                            } else if (i == 2) {
                                if (file.FileName == "utilityBillRTPay") {
                                    utilityBillFileName = file.FileName;
                                    userImageNames[i] = file.FileName;
                                } else {
                                    utilityBillFileName = uniqueFileNameTrimmed;
                                    userImageNames[i] = uniqueFileNameTrimmed;
                                }                                  
                            } /*else if (i == 3) {
                                if (file.FileName == "bankStatementRTPay") {
                                    bankStatementFileName = file.FileName;
                                    userImageNames[i] = file.FileName;
                                } else {
                                    bankStatementFileName = uniqueFileNameTrimmed;
                                    userImageNames[i] = uniqueFileNameTrimmed;
                                }    
                            } else if (i == 4) {
                                if (file.FileName == "workIDCardRTPay") {
                                    workIDCardFileName = file.FileName;
                                    userImageNames[i] = file.FileName;
                                } else {
                                    workIDCardFileName = uniqueFileNameTrimmed;
                                    userImageNames[i] = uniqueFileNameTrimmed;
                                }                                                                    
                            }    */                        

                            using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFileNameTrimmed), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);

                                if (file.ContentType.StartsWith("image")) {
                                    int width = 100;
                                    int height = 100;
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
                    } else {
                        if (i == 0) {
                            profilePictureFileName = userImageNamesDB[i];
                            userImageNames[i] = userImageNamesDB[i];                            
                        } else if (i == 1) {
                            officialIDCardFileName = userImageNamesDB[i];
                            userImageNames[i] = userImageNamesDB[i];                                                                                               
                        } else if (i == 2) {
                            utilityBillFileName = userImageNamesDB[i];
                            userImageNames[i] = userImageNamesDB[i];                                                                 
                        } /*else if (i == 3) {
                            bankStatementFileName = userImageNamesDB[i];
                            userImageNames[i] = userImageNamesDB[i];                                                                  
                        } else if (i == 4) {
                            workIDCardFileName = userImageNamesDB[i];
                            userImageNames[i] = userImageNamesDB[i];   
                        } */                   
                    }                 
                }                             
            }                

            var userUpload = new UserUpload {
                UserId = user.Id,
                ProfilePicture = profilePictureFileName,
                OfficialIDCard = officialIDCardFileName,
                UtilityBill = utilityBillFileName,
                /*BankStatement = bankStatementFileName,                
                WorkIDCard = workIDCardFileName,*/
            };

            user.ImageNames = "";
            userUpload.ImageNames = "";
            for (int i = 0; i < images.Length; i++) {
                user.ImageNames = user.ImageNames + "......" + userImageNames[i];
                userUpload.ImageNames = userUpload.ImageNames + "......" + userImageNames[i];
            }                

            DateTime userLocalDate_Nigeria = new DateTime();
            string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
            userLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                       
            user.DateEdited = userLocalDate_Nigeria;            
            user.LastSeen = userLocalDate_Nigeria;              

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            if (userParam.ImageNames.Contains("Changed")) { 
                //ThreadPool.QueueUserWorkItem(o => {
                    string body = "<br/><br/>The attached documents were sent by: " + user.FirstName + " (" + user.Email + ")<br/><br/>";                   
                    var message = new Message(new string[] { GlobalVariables.DocumentEmail }, "[RotatePay] Profile documents by " + user.Email, body, images);
                    _emailSenderService.SendEmail(message);    
                //});   
                userUpload.DateAdded = userLocalDate_Nigeria;
                _context.UserUploads.Add(userUpload);     
                await _context.SaveChangesAsync();           
            }

            //await _logService.Create(log);
            return user;
        }

        public async Task<IList<User>> GetByHiDee(string hiDee)
        {
            var user = await _context.Users.Where(x => x.HiDee == hiDee).FirstOrDefaultAsync();
            if (user.HiDee.Equals(GlobalVariables.BaseKey())) {
                return await _context.Users.OrderByDescending(x => x.DateAdded).ToListAsync();            
            } else {
                return await _context.Users.Where(x => x.HiDee == hiDee)
                    .OrderByDescending(x => x.DateAdded).ToListAsync();           
            }            
        } 

        public async Task<IList<User>> CanGuarantee(string hiDee,string userHiDee)
        {
            var user = await _context.Users.Where(x => x.HiDee == hiDee).FirstOrDefaultAsync();
            if (user == null || (!userHiDee.Equals(GlobalVariables.BaseKey()))) {
                throw new AppException("User is not found");
            }             
            user.CanGuarantee = !user.CanGuarantee;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            
            return await _context.Users.OrderByDescending(x => x.DateAdded).ToListAsync();
        }                

        public async Task<User> UpdateUserInfo(int id,string userCookie)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null) {
                var userCookieFromDb = user.UserCookie;
                var userCookieFromDbCounter = 0;
                if (user.UserCookieChangeCounter != null) {
                    userCookieFromDbCounter = Int32.Parse(user.UserCookieChangeCounter);                    
                }
                if ((userCookieFromDb == null) || (userCookieFromDb != userCookie)) {
                    userCookieFromDbCounter++;
                    user.UserCookieChangeCounter = userCookieFromDbCounter.ToString();
                    user.UserCookie = userCookie;
                }                

                if (!String.IsNullOrEmpty(userCookie)) {
                    var relatedAccounts = await _context.Users.Where(x => x.UserCookie == userCookie).Select(x => x.Email).ToListAsync();
                    if (!relatedAccounts.Contains(user.Email)) {
                        relatedAccounts.Add(user.Email);
                    }                    
                    if (String.IsNullOrEmpty(user.RelatedAccounts)) {
                        for (var i = 0; i < relatedAccounts.Count; i++) {
                            user.RelatedAccounts = user.RelatedAccounts + "..." + relatedAccounts[i];
                        }
                        user.UserNumberOfRelatedAccounts = CountSubstring(user.RelatedAccounts, "...").ToString();
                    } else {
                        for (var i = 0; i < relatedAccounts.Count; i++) {
                            if (user.RelatedAccounts.IndexOf(relatedAccounts[i], StringComparison.CurrentCultureIgnoreCase) == -1) {
                                user.RelatedAccounts = user.RelatedAccounts + "..." + relatedAccounts[i];
                            }
                        }                             
                        user.UserNumberOfRelatedAccounts =  CountSubstring(user.RelatedAccounts, "...").ToString();
                    }
                }                     

                DateTime userLocalDate_Nigeria = new DateTime();
                string windowsTimeZone = GetWindowsFromOlson.GetWindowsFromOlsonFunc("Africa/Lagos");
                userLocalDate_Nigeria = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));                
                user.LastSeen = userLocalDate_Nigeria;      
                _context.Users.Update(user);

                await _context.SaveChangesAsync();                
            }                        
            return user;
        }             

        public async Task Delete(int id)
        {                  
            var user = await _context.Users.FindAsync(id);
            if (user != null || ((user != null) && (!user.HiDee.Equals(GlobalVariables.BaseKey())))) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) {
                throw new ArgumentNullException("password");
            } 

            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            } 

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64) {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128) {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            } 

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) {
                        return false;
                    } 
                }
            }

            return true;
        }           

        public static int CountSubstring(string text, string value)
        {                  
            int count = 0, minIndex = text.IndexOf(value, 0);
            while (minIndex != -1)
            {
                minIndex = text.IndexOf(value, minIndex + value.Length);
                count++;
            }
            return count;
        }          
    }
}