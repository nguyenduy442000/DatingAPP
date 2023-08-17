namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalcuateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age= today.Year - dob.Year;//Bạn tính tuổi bằng cách lấy năm của ngày hiện tại trừ đi năm của ngày sinh (dob là ngày sinh). Điều này trả về tuổi dự kiến, tuy nhiên nó có thể chưa chính xác vì chưa xem xét tháng và ngày sinh.
            if(dob>today.AddYears(-age)) age--;//Đoạn này kiểm tra nếu ngày sinh trong năm hiện tại chưa kết thúc, thì tuổi sẽ giảm đi 1, vì người đó chưa đủ tuổi. 
            
            return age;
        }
    }
}