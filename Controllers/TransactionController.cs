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
    public class TransactionController : Controller
    {
        private ITransactionService _transactionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;        

        public TransactionController(
            ITransactionService transactionService,         
            IMapper mapper,
            IOptions<AppSettings> appSettings)
            
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;            
        }

        [HttpPost("gbhde")]
        public async Task<IActionResult> GetByHiDee([FromForm]TransactionDto transactionDto)
        {             
            var transactionByHiDee = await _transactionService.GetByHiDee(transactionDto.Type,transactionDto.UserHiDee);
            return Ok(transactionByHiDee);
        }                         
    }
}