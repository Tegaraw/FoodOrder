using Blazored.SessionStorage;
using FoodOrder.Client.Services;
using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FoodOrder.Client.Pages
{


        public partial class ReportDataOrder : ComponentBase
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
        private bool isLoading = false;

        public List<GetReportOrderData> FileResult { get; set; } = new List<GetReportOrderData>();
        private class fundReturnFilterValue
        {
            public string type { get; set; } = string.Empty;
            public string value { get; set; } = string.Empty;
        }


        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            token = sessionStorage.GetItem<string>("Token");
            fuelStockPageActive = 1;
            await LoadData(fuelStockPageActive, "");
            await GetCrossDockHeaderTotalRow("");
            isLoading = false;
        }

        private async Task LoadData(int PageNumber, string filter)
        {
            FileResult.Clear();

            QueryModel<AGetReportOrderDataModel> updateModel = new QueryModel<AGetReportOrderDataModel>();
            updateModel.Data = new AGetReportOrderDataModel();


            updateModel.Data.PageNumber = PageNumber;
            updateModel.Data.Condition = filter;

            var data = await FoodOrderService.GetReporOrderData(token, updateModel);

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
                QueryModel<AGetReportOrderDataModel> updateModel = new QueryModel<AGetReportOrderDataModel>();
                updateModel.Data = new AGetReportOrderDataModel();

                updateModel.Data.Condition = filter;

                var restRow = await FoodOrderService.GetReportOrderDataTotalRow(token, updateModel);
                fuelStockNumberofPages = restRow.Data;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            StateHasChanged();
        }
        private async Task resetFundReturnFilter()
        {
            try
            {


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
                    if (fil.type.Equals("NamaItem"))
                    {
                        compileCond += $"MI.NamaItem LIKE \'%{fil.value}%\' {advanceFilterOperandi} ";
                    }


                }

                compileCond = compileCond.Substring(0, compileCond.Length - (advanceFilterOperandi.Length + 2));

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
                        if (fil.type.Equals("NamaItem"))
                        {
                            compileCond += $"MI.NamaItem LIKE \'%{fil.value}%\' {advanceFilterOperandi} ";
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

    }
}
