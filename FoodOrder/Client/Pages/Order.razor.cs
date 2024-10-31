using Blazored.SessionStorage;
using FoodOrder.Client.Services;
using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace FoodOrder.Client.Pages
{

    public partial class Order : ComponentBase
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
        private bool isAdvancedFilterActive { get; set; } = false;

        private int fuelStockPageActive = 0;
        private int? fuelStockNumberofPages = 0;

        private string advanceFilterOperandi = "AND";
        private int fundReturnPageActive = 0;
        private List<fundReturnFilterValue> advanceFilter = new();
        IJSObjectReference _jsModule;

        public List<GetOrderData> FileResult { get; set; } = new List<GetOrderData>();
        private List<getOrderDetail> getItemData = new List<getOrderDetail>();
        private bool ShowOrderList = false;
        List<string> DataItemErr = new List<string>();

        [Parameter]
        public string IdHeaderOrderMenu { get; set; }

        protected override async Task OnInitializedAsync()
        {
            token = sessionStorage.GetItem<string>("Token");
            fuelStockPageActive = 1;
            await LoadData(fuelStockPageActive, "");
            await GetCrossDockHeaderTotalRow("");
        }


        private async Task LoadData(int PageNumber, string filter)
        {
            FileResult.Clear();

            QueryModel<AGetOrderModel> updateModel = new QueryModel<AGetOrderModel>();
            updateModel.Data = new AGetOrderModel();
            updateModel.Data.GetHead.UserName = irestService.activeUsers.Username;
            updateModel.Data.GetHead.Role = irestService.activeUsers.Role;

            updateModel.Data.PageNumber = PageNumber;
            updateModel.Data.Condition = filter;

            var data = await FoodOrderService.GetHeaderOrder(token, updateModel);

            if (data.Data != null)
            {

                FileResult = data.Data;



            }
            StateHasChanged();
        }





        private async Task GetCrossDockHeaderTotalRow(string filter)
        {
            try
            {
                QueryModel<AGetOrderModel> updateModel = new QueryModel<AGetOrderModel>();
                updateModel.Data = new AGetOrderModel();
                updateModel.Data.GetHead.UserName = irestService.activeUsers.Username;
                updateModel.Data.GetHead.Role = irestService.activeUsers.Role;

                updateModel.Data.Condition = filter;

                var restRow = await FoodOrderService.GetDataOrderTotalRow(token, updateModel);
                fuelStockNumberofPages = restRow.Data;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            StateHasChanged();
        }

        private class fundReturnFilterValue
        {
            public string type { get; set; } = string.Empty;
            public string value { get; set; } = string.Empty;
        }



        private async Task resetFundReturnFilter()
        {
            try
            {

                //fundReturnFilterActive = false;
                isAdvancedFilterActive = false;
                fundReturnPageActive = 1;
                QueryModel<string> param = new();

                await GetCrossDockHeaderTotalRow("");
                await LoadData(1, "");


                advanceFilter.Clear();
                advanceFilter.Add(new());
                StateHasChanged();
            }
            catch (Exception ex)
            {

                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task GetPage(int page)
        {

            fundReturnPageActive = page;


            string getFundReturnDocumentParam = string.Empty;

            if (isAdvancedFilterActive)
            {
                string compileCond = "WHERE ";

                foreach (var fil in advanceFilter)
                {
                    if (fil.type.Equals("NomorOrder"))
                    {
                        compileCond += $"HO.NomorOrder LIKE \'%{fil.value}%\' {advanceFilterOperandi} ";
                    }


                }

                compileCond = compileCond.Substring(0, compileCond.Length - (advanceFilterOperandi.Length + 2));
                //compileCond = $"{activeUser.location}!_!" + compileCond;
                fuelStockPageActive = page;
                await LoadData(fuelStockPageActive, compileCond);

            }
            else
            {
                fuelStockPageActive = page;
                await LoadData(fuelStockPageActive, "");
            }


            StateHasChanged();
        }


        private bool validateInputFilter(string paramValue)
        {
            //if (paramValue.Any(x => !char.IsLetterOrDigit(x)))
            //    return false;

            if (paramValue.Length < 1)
                return false;

            return true;
        }
        private async Task fundReturnAdvanceFilter()
        {
            try
            {
                if (!advanceFilter.Any(x => x.type.Length < 1))
                {
                    if (advanceFilter.Any(x => !validateInputFilter(x.value)))
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Input Value is not Valid !");
                        return;
                    }

                    QueryModel<string> param = new();
                    //fundReturnFilterActive = true;
                    isAdvancedFilterActive = true;

                    //FundReturnService.fundReturns = new();
                    //FundReturnService.fundReturns.Clear();
                    fundReturnPageActive = 1;

                    string compileCond = "WHERE ";

                    foreach (var fil in advanceFilter)
                    {
                        if (fil.type.Equals("NomorOrder"))
                        {
                            compileCond += $"HO.NomorOrder LIKE \'%{fil.value}%\' {advanceFilterOperandi} ";
                        }


                    }


                    compileCond = compileCond.Substring(0, compileCond.Length - (advanceFilterOperandi.Length + 2));
                    //compileCond = $"{activeUser.location}!_!" + compileCond;
                    fuelStockPageActive = 1;
                    await LoadData(fuelStockPageActive, compileCond);

                    await GetCrossDockHeaderTotalRow(compileCond);


                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
                }
            }
            catch (Exception ex)
            {

                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }
        


            private async Task GetDetailOrder(string IdHeaderOrder)
        {
            try
            {

                var CekNopol = new GetDetailOrder
                {
                    IdHeaderOrder = IdHeaderOrder

                };

                QueryModel<GetDetailOrder> acongceknopol = new QueryModel<GetDetailOrder>
                {
                    Data = CekNopol,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.GetDetailOrder(acongceknopol, token);
                if (resultceknopol.isSuccess)
                {
                    getItemData = resultceknopol.Data
                        .Select(g => new getOrderDetail
                        {
                            IdItem = g.IdItem,
                            NamaItem = g.NamaItem,
                            IdHeaderOrder = g.IdHeaderOrder,
                            Total = g.Total,
                            Qty = g.Qty,
                            HargaSatuan = g.HargaSatuan,
                            IdDetailOrder = g.IdDetailOrder
                            
                        }).ToList();


                    if (getItemData.Any())
                    {
                        IdHeaderOrderMenu = getItemData.First().IdHeaderOrder;



                    }


                    ShowOrderList = true;
                    if (getItemData == null || !getItemData.Any())
                    {
                        alertMessage = "Data tidak ditemukan.";
                    }
                }
            }
            catch (Exception ex)
            {
                alertMessage = $"An error occurred: {ex.Message}";
            }
        }

       

        private string GetTotalKeseluruhan()
        {
            return getItemData.Sum(detail => int.Parse(detail.Total)).ToString();
        }


        private void ToggleEditMode(getOrderDetail detail)
        {
            detail.IsEditing = !detail.IsEditing;
        }

        private void IncrementJumlah(getOrderDetail detail)
        {
            detail.Qty++;
            UpdateTotal(detail);
        }

        private void DecrementJumlah(getOrderDetail detail)
        {
            if (detail.Qty > 1)
            {
                detail.Qty--;
                UpdateTotal(detail);
            }
        }

        private void UpdateTotal(getOrderDetail detail)
        {
            detail.Total = (detail.Qty * detail.HargaSatuan).ToString();
        }

      
            private async Task SaveQuantity(getOrderDetail detail)
            {

                string msg = string.Empty;

            try
            {
                var sentcreatemenu = new updateOrderDetail
                {
                    IdItem = detail.IdItem,
                    Qty = detail.Qty,
                    Total = detail.Total
                };

                QueryModel<updateOrderDetail> acongceknopol = new QueryModel<updateOrderDetail>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.UpdateOrder(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    detail.IsEditing = false;
                    
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
    


        private void NavigateToMenu(){

            string tempurl = Base64Encode(IdHeaderOrderMenu);

           

            NavigationManager.NavigateTo($"/AddMenu/{tempurl}");
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }


        public bool ShouldShowSaveProsesMuat => irestService.activeUsers.Role == "1";


        private async Task SavePembayaran()
        {
            string msg = string.Empty;
            try
            {

                var sentcreatemenu = new updatePembayaran
                {
                    IdHeaderOrder = IdHeaderOrderMenu

                };

                QueryModel<updatePembayaran> acongceknopol = new QueryModel<updatePembayaran>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.UpdatePembayaran(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    ShowOrderList = false;
                    await OnInitializedAsync();

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
        private async Task RemoveOrderDetail(getOrderDetail detail)
        {

           
          

            string msg = string.Empty;
            try
            {
                var sentcreatemenu = new DeleteItem
                {
                    IdHeaderOrder = detail.IdHeaderOrder,
                    IdDetailOrder = detail.IdDetailOrder

                };

                QueryModel<DeleteItem> acongceknopol = new QueryModel<DeleteItem>
                {
                    Data = sentcreatemenu,
                    username = irestService.activeUsers.Username.ToString()
                };

                var resultceknopol = await FoodOrderService.DeleteItem(token, acongceknopol);
                msg = resultceknopol.Data.message.ToString();


                if (DataItemErr.Count == 0)
                {
                    await JSRuntime.InvokeVoidAsync("alert", msg);
                    await OnInitializedAsync();

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
    }
}
