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
        public int Id { get; set; } = 0;
        public int IdLogBook { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public long Size { get; set; } = 0;
        public byte[] fileContent { get; set; } = new byte[0];
    }
}
