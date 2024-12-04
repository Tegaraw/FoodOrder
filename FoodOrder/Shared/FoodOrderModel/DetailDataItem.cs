using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class DetailDataItem
    {
        public SentAddMenu SentAddMenu { get; set; } = new SentAddMenu();
        public List<DetailFileType> DetailFileType { get; set; } = new List<DetailFileType>();

    }
}
