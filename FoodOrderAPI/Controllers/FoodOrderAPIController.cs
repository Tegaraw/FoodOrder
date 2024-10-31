using FoodOrderAPI.Models;
using FoodOrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FoodOrderAPI.Controllers
{
    [Route("api/RestAPI")]
    [ApiController]
    public class FoodOrderAPIController : ControllerBase // Gunakan ControllerBase
    {
        private readonly HttpClient _http;
        private readonly string _conString;
        private readonly string _AudienceRES;
        private readonly string _IssuerRES;
        private readonly string _SigningKeyRES;
        private readonly string _RowsOfPage;
        public string logresult;

        private readonly FoodOrderService _restService = new FoodOrderService();

        public FoodOrderAPIController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _conString = config.GetValue<string>("ConnectionStrings:SCFoodOrder");
            _AudienceRES = config.GetValue<string>("JWTSettings:Audience");
            _IssuerRES = config.GetValue<string>("JWTSettings:Issuer");
            _SigningKeyRES = config.GetValue<string>("JWTSettings:IssuerSigningKey");
            _RowsOfPage = config.GetValue<string>("Paging:RowsOfPageOrder");
        }

        [HttpPost("LoginUsers")]
        public async Task<IActionResult> LoginUsers(RequestLogin file)
        {
            DataTable dt = new DataTable();
            ResultModel<ResultLogin> res = new ResultModel<ResultLogin> { Data = new ResultLogin() };

            try
            {
                dt = _restService.LoginUser(file, _conString);
                if (dt.Rows.Count == 1)
                {
                    foreach (DataRow d in dt.Rows)
                    {
                        res.Data.Id = Convert.ToInt32(d["IdUser"]);
                        res.Data.Name = d["Name"].ToString();
                        res.Data.Username = d["Username"].ToString();
                        res.Data.Role = d["IdRole"].ToString();
                    }

                    res.Data.Token = _restService.CreateJwtToken(_AudienceRES, _IssuerRES, _SigningKeyRES, res.Data.Name, res.Data.Username, file);
                }

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                return BadRequest(res);
            }
        }

        [HttpPost("GetMasterItem")]
        public async Task<IActionResult> GetMasterItem(QueryModel<GetMenu> data)
        {
            ResultModel<List<MasterItem>> res = new ResultModel<List<MasterItem>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<MasterItem> Data = new List<MasterItem>();

            try
            {
                HeaderData = _restService.GetMasterItem(_conString, data.Data.IdJenis);
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        MasterItem tmp = new MasterItem();

                        tmp.IdItem = dt["IdItem"].ToString();
                        tmp.NamaItem = dt["NamaItem"].ToString();
                        tmp.Harga = Convert.ToInt32(dt["Harga"]);

                        tmp.IdJenis = dt["IdJenis"].ToString();
                        tmp.QtyAvailable = dt["QtyAvailable"].ToString();
                        tmp.Tersedia = dt["Tersedia"].ToString();



                        Data.Add(tmp);

                    }

                    res.Data = Data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> GetCategory()
        {
            ResultModel<List<getCategory>> res = new ResultModel<List<getCategory>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<getCategory> Data = new List<getCategory>();

            try
            {
                HeaderData = _restService.GetCategory(_conString);
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        getCategory tmp = new getCategory();

                        tmp.IdJenis = dt["IdJenis"].ToString();
                        tmp.NamaJenis = dt["NamaJenis"].ToString();
                       

                        Data.Add(tmp);

                    }

                    res.Data = Data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> UpdateTersedia(QueryModel<GetMenu> data)
        {
            DataTable dataTable = new DataTable();
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();
            IActionResult actionResult = null;

            try
            {
                dataTable = _restService.UpdateTersedia(_conString, data.Data.IdJenis, data.Data.Tersedia);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow detailRow in dataTable.Rows)
                    {
                        dataResult.message = detailRow["MESSAGE"].ToString();

                    }

                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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


        [HttpPost("SentCreateMenu")]
        public async Task<IActionResult> SentCreateMenu(QueryModel<AddNewMenu> data)
        {
            var res = new ResultModel<ReturnMessage>();
            var dataResult = new ReturnMessage();

            IActionResult actionResult = null;

            try
            {
                DataTable SentCreateMenu = _restService.ListToDataTable<SentAddMenu>(data.Data.SentAddMenuModel, "");

                // Menggunakan ResultModel<ReturnMessageInsertDC> sebagai hasil dari SentOutboundData
                var result = _restService.SentOutboundData(SentCreateMenu, _conString);

                if (result.isSuccess)
                {
                    dataResult.message = result.Data.message;
                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }

                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage ?? "No data returned.";
                    return BadRequest(res);
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



        [HttpPost("SentOrderNew")]
        public async Task<IActionResult> SentOrderNew(QueryModel<SentOrder> data)
        {
            var res = new ResultModel<ReturnMessage>();
            var dataResult = new ReturnMessage();

            IActionResult actionResult = null;

            try
            {
                DataTable SentOrderData = _restService.ListToDataTable<OrderDetail>(data.Data.SentOrderDetail, "");

                // Menggunakan ResultModel<ReturnMessageInsertDC> sebagai hasil dari SentOutboundData
                var result = _restService.SentOrderNew(data, SentOrderData, _conString);

                if (result.isSuccess)
                {
                    dataResult.message = result.Data.message;
                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }

                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage ?? "No data returned.";
                    return BadRequest(res);
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

        [HttpPost("GetHeaderOrder")]
        public async Task<IActionResult> GetHeaderOrder(QueryModel<AGetOrderModel> data)
        {
            ResultModel<List<GetOrderData>> res = new ResultModel<List<GetOrderData>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<GetOrderData> Data = new List<GetOrderData>();

            try
            {
                HeaderData = _restService.GetHeaderOrder(_conString, data.Data, Convert.ToInt32(_RowsOfPage));
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        GetOrderData tmp = new GetOrderData();
                        tmp.IdHeaderOrder = dt["IdHeaderOrder"].ToString();
                        tmp.NomorOrder = dt["NomorOrder"].ToString();
                        tmp.NomorMeja = dt["NomorMeja"].ToString();
                        tmp.NamaStatus = dt["NamaStatus"].ToString();
                        tmp.CreateBy = dt["CreateBy"].ToString();
                        tmp.IdStatus = dt["IdStatus"].ToString();
                        tmp.CreateDate = dt["Tanggal"].ToString();

                        Data.Add(tmp);

                    }

                    res.Data = Data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> GetDataOrderTotalRow(QueryModel<AGetOrderModel> data)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            int ListData = 0;
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;

            try
            {
                HeaderData = _restService.GetDataOrderTotalRow(_conString, data.Data, Convert.ToInt32(_RowsOfPage));
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        ListData = Convert.ToInt32(dt["NumberRows"]);
                    }

                    res.Data = ListData;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> GetDetailOrder(QueryModel<GetDetailOrder> data)
        {
            ResultModel<List<getOrderDetail>> res = new ResultModel<List<getOrderDetail>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<getOrderDetail> Data = new List<getOrderDetail>();

            try
            {
                HeaderData = _restService.GetDetailOrder(_conString, data.Data.IdHeaderOrder);
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        getOrderDetail tmp = new getOrderDetail();

                        tmp.IdItem = dt["IdItem"].ToString();
                        tmp.NamaItem = dt["NamaItem"].ToString();
                        tmp.IdHeaderOrder = dt["IdHeaderOrder"].ToString();
                        tmp.IdDetailOrder = dt["IdDetailOrder"].ToString();
                        tmp.Total = dt["Total"].ToString();
                        tmp.Qty = int.Parse(dt["Qty"].ToString());
                        tmp.HargaSatuan = int.Parse(dt["harga"].ToString());




                        Data.Add(tmp);

                    }

                    res.Data = Data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> SentAddOrderNew(QueryModel<SentOrder> data)
        {
            var res = new ResultModel<ReturnMessage>();
            var dataResult = new ReturnMessage();

            IActionResult actionResult = null;

            try
            {
                DataTable SentOrderData = _restService.ListToDataTable<OrderDetail>(data.Data.SentOrderDetail, "");

                // Menggunakan ResultModel<ReturnMessageInsertDC> sebagai hasil dari SentOutboundData
                var result = _restService.SentAddOrderNew(data, SentOrderData, _conString);

                if (result.isSuccess)
                {
                    dataResult.message = result.Data.message;
                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }

                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage ?? "No data returned.";
                    return BadRequest(res);
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

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(QueryModel<updateOrderDetail> data)
        {
            DataTable dataTable = new DataTable();
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();
            IActionResult actionResult = null;

            try
            {
                dataTable = _restService.UpdateOrder(_conString, data.Data.IdItem, data.Data.Qty, data.Data.Total);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow detailRow in dataTable.Rows)
                    {
                        dataResult.message = detailRow["MESSAGE"].ToString();

                    }

                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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



        [HttpPost("UpdatePembayaran")]
        public async Task<IActionResult> UpdatePembayaran(QueryModel<updatePembayaran> data)
        {
            DataTable dataTable = new DataTable();
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();
            IActionResult actionResult = null;

            try
            {
                dataTable = _restService.UpdatePembayaran(_conString, data.Data.IdHeaderOrder, data.username);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow detailRow in dataTable.Rows)
                    {
                        dataResult.message = detailRow["MESSAGE"].ToString();

                    }

                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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


        [HttpPost("DeleteItem")]
        public async Task<IActionResult> DeleteItem(QueryModel<DeleteItem> data)
        {
            DataTable dataTable = new DataTable();
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();
            IActionResult actionResult = null;

            try
            {
                dataTable = _restService.DeleteItem(_conString, data.Data.IdHeaderOrder, data.Data.IdDetailOrder);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow detailRow in dataTable.Rows)
                    {
                        dataResult.message = detailRow["MESSAGE"].ToString();

                    }

                    res.Data = dataResult;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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
