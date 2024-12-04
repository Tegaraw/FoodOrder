namespace FoodOrderAPI.Models
{
    public class MasterItem
    {
        public string IdItem { get; set; }
        public string NamaItem { get; set; }
        public int Harga { get; set; }
        public string IdJenis { get; set; }
        public string QtyAvailable { get; set; }
        public string Tersedia { get; set; }
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
