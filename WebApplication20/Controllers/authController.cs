using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication20.Data;
using WebApplication20.Helper;
using WebApplication20.Models;
using WebApplication20.ModelView;

namespace WebApplication20.Controllers
{
    [Route(template: "api")]
    [ApiController]
    
    // [EnableCors("AllowSetOrigins")]

    public class authController : Controller
    {

        private readonly IUserRepository _repository;
        private readonly IOtpRepository _OtpRepository;
        private readonly jwtService _jwtService;

        public  authController(IUserRepository repository,jwtService jwtService, IOtpRepository Otprepository)
        {
            _repository = repository;
            _jwtService = jwtService;
            _OtpRepository = Otprepository;
        }
       [HttpPost("register")]
        public IActionResult Register(RegisterModel reg)
        {

            var user = new User
            {
                Name = reg.Name,
                Email = reg.Email,
                Passsword = BCrypt.Net.BCrypt.HashPassword(reg.Password)
            };
          
            return Created("Ok", _repository.Create(user));
        }


       
      
        [HttpPost("login")]
        public IActionResult Login (LoginModel login)
        {
            var user = _repository.GetByEmail(login.Email);
            if (user == null) return BadRequest(new { message = "Error User" });

            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Passsword))
            {
                return BadRequest(new { message = "Error : Password" });
            }
            var jwt = _jwtService.Generate(user.Id);
            DateTime DateNow = new DateTime();
            DateTime DateNow2 = DateTime.Now.AddSeconds(0);
            var OtpUser = _OtpRepository.GetById(user.Id);
            var diffInSeconds = (DateNow2 - OtpUser.StartTime).TotalSeconds;
                    if (diffInSeconds >= 0 && diffInSeconds <= 30 && login.OtpCode == OtpUser.OtpToken)
                    {
                        Response.Cookies.Append("jwt", jwt, new CookieOptions
                        {
                            HttpOnly = true,
                            IsEssential = true,
                            Secure = false,
                            SameSite = SameSiteMode.Lax,
                            Domain = "localhost", //using https://localhost:44340/ here doesn't work
                            Expires = DateTime.UtcNow.AddDays(1)
                        });

                    }
                    else
                    {
                          return BadRequest(new { message = "Error : OTP" });
                    }

            return Ok (new { message = "ok"}) ;
        }
        [AllowAnonymous]
        [HttpPost("otp")]
        public IActionResult Otp(OtpModel otp)
        {

            var key = KeyGeneration.GenerateRandomKey(20);
            var totp = new Hotp(key, mode: OtpHashMode.Sha512, hotpSize: 8);

            var OtpG = totp.ComputeHOTP(6);
            OtpTableInfo NewOTPToken = new OtpTableInfo
            {
                StartTime = otp.OtpCodeDate,
                userId = otp.userId,
                OtpToken = OtpG
            };
            _OtpRepository.Create(NewOTPToken);


            return new JsonResult(new { OtpGenerate = OtpG, DateTimeStart = otp.OtpCodeDate });



            return Ok(new { message = "ok" });
        }


        [HttpGet("user")]
        public IActionResult user()
        {
            try
            {
              

                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _repository.GetById(userId);
                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized(); 
            }

        }
        [AllowAnonymous]
        [HttpGet("listuser")]
        public IActionResult ListUsers()
        {
            try
            {


               // var jwt = Request.Cookies["jwt"];
               // var token = _jwtService.verify(jwt);
               // int userId = int.Parse(token.Issuer);
                var user = _repository.GetAll();           
                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }


        [HttpGet("logout")]
        public IActionResult logOut()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "Ok"
            });
        }

    }
}
