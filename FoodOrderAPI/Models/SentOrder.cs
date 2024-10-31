namespace FoodOrderAPI.Models
{
    public class SentOrder
    {
        public SentOrderModel SentOrderModel { get; set; } = new SentOrderModel();
        public List<OrderDetail> SentOrderDetail { get; set; } = new List<OrderDetail>();

    }
}
