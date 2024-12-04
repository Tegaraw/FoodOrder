namespace FoodOrderAPI.Models
{
    public class AddNewMenu
    {
        public GetMenu GetMenuModel { get; set; } = new GetMenu();
        public List<SentAddMenu> SentAddMenuModel { get; set; } = new List<SentAddMenu>();
        public List<DetailFileType> DetailFileType { get; set; } = new List<DetailFileType>();
    }
}
