using companyappbasic.Data.Entity;
using companyappbasic.Data.Models;
using companyappbasic.Services.AppUserServices;
using companyappbasic.Services.BackgroundServices;
using companyappbasic.Services.RabbitMQServices;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace companyappbasic.Controller
{
    [Route("Api/Account")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IToken _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly BackgroundServi _backgroundServi;
        private readonly RabbitProducer _rabbitProducer;



        public AccountController(UserManager<AppUser> userManager, IToken tokenServices, SignInManager<AppUser> signInManager, BackgroundServi backgroundServi, RabbitProducer rabbitProducer)

        {
            _userManager = userManager;
            _tokenService = tokenServices;
            _signInManager = signInManager;
            _backgroundServi = backgroundServi;
            _rabbitProducer = rabbitProducer;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName!.ToLower());

            if (user == null)
            {
                return Unauthorized("Geçersiz kullanıcıadı !");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Kullanıcı adı bulunamadı veya şifre yanlış");
            }

            var token = _tokenService.CreateToken(user);
            await _backgroundServi.AddLoginLogAsync(user.UserName!, user.Email!, user.Id);

            string emailBody = $"{user.UserName} başarılı giriş yaptı!";
            await _rabbitProducer.SendMessageAsync(user.Email!, "Başarılı Giriş", emailBody);

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token
                }

            );
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, registerDto.Role!);
                    if (roleResult.Succeeded)
                    {
                        // Token oluşturuluyor
                        var token = _tokenService.CreateToken(appUser);

                        string emailBody = $"Hoşgeldiniz {appUser.UserName}!";
                        await _rabbitProducer.SendMessageAsync(appUser.Email!, "Hoşgeldiniz", emailBody);

                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = token

                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }


        }


    }
}

