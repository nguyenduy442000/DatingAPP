using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public IUserRepository _userRepository { get; set; }
        private readonly IMapper _mapper;
        
        public UsersController(IUserRepository userRepository ,IMapper mapper)
        { 
            _mapper = mapper;
            _userRepository = userRepository;
            

        }
   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
            
        }
      
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRepository.GetMemberAsync(username);
       
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            /*
            Ứng dụng xác thực: Khi người dùng đăng nhập vào một ứng dụng, 
            hệ thống xác thực sẽ tạo ra danh sách các claims dựa trên thông tin mà họ đã cung cấp. 
            Điều này có thể bao gồm tên, email, vai trò, quyền hạn và các thông tin tùy chỉnh khác.

            JWT (JSON Web Tokens): Trong các ứng dụng sử dụng JWT để quản lý xác thực và ủy quyền, 
            thông tin claims được mã hóa vào token. Khi người dùng gửi yêu cầu,
             ứng dụng có thể giải mã token để trích xuất thông tin claims.
            */



            var user = await _userRepository.GetUserByUsernameAsync(username);
            if(user == null) return NotFound();
            _mapper.Map(memberUpdateDto , user); // dùng autoMapper để thay đổi dữ liệu trong database

            if(await _userRepository.SaveAllAsync()) return NoContent(); // lưu dữ liệu sau khi dùng autoMapper
            return BadRequest("Failed to update user");
        }
        
 
    }
}