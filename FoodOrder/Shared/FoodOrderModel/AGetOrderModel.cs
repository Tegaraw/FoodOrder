using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class AGetOrderModel
    {
        public GetOrderDataModels GetHead { get; set; } = new GetOrderDataModels();
        public List<GetOrderData> GetOrderDataModel { get; set; } = new List<GetOrderData>();
        public int PageNumber { get; set; } = 0;
        public string Condition { get; set; } = string.Empty;

    }
}
