using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Dtos;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Backend.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Backend.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;        

        public UserController(
            IUserService userService,         
            IMapper mapper,
            IOptions<AppSettings> appSettings)
            
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;            
        }

        [AllowAnonymous]
        [HttpPost("reg/{usrreg}")]
        public async Task<IActionResult> Register(string ow, string dt, [FromForm]UserDtoAdmin UserDtoAdmin)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  
                var user = _mapper.Map<User>(UserDtoAdmin);
                Random rnd = new Random();
                user.EmailConfirmationCode = rnd.Next(100000,1000000);                              
                var addedUser = await _userService.Create(newLog, user, UserDtoAdmin.Password);

                var tokenString = "";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, addedUser.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(365),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token); 

                var userDtoUser = _mapper.Map<UserDtoUser>(addedUser);
                userDtoUser.Token = tokenString;
                return Ok(userDtoUser);               
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }   

        [AllowAnonymous]
        [HttpPost("rqc/{usrrqc}")]
        public async Task<IActionResult> RequestResetPassword(string ow, string dt, string em)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  
                Random rnd = new Random(); 
                var verificationCode = rnd.Next(100000,1000000);
                var email = await _userService.RequestPasswordReset(newLog,em,verificationCode);  
                return Ok(new {
                    Email = email,             
                });            
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [AllowAnonymous]
        [HttpPost("rcu/{usrrcu}")]
        public async Task<IActionResult> ResetPassword(string ow, string dt, string em, string cpd, string pd, int rc)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  

                var user = await _userService.ResetPassword(newLog,em,cpd,pd,rc);  
                return Ok(new {
                    Email = user.Email
                });                
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }            
        }               

        [HttpPost("rqve/{usrrqve}")]
        public async Task<IActionResult> RequestVerification(string ow, string dt, string em)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  
                var email = await _userService.RequestVerification(newLog,em);  
                return Ok(new {
                    Email = email,             
                });                 
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }            
        }       
        
        [AllowAnonymous]
        [HttpPost("ver/{usrver}")]
        public async Task<IActionResult> Verify(string ow, string dt, string em, int vc)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  
                var user = await _userService.Verify(newLog,em,vc);  

                var tokenString = "";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(365),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token); 

                var userDtoUser = _mapper.Map<UserDtoUser>(user);
                userDtoUser.Token = tokenString;
                return Ok(userDtoUser);  
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }            
        }                  

        [AllowAnonymous]
        [HttpPost("lgn/{usrlgn}")]
        public async Task<IActionResult> Authenticate(string ow, string dt, [FromBody]UserDtoAdmin UserDtoAdmin)
        {
            try 
            {                
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                  
                var user = await _userService.Authenticate(newLog,UserDtoAdmin.Email, UserDtoAdmin.Password);

                if (user == null) {
                    return BadRequest("Email or password is incorrect");
                }                

                var tokenString = "";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(365),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);    

                var userDtoUser = _mapper.Map<UserDtoUser>(user);
                userDtoUser.Token = tokenString;
                return Ok(userDtoUser);  
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
                
        }

        [HttpGet("gal/{usrgal}")]
        public async Task<IActionResult> GetAll(string ow, string dt)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var users = await _userService.GetAll(newLog);
            var userDtoAlls = _mapper.Map<IList<UserDtoAll>>(users);
            return Ok(userDtoAlls);
        }                

        [HttpPut("upd/{usrupd}")]
        public async Task<IActionResult> Update(string ow, string dt, int id, [FromForm]UserDtoAdmin UserDtoAdmin)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };          
                var user = _mapper.Map<User>(UserDtoAdmin);
                user.Id = id;                      

                var updatedUser = await _userService.Update(newLog, user, UserDtoAdmin.Images, UserDtoAdmin.Password);

                var tokenString = "";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, updatedUser.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(365),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);    

                var userDtoUser = _mapper.Map<UserDtoUser>(updatedUser);
                userDtoUser.Token = tokenString;
                return Ok(userDtoUser);  
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("gbud/{usrgbud}")]
        public async Task<IActionResult> GetByUserId(string ow, string dt, int id)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var user = await _userService.GetByUserId(newLog,id);
            var userDtoAll = _mapper.Map<UserDtoAll>(user);
            return Ok(userDtoAll);
        }   

        [HttpGet("gbem/{usrgbem}")]
        public async Task<IActionResult> GetByEmail(string ow, string dt, string em)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var user =  await _userService.GetByEmail(newLog,em);
            var userDtoAll = _mapper.Map<UserDtoAll>(user);
            return Ok(userDtoAll);
        }      

        [HttpPut("uui/{usruui}")]
        public async Task<IActionResult> UpdateUserInfo(string ow, string dt, int id, string uc)
        {          
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };                           
            try 
            {
                var updatedUser = await _userService.UpdateUserInfo(newLog,id,uc);
                return Ok();
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }          

        [HttpGet("guui/{usrguui}")]
        public async Task<IActionResult> GetUserUpdatedInfo(string ow, string dt, string em, string emli)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var userUpdatedInfo =  await _userService.GetUserUpdatedInfo(newLog,em,emli);
            var userDtoAll = _mapper.Map<UserDtoAll>(userUpdatedInfo);
            var userDtoUser = _mapper.Map<UserDtoUser>(userUpdatedInfo);
            var userDtoAdmin = _mapper.Map<UserDtoAdmin>(userUpdatedInfo);
            if(emli == "RotatePay") {
                return Ok(userDtoAdmin);
            } else if (em == emli) {
                return Ok(userDtoUser);                       
            } else {
                return Ok(userDtoAll);                
            }
        }                  

        [HttpPost("adpayn/{usradpayn}")]
        public async Task<IActionResult> AddPaymentNotification(string ow, string dt, [FromForm]PaymentNotificationDto paymentNotificationDto)
        {
            try 
            {
                var newLog = new Log {
                    Owner = ow,
                    Detail = dt
                };                          
                var paymentNotification = _mapper.Map<PaymentNotification>(paymentNotificationDto);                    
                var addedPaymentNotification = await _userService.AddPaymentNotification(newLog,paymentNotification);
                return Ok(addedPaymentNotification);               
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }   

        [HttpGet("galpayn/{usrgalpayn}")]
        public async Task<IActionResult> GetAllPaymentNotification(string ow, string dt, string type)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var allPaymentNotification = await _userService.GetAllPaymentNotification(newLog,type);
            return Ok(allPaymentNotification);
        }      

        [HttpGet("paynbem/{usrpaynbem}")]
        public async Task<IActionResult> PaymentNotificationByEmail(string ow, string dt, string type, string em)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            var paymentNotificationByUserId = await _userService.PaymentNotificationByEmail(newLog,type,em);
            return Ok(paymentNotificationByUserId);
        }     

        [HttpDelete("del/{usrdel}")]
        public async Task<IActionResult> Delete(string ow, string dt, int id)
        {
            var newLog = new Log {
                Owner = ow,
                Detail = dt
            };              
            await _userService.Delete(newLog,id);
            return Ok();
        }               
    }
}