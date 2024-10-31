namespace FoodOrder.Shared.FoodOrderModel
{
    public class MasterItem
    {
        public string IdItem { get; set; }
        public string NamaItem { get; set; }
        public int  Harga { get; set; }
        public string IdJenis { get; set; }
        public string QtyAvailable { get; set; }
        public string Tersedia { get; set; }

        public bool IsEditing { get; set; } = false;

        public string Jenis { get; set; }
    }
}
