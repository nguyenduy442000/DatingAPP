using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
              /*
        Ứng dụng xác thực: Khi người dùng đăng nhập vào một ứng dụng, 
        hệ thống xác thực sẽ tạo ra danh sách các claims dựa trên thông tin mà họ đã cung cấp. 
        Điều này có thể bao gồm tên, email, vai trò, quyền hạn và các thông tin tùy chỉnh khác.

        JWT (JSON Web Tokens): Trong các ứng dụng sử dụng JWT để quản lý xác thực và ủy quyền, 
        thông tin claims được mã hóa vào token. Khi người dùng gửi yêu cầu,
         ứng dụng có thể giải mã token để trích xuất thông tin claims.
        */
        }

         public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        }

      
    }
}