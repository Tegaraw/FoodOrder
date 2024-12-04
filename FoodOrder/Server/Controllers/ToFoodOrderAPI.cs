using FoodOrder.Shared.FoodOrderModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace FoodOrder.Server.Controllers
{
    [Route("api/ToRestapi")]
    [ApiController]

    public class ToFoodOrderAPI : Controller
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public ToFoodOrderAPI(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseURL"));
        }

        [HttpPost("LoginUsers")]
        public async Task<IActionResult> LoginUsers(RequestLogin data)
        {
            IActionResult actionResult = null;
            ResultModel<ResultLogin> res = new ResultModel<ResultLogin>();
            try
            {
                string jsons = JsonConvert.SerializeObject(data);
                var resault = await _http.PostAsJsonAsync<RequestLogin>("api/RestAPI/LoginUsers", data);
                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ResultLogin>>();
                //var resultget = await _http.GetFromJsonAsync<string>($"api/POSda/test");

                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToPOSapi" + ex.Message.ToString();
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("GetMasterItem")]
        public async Task<IActionResult> GetMasterItem([FromHeader(Name = "Authorization")] string token, QueryModel<GetMenu> data)
        {
            ResultModel<List<MasterItem>> res = new ResultModel<List<MasterItem>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/RestAPI/GetMasterItem", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<MasterItem>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("GetCategory")]
        public async Task<IActionResult> GetCategory([FromHeader(Name = "Authorization")] string token, QueryModel<GetMenu> data)
        {
            ResultModel<List<getCategory>> res = new ResultModel<List<getCategory>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/RestAPI/GetCategory", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<getCategory>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }
        [HttpPost("UpdateTersedia")]
        public async Task<IActionResult> UpdateTersedia([FromHeader(Name = "Authorization")] string token, QueryModel<GetMenu> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<GetMenu>>($"api/RestAPI/UpdateTersedia", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("SentCreateMenu")]
        public async Task<IActionResult> SentCreateMenu([FromHeader(Name = "Authorization")] string token, QueryModel<AddNewMenu> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<AddNewMenu>>($"api/RestAPI/SentCreateMenu", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }



        [HttpPost("SentOrderNew")]
        public async Task<IActionResult> SentOrderNew([FromHeader(Name = "Authorization")] string token, QueryModel<SentOrder> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentOrder>>($"api/RestAPI/SentOrderNew", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("GetHeaderOrder")]
        public async Task<IActionResult> GetHeaderOrder([FromHeader(Name = "Authorization")] string token, QueryModel<AGetOrderModel> data)
        {
            ResultModel<List<GetOrderData>> res = new ResultModel<List<GetOrderData>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetOrderModel>>($"api/RestAPI/GetHeaderOrder", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<GetOrderData>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }
        [HttpPost("GetDataOrderTotalRow")]
        public async Task<IActionResult> GetSummarySupplierTotalRow([FromHeader(Name = "Authorization")] string token, QueryModel<AGetOrderModel> data)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetOrderModel>>($"api/RestAPI/GetDataOrder", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int?>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("GetDetailOrder")]
        public async Task<IActionResult> GetDetailOrder([FromHeader(Name = "Authorization")] string token, QueryModel<GetDetailOrder> data)
        {
            ResultModel<List<getOrderDetail>> res = new ResultModel<List<getOrderDetail>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<GetDetailOrder>>($"api/RestAPI/GetDetailOrder", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<getOrderDetail>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("SentAddOrderNew")]
        public async Task<IActionResult> SentAddOrderNew([FromHeader(Name = "Authorization")] string token, QueryModel<SentOrder> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentOrder>>($"api/RestAPI/SentAddOrderNew", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromHeader(Name = "Authorization")] string token, QueryModel<updateOrderDetail> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<updateOrderDetail>>($"api/RestAPI/UpdateOrder", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }



        [HttpPost("UpdatePembayaran")]
        public async Task<IActionResult> UpdatePembayaran([FromHeader(Name = "Authorization")] string token, QueryModel<updatePembayaran> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<updatePembayaran>>($"api/RestAPI/UpdatePembayaran", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("DeleteItem")]
        public async Task<IActionResult> DeleteItem([FromHeader(Name = "Authorization")] string token, QueryModel<DeleteItem> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<DeleteItem>>($"api/RestAPI/DeleteItem", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromHeader(Name = "Authorization")] string token, QueryModel<SentUpdateItemModel> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentUpdateItemModel>>($"api/RestAPI/UpdateItem", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("DeleteItemMaster")]
        public async Task<IActionResult> DeleteItemMaster([FromHeader(Name = "Authorization")] string token, QueryModel<SentDeleteItemModel> File)
        {
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();

            IActionResult actionResult = null;
            bool dataError = false;
            object lockObject = new object();
            ReturnMessage dataresult = new ReturnMessage();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                var resault = await _http.PostAsJsonAsync<QueryModel<SentDeleteItemModel>>($"api/RestAPI/DeleteItemMaster", File);

                var result = await resault.Content.ReadFromJsonAsync<ResultModel<ReturnMessage>>();
                if (result.isSuccess)
                {
                    res.Data = result.Data;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;
                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = "Bad request ToFacade Controller";
                actionResult = BadRequest(res);
            }
            return actionResult;
        }



        [HttpPost("GetReportStok")]
        public async Task<IActionResult> GetReportStok([FromHeader(Name = "Authorization")] string token, QueryModel<AGetReportStockDataModel> data)
        {
            ResultModel<List<GetReportStockData>> res = new ResultModel<List<GetReportStockData>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetReportStockDataModel>>($"api/RestAPI/GetReportStok", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<GetReportStockData>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("GetReportStokTotalRow")]
        public async Task<IActionResult> GetReportStokTotalRow([FromHeader(Name = "Authorization")] string token, QueryModel<AGetReportStockDataModel> data)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetReportStockDataModel>>($"api/RestAPI/GetReportStokTotalRow", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int?>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }
        [HttpPost("GetReporOrderData")]
        public async Task<IActionResult> GetReporOrderData([FromHeader(Name = "Authorization")] string token, QueryModel<AGetReportOrderDataModel> data)
        {
            ResultModel<List<GetReportOrderData>> res = new ResultModel<List<GetReportOrderData>>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetReportOrderDataModel>>($"api/RestAPI/GetReporOrderData", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<GetReportOrderData>>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }


        [HttpPost("GetReportOrderDataTotalRow")]
        public async Task<IActionResult> GetReportOrderDataTotalRow([FromHeader(Name = "Authorization")] string token, QueryModel<AGetReportOrderDataModel> data)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            IActionResult actionResult = null;

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");
                var result = await _http.PostAsJsonAsync<QueryModel<AGetReportOrderDataModel>>($"api/RestAPI/GetReportOrderDataTotalRow", data);
                var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int?>>();

                if (respBody.isSuccess)
                {
                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }
    }
}
