using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_ecommer.Data;
using WebAPI_ecommer.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_ecommer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
     {
        private readonly myDBContext _context;
        private readonly AppSetting _appSettings;

        public AuthController(myDBContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        
        public async Task <IActionResult> Validate([FromQuery] Login model)
        {
            var user = _context.Users.SingleOrDefault(p=> p.Username==model.Username && model.Password ==p.Password);
            if (user == null)// khong dung
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"

                });
            }
            //Cap token
            var token = await GenerateToken(user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });

        }
        private async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName",user.Username ),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Role", user.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)


            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Luu database
            var refreshTokenEntiy = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                User_Id= user.Id,
                Jwt_Id = token.Id,
                Token = refreshToken,
                Is_Used = false,
                Is_Revoked = false,
                Isused_At = DateTime.UtcNow,
                Expried = DateTime.UtcNow.AddHours(1)
            };

            await _context.AddAsync(refreshTokenEntiy);
            await _context.SaveChangesAsync();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken()
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create() )
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateparam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                // Ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes), // Khoa bi mat de giai ma token

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //Khong kiem tra token het han
            };
            try
            {
                //check 1: AccessToken vilid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateparam, out var validatedToken);

                //check 2: Check alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if(!result) //false
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }
                //check 3: Check accessToken exprire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => 
                x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if(expireDate >DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB 
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token ==
                model.RefreshToken);
                if(storedToken==null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                // check 5: check refreshToken is used/revoked
                if(storedToken.Is_Used)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }

                if (storedToken.Is_Revoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: Id accessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.Jwt_Id != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "token doesn't match"
                    });
                }

                //Update token is used
                storedToken.Is_Revoked = true;
                storedToken.Is_Used = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.Users.SingleOrDefaultAsync(us => us.Id == storedToken.User_Id);
                var token = await GenerateToken(user);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success"


                });
            }
            catch(Exception)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
           var dateTimeInterval = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
           dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

           return dateTimeInterval;
        }
    }
}
