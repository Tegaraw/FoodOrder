namespace FoodOrderAPI.Models
{
    public class AGetReportStockDataModel
    {

        public List<GetReportStockData> GetReportStockDataModel { get; set; } = new List<GetReportStockData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
