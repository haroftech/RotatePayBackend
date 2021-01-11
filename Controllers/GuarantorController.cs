/*using System.IO;
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
    public class GuarantorController : Controller
    {
        private IGuarantorService _guarantorService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;        

        public GuarantorController(
            IGuarantorService guarantorService,          
            IMapper mapper,
            IOptions<AppSettings> appSettings)
            
        {
            _guarantorService = guarantorService;
            _mapper = mapper;
            _appSettings = appSettings.Value;            
        }            

        [HttpPost("ad")]
        public async Task<IActionResult> Add([FromForm]GuarantorDto guarantorDto)
        {  
            try 
            {              
                var guarantor = _mapper.Map<Guarantor>(guarantorDto);            
                var addedGuarantor =  await _guarantorService.Add(guarantor);                
                return Ok(addedGuarantor);        
                    
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }  

        [HttpGet("gal")]
        public async Task<IActionResult> GetAll()
        {             
            var guarantors = await _guarantorService.GetAll();
            var guarantorDtos = _mapper.Map<IList<GuarantorDto>>(guarantors);
            return Ok(guarantorDtos);
        }  

        [HttpPut("upd")]
        public async Task<IActionResult> Update([FromForm]GuarantorDto guarantorDto)
        {
            try 
            {        
                var guarantor = _mapper.Map<Guarantor>(guarantorDto);
                var updatedGuarantor = await _guarantorService.Update(guarantor);
                return Ok(updatedGuarantor);
            } 
            catch(AppException ex)
            {
                return BadRequest(ex.Message);
            }
        }     

        [HttpDelete("del")]
        public async Task<IActionResult> Delete([FromForm]GuarantorDto guarantorDto)
        {        
            await _guarantorService.Delete(guarantorDto.Id);
            return Ok();
        }               
    }
}*/