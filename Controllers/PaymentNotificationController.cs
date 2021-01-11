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
    public class PaymentNotificationController : Controller
    {
        private IPaymentNotificationService _paymentNotificationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;        

        public PaymentNotificationController(
            IPaymentNotificationService paymentNotificationService,         
            IMapper mapper,
            IOptions<AppSettings> appSettings)
            
        {
            _paymentNotificationService = paymentNotificationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;            
        }

        [HttpPost("ad")]
        public async Task<IActionResult> Add([FromForm]PaymentNotificationDto paymentNotificationDto)
        {
            try 
            {                       
                var paymentNotification = _mapper.Map<PaymentNotification>(paymentNotificationDto);                    
                var userAddedPaymentNotification = await _paymentNotificationService.Add(paymentNotification,paymentNotificationDto.Images);

                var tokenString = "";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, userAddedPaymentNotification.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(365),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);    

                var userDtoUser = _mapper.Map<UserDtoUser>(userAddedPaymentNotification);
                userDtoUser.Token = tokenString;
                return Ok(userDtoUser);          
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }   

        [HttpPost("upd")]
        public async Task<IActionResult> Update([FromForm]PaymentNotificationDto paymentNotificationDto)
        {
            try 
            {                       
                var paymentNotification = _mapper.Map<PaymentNotification>(paymentNotificationDto);                    
                var updatedPaymentNotification = await _paymentNotificationService.Update(paymentNotification,paymentNotificationDto.Images);
                return Ok(updatedPaymentNotification);          
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }   

        [HttpPost("gbhde")]
        public async Task<IActionResult> GetByHiDee([FromForm]PaymentNotificationDto paymentNotificationDto)
        {             
            var paymentNotificationByHiDee = await _paymentNotificationService.GetByHiDee(paymentNotificationDto.Type,paymentNotificationDto.UserHiDee);
            return Ok(paymentNotificationByHiDee);
        }    
    }
}