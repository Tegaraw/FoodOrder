using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class AGetReportStockDataModel
    {

        public List<GetReportStockData> GetReportStockDataModel { get; set; } = new List<GetReportStockData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
