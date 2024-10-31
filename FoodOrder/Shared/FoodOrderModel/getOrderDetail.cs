using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
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
        public bool IsEditing { get; set; } = false;

    }
}
