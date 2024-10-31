using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class QueryModel<T>
    {
        public T? Data { get; set; }
        public string username { get; set; } = string.Empty;
        public string userAction { get; set; } = string.Empty;
       
        public DateTime userActionDate { get; set; } = DateTime.Now;
    }
}
