using Blazored.SessionStorage;
using FoodOrder.Client.Services;
using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodOrder.Client.Pages
{
    public partial class Menu : ComponentBase
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
        private bool isEditMasterItem = false;

        private bool isDeleteMasterItem = false;

        private bool IsOrder = false;
        private int jumlah = 1; // Inisialisasi default awal

        private List<OrderDetail> currentOrderDetails = new List<OrderDetail>();
        private bool ShowOrderList = false;
        private Dictionary<string, int> itemQuantities = new Dictionary<string, int>();
        private string _nomormeja;

       

        List<DetailFileType> DetailFileType = new List<DetailFileType>();
        private Guid inputFileId = Guid.NewGuid();
        private List<string> imageBase64List = new();
        private Dictionary<string, string> itemImages = new Dictionary<string, string>();
        private string selectedNamaItem = string.Empty;
        private string selectedHarga = string.Empty;
        private string selectedIdItem = string.Empty;

        private string QTYReady;
        private bool isLoading = false;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true; 

            token = sessionStorage.GetItem<string>("Token");
            await LoadData();
            await LoadDataCategory();

            if (getItemDataCategory.Any())
            {
                selectedTab = getItemDataCategory.First().IdJenis;
            }

            isLoading = false; 
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
                var CekNopol = new GetMenu
                {
                    IdJenis = "",
                    Tersedia = ""
                };

                QueryModel<GetMenu> acongceknopol = new QueryModel<GetMenu>
                {
                    Data = CekNopol,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.GetMasterItem(acongceknopol, token);

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
                var CekNopol = new GetMenu
                {
                    IdJenis = "",
                    Tersedia = ""
                };

                QueryModel<GetMenu> acongceknopol = new QueryModel<GetMenu>
                {
                    Data = CekNopol,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.GetCategory(acongceknopol, token);

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
                        SentAddMenuModel = createmenunewmodel,
                        DetailFileType = DetailFileType

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
            isLoading = true;
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

                isLoading = false;
                await JSRuntime.InvokeVoidAsync("alert", "Item berhasil ditambahkan ke keranjang!");
                await InvokeAsync(StateHasChanged);

            }
            else
            {
               
                await JSRuntime.InvokeVoidAsync("alert", "Jumlah melebihi stok yang tersedia!");
                isLoading = false;
            }
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

            if (string.IsNullOrWhiteSpace(NomorMeja))
            {
               
                await JSRuntime.InvokeVoidAsync("alert", "Nomor Meja harus diisi.");
                return;

            }

            string msg = string.Empty;


            try
            {
                var sentcreatemenu = new SentOrderModel
                {

                    NomorMeja = NomorMeja,
                    IdHeaderOrder = "",
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

                var result = await FoodOrderService.SentOrderNew(token, acong);
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
                isLoading = false;
            }
        }
        private async Task OnInputFileChange(InputFileChangeEventArgs e, int Id)
        {
            try
            {
                if (e.File.Size <= 5242880)
                {
                    var newData = DetailFileType.Where(x => x.Id == Id).FirstOrDefault();

                    if (newData != null)
                    {
                        using var datafile = e.File.OpenReadStream(5242880);
                        var fileBytes = new byte[datafile.Length];
                        await datafile.ReadAsync(fileBytes, 0, (int)datafile.Length);

                        newData.fileContent = fileBytes;
                        newData.FileName = e.File.Name;
                        newData.Size = Convert.ToInt32(e.File.Size);
                    }
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
            }
            StateHasChanged();
        }
        private void deleteTask(DetailFileType data)
        {
            DetailFileType.Remove(data);
        }


        private void addLine()
        {
            DetailFileType newData = new DetailFileType();
            var lastId = DetailFileType.OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastId == null)
            {
                newData.Id = 1;
            }
            else
            {
                newData.Id = lastId.Id + 1;
            }

            DetailFileType.Add(newData);
        }


        private void EditMenu(MasterItem item)
        {
           
            selectedIdItem = item.IdItem;
            selectedNamaItem = item.NamaItem;
            selectedHarga = item.Harga.ToString();

           
            isEditMasterItem = true;
        }


        private void CloseEditMenuModal()
        {
           
            isEditMasterItem = false;
            selectedNamaItem = string.Empty;
            selectedHarga = string.Empty;
            selectedIdItem = string.Empty;
        }


        private async Task UpdateMasterItem()
        {
            isLoading = true;
            string msg = string.Empty;

            try
            {
                var sentcreatemenu = new SentUpdateItemModel
                {
                    IdItem = selectedIdItem,
                    NamaItem = selectedNamaItem,
                    Harga = selectedHarga,
                    QTY = QTYReady
                };

                QueryModel<SentUpdateItemModel> acongceknopol = new QueryModel<SentUpdateItemModel>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.UpdateItem(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    await LoadData();
                    CloseEditMenuModal();
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
                isLoading = false;
            }
        }



        private void DeleteMenu(MasterItem item)
        {

            selectedIdItem = item.IdItem;
            selectedNamaItem = item.NamaItem;
            selectedHarga = item.Harga.ToString();


            isDeleteMasterItem = true;
        }




        private async Task DeleteMasterItem()
        {
            isLoading = true;
            string msg = string.Empty;

            try
            {
                var sentcreatemenu = new SentDeleteItemModel
                {
                    IdItem = selectedIdItem
                  
                };

                QueryModel<SentDeleteItemModel> acongceknopol = new QueryModel<SentDeleteItemModel>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.DeleteItemMaster(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    await LoadData();
                    CloseDeleteMenuModal();
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
                isLoading = false;
            }
        }


        private void CloseDeleteMenuModal()
        {

            isDeleteMasterItem = false;
            selectedNamaItem = string.Empty;
            selectedHarga = string.Empty;
            selectedIdItem = string.Empty;
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

    }
}
