namespace FoodOrder.Shared.FoodOrderModel
{
    public class Order
    {
        public int IdHeaderOrder { get; set; }
        public string NomorOrder { get; set; }
        public int NomorMeja { get; set; }
        public string PaidBy { get; set; }
        public DateTime? PaidDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
