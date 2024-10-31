namespace FoodOrderAPI.Models
{
    public class QueryModel<T>
    {
        public T? Data { get; set; }
        public string username { get; set; } = string.Empty;
        public string userAction { get; set; } = string.Empty;
       
        public DateTime userActionDate { get; set; } = DateTime.Now;
    }
}
