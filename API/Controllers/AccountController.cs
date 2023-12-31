using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
    
        public AccountController(DataContext context, ITokenService tokenService ,IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
            
        }
        [HttpPost("register")] // POST : api/account/register?username=dave&password=pwd
        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto){
            var user = _mapper.Map<AppUser>(registerDto);
            if(await UserExists(registerDto.Username)) {
                return BadRequest("Username is taken");
            }


            using var hmac = new HMACSHA512(); // Hàm mã hóa mật khẩu .Net 
           
            user.UserName= registerDto.Username.ToLower(); // user name là chữ thường
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)); 
            user.PasswordSalt = hmac.Key;
          
            _context.Users.Add(user); // thêm người dùng mới bằng method Add
            await _context.SaveChangesAsync(); //đợi sau khi lưu user mới thành công mới return về user
            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

                
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto){
            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x=>x.UserName == loginDto.Username); // check username

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
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain).Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender

            };



        }

        //kiểm tra username có tồn tại hay không
          private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
        
        }

      
}