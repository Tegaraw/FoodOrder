using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{

    public class SentOrder
    {
        public SentOrderModel SentOrderModel { get; set; } = new SentOrderModel();
        public List<OrderDetail> SentOrderDetail { get; set; } = new List<OrderDetail>();

    }
}
