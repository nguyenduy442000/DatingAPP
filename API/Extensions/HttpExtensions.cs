using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
      public static void AddPaginationHeader(this HttpResponse response,  PaginationHeader header)
      {
     
     
       var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};   
        //cấu hình quy tắc đặt tên các thuộc tính JSON.  
       
       
       
       response.Headers.Add("Pagination",JsonSerializer.Serialize(header, jsonOptions));
       /*
      - thêm một header có tên là "Pagination" vào HTTP response. 
       Đối số thứ hai là kết quả của việc chuyển đổi đối tượng header (được truyền vào phương thức)
          thành một chuỗi JSON bằng cách sử dụng JsonSerializer.
       Serialize. Các quy tắc đặt tên thuộc tính JSON đã được cấu hình trong jsonOptions.
       */ 
      
      
       response.Headers.Add("Access-Control-Expose-Headers","Pagination");  
        /*
        Dòng này thêm một header có tên là "Access-Control-Expose-Headers" vào HTTP response. 
        Header này thường được sử dụng trong CORS (Cross-Origin Resource Sharing)
         để chỉ ra các header mà trình duyệt cho phép JavaScript truy cập khi thực hiện các yêu cầu từ nguồn gốc khác. 
        */

      }
        
    }
}