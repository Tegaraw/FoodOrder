using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class AGetReportOrderDataModel
    {

        public List<GetReportOrderData> GetReportOrderData { get; set; } = new List<GetReportOrderData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
