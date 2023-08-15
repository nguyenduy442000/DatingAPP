using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IHostEnvironment env){
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }
            catch(Exception ex){
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment() 
                ? new ApiExeception(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiExeception(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response , options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
// Vì phản hồi từ server chỉ hiển thị ở môi trường Development (ví dụ như ở postman là phản hồi chữ màu đen) 
// Để chuyển đổi phản hồi đó thành 1 message về client thì sử dụng code này 