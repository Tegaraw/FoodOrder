namespace FoodOrderAPI.Models
{
    public class AGetOrderModel
    {
        public GetOrderDataModels GetHead { get; set; } = new GetOrderDataModels();
        public List<GetOrderData> GetOrderDataModel { get; set; } = new List<GetOrderData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
