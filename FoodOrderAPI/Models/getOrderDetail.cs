namespace FoodOrderAPI.Models
{
    public class getOrderDetail
    {
        public string IdItem { get; set; }
        public int Qty { get; set; }
        public int HargaSatuan { get; set; }
        public string IdHeaderOrder { get; set; }
        public string IdDetailOrder { get; set; }
        public string Total { get; set; }
        public string NamaItem { get; set; }

    }
}
