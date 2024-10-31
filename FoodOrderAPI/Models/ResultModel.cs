namespace FoodOrderAPI.Models
{
    public class ResultModel<T>
    {
        public T Data { get; set; }
        public bool isSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
