using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Shared.FoodOrderModel
{
    public class DetailFileType
    {
        public int Id { get; set; } = 0;

        public string FileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public int Size { get; set; } = 0;
        public byte[]? fileContent { get; set; } = new byte[0];
    }
}
