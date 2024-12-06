using Blazored.SessionStorage;
using FoodOrder.Client.Services;
using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace FoodOrder.Client.Pages
{
    public partial class AddMenu : ComponentBase
    {
        [Inject]
        public iFoodOrderServices FoodOrderService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public ISyncSessionStorageService SessionStorage { get; set; }

        public string token;
        public string? alertMessage = "";
        private List<MasterItem> getItemData = new List<MasterItem>();

        private List<getCategory> getItemDataCategory = new List<getCategory>();
        private string selectedTab = ""; // Menyimpan tab yang dipilih

        private Dictionary<int, bool> isEditing = new Dictionary<int, bool>();
        private Dictionary<int, string> newTersediaValue = new Dictionary<int, string>();
        List<string> DataItemErr = new List<string>();
        List<InsertMasterItemModel> masterItemAddEntries = new List<InsertMasterItemModel>();
        private List<string> selectedItemNames = new List<string>();
        private string selectedharga = string.Empty;


        private bool isAddMasterItem = false;

        private bool IsOrder = false;
        private int jumlah = 1; 

        private List<OrderDetail> currentOrderDetails = new List<OrderDetail>();
        private bool ShowOrderList = false;
        private Dictionary<string, int> itemQuantities = new Dictionary<string, int>();
        private string _nomormeja;
        private List<string> selectedItemName = new List<string>();

        private Dictionary<string, string> itemImages = new Dictionary<string, string>();

        private bool isLoading = false;


        [Parameter]
        public string IdHeaderOrder { get; set; }


        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            token = sessionStorage.GetItem<string>("Token");
            GetDataDO(Base64Decode(IdHeaderOrder));

            await LoadData();
            await LoadDataCategory();

        
            if (getItemDataCategory.Any())
            {
                selectedTab = getItemDataCategory.First().IdJenis;
            }
            isLoading = false;
        }

        private void GetDataDO(string dataURL)
        {
            try
            {
                string[] tmpVal = dataURL.Split(";");

                IdHeaderOrder = tmpVal[0].ToString();
               

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
        internal string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                Console.WriteLine("Base64 string is null or empty.");
                return string.Empty; 
            }


            string cleanBase64String = base64EncodedData.Trim()
                                             .Replace(" ", "+");

          
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(cleanBase64String);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error decoding Base64 string: {ex.Message}");
                return null;
            }
        }

        public class InsertMasterItemModel
        {
            public string NamaItem { get; set; } = string.Empty;
            public string harga { get; set; } = string.Empty;
            public string jenis { get; set; } = string.Empty;
            public string qtytersedia { get; set; } = string.Empty;
            public string keterangan { get; set; } = string.Empty;
        }
        public string NomorMeja
        {
            get => _nomormeja;
            set => _nomormeja = value;
        }


        private async Task HandleInputNomorMeja(ChangeEventArgs e)
        {
            NomorMeja = e.Value.ToString();
        }


        private void AddNewMasterItemEntry()
        {
           
            masterItemAddEntries.Add(new InsertMasterItemModel());
            StateHasChanged(); 
        }

        private void RemoveLoadingManifestEntry(int index)
        {
            if (index >= 0 && index < masterItemAddEntries.Count)
            {
                masterItemAddEntries.RemoveAt(index);
                StateHasChanged(); 
            }
        }

        private void CloseAddMenuModal()
        {
           
            masterItemAddEntries.Clear();
            isAddMasterItem = false;
        }



        private async Task LoadData()
        {
            try
            {
              

                var resultceknopol = await FoodOrderService.GetMasterItem("0", token);

                if (resultceknopol.isSuccess && resultceknopol.Data != null)
                {
                    getItemData = resultceknopol.Data?
                        .Select(g => new MasterItem
                        {
                            IdItem = g.IdItem,
                            NamaItem = g.NamaItem,
                            Harga = g.Harga,
                            QtyAvailable = g.QtyAvailable,
                            Tersedia = g.Tersedia,
                            IdJenis = g.IdJenis
                        }).ToList() ?? new List<MasterItem>();

                    itemImages = new Dictionary<string, string>();

                    foreach (var file in resultceknopol.Data)
                    {
                        if (file.fileContent != null)
                        {
                            var base64Image = Convert.ToBase64String(file.fileContent);
                            itemImages[file.IdItem] = base64Image;
                        }
                    }

                    if (!getItemData.Any())
                    {
                        alertMessage = "Data Kosong";
                    }
                }
                else
                {
                    alertMessage = "Data Kosong";
                }
            }
            catch (Exception)
            {

                alertMessage = "Data Kosong";
            }
        }




        private async Task LoadDataCategory()
        {
            try
            {
              
                var resultceknopol = await FoodOrderService.GetCategory("0", token);

                if (resultceknopol.isSuccess && resultceknopol.Data != null)
                {
                    getItemDataCategory = resultceknopol.Data?
                        .Select(g => new getCategory
                        {
                            IdJenis = g.IdJenis,
                            NamaJenis = g.NamaJenis
                        }).ToList() ?? new List<getCategory>();

                    if (!getItemDataCategory.Any())
                    {
                        alertMessage = "Data Kosong";
                    }
                }
                else
                {
                    alertMessage = "Data Kosong";
                }
            }
            catch (Exception)
            {

                alertMessage = "Data Kosong";
            }
        }
        private void SelectTab(string idJenis)
        {
            selectedTab = idJenis;
        }

        private IEnumerable<MasterItem> GetItemsBySelectedTab()
        {
            return getItemData.Where(item => item.IdJenis == selectedTab);
        }


        private void OnTambahMenuClicked()
        {
            isAddMasterItem = true;

           
            if (masterItemAddEntries.Count == 0)
            {
                masterItemAddEntries.Add(new InsertMasterItemModel());
            }
        }


        public bool ShouldShowSaveProsesMuat => irestService.activeUsers.Role == "1";


        private void EditStatus(MasterItem item)
        {
            item.IsEditing = true;
        }

        private async Task SaveStatus(string idItem, string tersedia)
        {
            string msg = string.Empty;

            try
            {
                var sentcreatemenu = new GetMenu
                {
                    IdJenis = idItem,
                    Tersedia = tersedia
                };

                QueryModel<GetMenu> acongceknopol = new QueryModel<GetMenu>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.UpdateTersedia(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);


                    var item = getItemData.FirstOrDefault(i => i.IdItem == idItem);
                    if (item != null)
                    {
                        item.IsEditing = false;
                    }
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Data Gagal Dikirimkan");
                }
            }
            catch (Exception ex)
            {
                alertMessage = $"An error occurred: {ex.Message}";
            }
        }


        private void CancelEdit(MasterItem item)
        {
            item.IsEditing = false;
        }

        private async Task SaveMenu()
        {
            string msg = string.Empty;
            var createmenunewmodel = masterItemAddEntries
                .Select(container => new SentAddMenu
                {
                    NamaItem = container.NamaItem,
                    harga = container.harga,
                    jenis = container.jenis,
                    qtytersedia = container.qtytersedia

                }).ToList();

            try
            {
                var sentcreatemenu = new GetMenu
                {
                    IdJenis = "",
                    Tersedia = ""

                };

                var acong = new QueryModel<AddNewMenu>
                {
                    Data = new AddNewMenu
                    {
                        GetMenuModel = sentcreatemenu,
                        SentAddMenuModel = createmenunewmodel

                    }

                };

                var result = await FoodOrderService.SentCreateMenu(token, acong);
                msg = result.Data.message.ToString();

                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    await LoadData();
                    await LoadDataCategory();
                    isAddMasterItem = false;
                    masterItemAddEntries.Clear();
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Data Gagal Dikirimkan");
                }


            }
            catch (Exception ex)
            {
                alertMessage = $"An error occurred: {ex.Message}";
            }

        }
        private async Task PopUpOrder(string idItem, string itemName, int harga)
        {
            int qty = GetItemQuantity(idItem);


            var item = getItemData.FirstOrDefault(x => x.IdItem == idItem);
            if (item != null && Convert.ToInt32(item.QtyAvailable) >= qty)
            {

                item.QtyAvailable = (Convert.ToInt32(item.QtyAvailable) - qty).ToString();


                var existingDetail = currentOrderDetails.FirstOrDefault(d => d.IdItem == idItem);

                if (existingDetail != null)
                {
                    existingDetail.Qty = (int.Parse(existingDetail.Qty) + qty).ToString();
                    existingDetail.Total = (int.Parse(existingDetail.Qty) * harga).ToString();
                }
                else
                {
                    var newDetail = new OrderDetail
                    {
                        IdItem = idItem,
                        Qty = qty.ToString(),
                        Total = (qty * harga).ToString()
                    };

                    currentOrderDetails.Add(newDetail);
                }


                itemQuantities[idItem] = 1;


                await JSRuntime.InvokeVoidAsync("alert", "Item berhasil ditambahkan ke keranjang!");
                await InvokeAsync(StateHasChanged);
            }
            else
            {

                await JSRuntime.InvokeVoidAsync("alert", "Jumlah melebihi stok yang tersedia!");
            }
        }

        private void RemoveOrderDetail(OrderDetail detail)
        {

            var item = getItemData.FirstOrDefault(x => x.IdItem == detail.IdItem);

            if (item != null)
            {

                item.QtyAvailable = (Convert.ToInt32(item.QtyAvailable) + int.Parse(detail.Qty)).ToString();
            }


            currentOrderDetails.Remove(detail);


            StateHasChanged();
        }
        private int GetItemQuantity(string idItem)
        {
            if (!itemQuantities.ContainsKey(idItem))
            {
                itemQuantities[idItem] = 1; 
            }
            return itemQuantities[idItem];
        }

        private void IncrementJumlah(string idItem)
        {
            if (itemQuantities.ContainsKey(idItem))
            {
                itemQuantities[idItem]++;
            }
            else
            {
                itemQuantities[idItem] = 1;
            }
        }

      
        private void DecrementJumlah(string idItem)
        {
            if (itemQuantities.ContainsKey(idItem) && itemQuantities[idItem] > 1)
            {
                itemQuantities[idItem]--;
            }
        }

        private string GetTotalKeseluruhan()
        {
            return currentOrderDetails.Sum(detail => int.Parse(detail.Total)).ToString();
        }
        private async Task InsertOrder()
        {

            isLoading = true;

            string msg = string.Empty;


            try
            {
                var sentcreatemenu = new SentOrderModel
                {

                    NomorMeja = "",
                    IdHeaderOrder = IdHeaderOrder,
                    PaidBy = "",
                    CreateBy = irestService.activeUsers.Username


                };

                var acong = new QueryModel<SentOrder>
                {
                    Data = new SentOrder
                    {
                        SentOrderModel = sentcreatemenu,
                        SentOrderDetail = currentOrderDetails

                    },


                };

                var result = await FoodOrderService.SentAddOrderNew(token, acong);
                msg = result.Data.message.ToString();

                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    await LoadData();
                    await LoadDataCategory();
                    ShowOrderList = false;
                    currentOrderDetails.Clear();
                    isLoading = false;
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Data Gagal Dikirimkan");
                    isLoading = false;
                }
            }
            catch (Exception ex)
            {
                alertMessage = $"An error occurred: {ex.Message}";
            }
        }


    }
}
