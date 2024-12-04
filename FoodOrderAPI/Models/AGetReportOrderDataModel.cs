namespace FoodOrderAPI.Models
{
    public class AGetReportOrderDataModel
    {

        public List<GetReportOrderData> GetReportOrderData { get; set; } = new List<GetReportOrderData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
