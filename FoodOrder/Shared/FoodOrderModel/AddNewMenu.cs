using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class AddNewMenu
    {
        public GetMenu GetMenuModel { get; set; } = new GetMenu();
        public List<SentAddMenu> SentAddMenuModel { get; set; } = new List<SentAddMenu>();

    }
}
