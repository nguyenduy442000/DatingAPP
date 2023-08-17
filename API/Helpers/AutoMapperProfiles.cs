using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            // Configurting auto Mapper chọn 1 hình là hình đại diện
            CreateMap<AppUser , MemberDto>()
            .ForMember(dest => dest.PhotoUrl , opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age , opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
/*
Cách hoạt động của auto Mapper Profile là :

-automapper trong . net hoạt động là đối chiếu giữa đối tượng trong DTO với đối tượng dữ liệu trong database ,
và nếu thuộc tính nào giống nó sẽ đẩy dữ liệu từ database ra DTO của .net 

-AutoMapper trong .NET là một thư viện giúp tự động ánh xạ dữ liệu giữa các đối tượng, 
thường được sử dụng để ánh xạ giữa các đối tượng DTO (Data Transfer Objects) và các đối tượng dữ liệu trong cơ sở dữ liệu.

-Cách hoạt động của AutoMapper tương tự như cách bạn đã mô tả:

+Cấu hình ánh xạ: Trước tiên, bạn cần cấu hình AutoMapper để nó biết cách ánh xạ giữa các loại đối tượng.
 Bạn chỉ định các quy tắc ánh xạ bằng cách định nghĩa các profile hoặc map cụ thể trong phần cấu hình.

+Tự động ánh xạ: Khi bạn gọi phương thức ánh xạ, AutoMapper sẽ tự động so khớp các thuộc tính có tên giống nhau 
hoặc được cấu hình ánh xạ với nhau giữa các đối tượng và sao chép dữ liệu từ một đối tượng sang đối tượng khác.



*/