using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
    
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
          
            _context = context;
            
        }
        [HttpPost("register")] // POST : api/account/register?username=dave&password=pwd
        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto){

            if(await UserExists(registerDto.Username)) {
                return BadRequest("Username is taken");
            }


            using var hmac = new HMACSHA512(); // Hàm mã hóa mật khẩu .Net 
            var user = new AppUser{
                UserName= registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user); // thêm người dùng mới bằng method Add
            await _context.SaveChangesAsync(); //đợi sau khi lưu user mới thành công mới return về user
            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

                
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto){
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == loginDto.Username); // check username

            if(user == null) return Unauthorized("invalid username"); // không tìm thấy username

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // mã hóa password của người dùng nhập vào để so sánh với password đã được mã hóa trong csdl
            
            //kiểm tra password bằng cách kiểm tra từng phần tử trong mảng có đúng không
            for(int i =0 ; i<computerHash.Length; i++){
                if(computerHash[i]!= user.PasswordHash[i]){
                    return Unauthorized("invalid password");
                }
            }
            return  new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };



        }

        //kiểm tra username có tồn tại hay không
          private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
        
        }

      
}