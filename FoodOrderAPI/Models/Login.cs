namespace FoodOrderAPI.Models
{
    public class RequestLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class ResultLogin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
