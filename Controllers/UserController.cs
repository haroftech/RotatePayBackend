﻿using System;
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
        [HttpPost("reg")]
        public async Task<IActionResult> Register([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {               
                var user = _mapper.Map<User>(userDtoAdmin);
                Random rnd = new Random();
                user.EmailConfirmationCode = rnd.Next(100000,1000000);                              
                var addedUser = await _userService.Create(user, userDtoAdmin.Password);

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
        [HttpPost("rqc")]
        public async Task<IActionResult> RequestResetPassword([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {             
                Random rnd = new Random(); 
                var verificationCode = rnd.Next(100000,1000000);
                var email = await _userService.RequestPasswordReset(userDtoAdmin.Email,verificationCode);  
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
        [HttpPost("rcu")]
        public async Task<IActionResult> ResetPassword([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {
                var user = await _userService.ResetPassword(userDtoAdmin.Email,userDtoAdmin.CurrentPassword,userDtoAdmin.Password,userDtoAdmin.ResetCode);  
                return Ok(new {
                    Email = user.Email
                });                
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }            
        }               

        [HttpPost("rqve")]
        public async Task<IActionResult> RequestVerification([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {               
                var email = await _userService.RequestVerification(userDtoAdmin.Email);  
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
        [HttpPost("ver")]
        public async Task<IActionResult> Verify([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {                
                var user = await _userService.Verify(userDtoAdmin.Email,userDtoAdmin.VerificationCode);  

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
        [HttpPost("lgn")]
        public async Task<IActionResult> Authenticate([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {                              
                var user = await _userService.Authenticate(userDtoAdmin.Email, userDtoAdmin.Password);

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

        [HttpPut("upd")]
        public async Task<IActionResult> Update([FromForm]UserDtoAdmin userDtoAdmin)
        {
            try 
            {  
                var user = _mapper.Map<User>(userDtoAdmin);
                var updatedUser = await _userService.Update(user, userDtoAdmin.Images, userDtoAdmin.Password);

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

        [HttpPost("gbhde")]
        public async Task<IActionResult> GetByHiDee([FromForm]UserDtoAdmin userDtoAdmin)
        {             
            var user =  await _userService.GetByHiDee(userDtoAdmin.HiDee);
            if (userDtoAdmin.HiDee.Equals(GlobalVariables.BaseKey())) {
                var userDtoAdminn = _mapper.Map<IList<UserDtoAdmin>>(user);
                return Ok(userDtoAdminn);
            } else {
                var userDtoUser = _mapper.Map<IList<UserDtoUser>>(user);
                return Ok(userDtoUser);
            }
        }   

        [HttpPost("cngrtee")]
        public async Task<IActionResult> CanGuarantee([FromForm]UserDtoAdmin userDtoAdmin)
        {             
            var user =  await _userService.CanGuarantee(userDtoAdmin.HiDee,userDtoAdmin.UserHiDee);
            if (userDtoAdmin.HiDee.Equals(GlobalVariables.BaseKey())) {
                var userDtoAdminn = _mapper.Map<IList<UserDtoAdmin>>(user);
                return Ok(userDtoAdminn);
            } else {
                var userDtoUser = _mapper.Map<IList<UserDtoUser>>(user);
                return Ok(userDtoUser);
            }
        }              

        [HttpPut("uui")]
        public async Task<IActionResult> UpdateUserInfo([FromForm]UserDtoAdmin userDtoAdmin)
        {          
                          
            try 
            {
                var updatedUser = await _userService.UpdateUserInfo(userDtoAdmin.Id,userDtoAdmin.UserCookie);
                return Ok();
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }                        

        [HttpDelete("del")]
        public async Task<IActionResult> Delete([FromForm]UserDtoAdmin userDtoAdmin)
        {             
            await _userService.Delete(userDtoAdmin.Id);
            return Ok();
        }               
    }
}