using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FoodOrderAPI.Models;
using FoodOrderAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
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
        private string PathFolder;
        private readonly FoodOrderService _restService = new FoodOrderService();

        public FoodOrderAPIController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _conString = config.GetValue<string>("ConnectionStrings:SCFoodOrder");
            _AudienceRES = config.GetValue<string>("JWTSettings:Audience");
            _IssuerRES = config.GetValue<string>("JWTSettings:Issuer");
            _SigningKeyRES = config.GetValue<string>("JWTSettings:IssuerSigningKey");
            _RowsOfPage = config.GetValue<string>("Paging:RowsOfPageOrder");
            PathFolder = config.GetValue<string>("File:TransportTracking:UploadFilePath");
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

        [Authorize]
        [HttpGet("GetMasterItem/{idjenis}")]
        public async Task<IActionResult> GetMasterItem([FromHeader(Name = "Authorization")] string token, string idjenis)
        {
            ResultModel<List<MasterItem>> res = new ResultModel<List<MasterItem>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<MasterItem> Data = new List<MasterItem>();

            try
            {
                HeaderData = _restService.GetMasterItem(_conString, idjenis);
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        MasterItem tmp = new MasterItem
                        {
                            IdItem = dt["IdItem"]?.ToString(),
                            NamaItem = dt["NamaItem"]?.ToString(),
                            Harga = Convert.ToInt32(dt["Harga"] ?? 0),
                            IdJenis = dt["IdJenis"]?.ToString(),
                            QtyAvailable = dt["QtyAvailable"]?.ToString(),
                            Tersedia = dt["Tersedia"]?.ToString(),
                            Id = Convert.ToInt32(dt["IdFile"] ?? 0),
                            IdLogBook = Convert.ToInt32(dt["IdItem"] ?? 0),
                            FileName = dt["FileName"]?.ToString(),
                            FileType = dt["FileType"]?.ToString(),
                            FilePath = dt["FilePath"]?.ToString(),
                            Size = Convert.ToInt64(dt["Size"] ?? 0)

                        };
                        Data.Add(tmp);

                    }

                    res.Data = Data;

                    // Iterasi file hanya jika Data terisi
                    foreach (var file in res.Data)
                    {
                        if (!string.IsNullOrEmpty(file.FilePath))
                        {
                            byte[] DataStream = _restService.DownloadFile(PathFolder + file.FilePath);
                            file.fileContent = DataStream;
                        }
                        else
                        {
                            res.isSuccess = false;
                            res.ErrorMessage = "File path is missing for one of the files.";
                            return BadRequest(res);
                        }
                    }

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




        [Authorize]
        [HttpGet("GetCategory/{idjenis}")]
        public async Task<IActionResult> GetCategory([FromHeader(Name = "Authorization")] string token, string idjenis)
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









        [Authorize]
        [HttpPost("UpdateTersedia")]
        public async Task<IActionResult> UpdateTersedia([FromHeader(Name = "Authorization")] string token, QueryModel<GetMenu> data)
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

        [Authorize]
        [HttpPost("SentCreateMenu")]
        public async Task<IActionResult> SentCreateMenu([FromHeader(Name = "Authorization")] string token, QueryModel<AddNewMenu> data)
        {
            var res = new ResultModel<ReturnMessage>();
            var dataResult = new ReturnMessage();
            List<DetailFileType> tmpData = new List<DetailFileType>();
            IActionResult actionResult = null;

            try
            {

                if (data.Data.DetailFileType == null || !data.Data.DetailFileType.Any())
                {
                    res.isSuccess = false;
                    res.ErrorCode = "99";
                    res.ErrorMessage = "TransportTrackingTypeFiles cannot be null or empty.";
                    return BadRequest(res);
                }

                foreach (DetailFileType file in data.Data.DetailFileType)
                {
                    if (file.fileContent == null || file.fileContent.Length == 0)
                    {
                        res.isSuccess = false;
                        res.ErrorCode = "99";
                        res.ErrorMessage = "File content cannot be null or empty.";
                        return BadRequest(res);
                    }

                    string RdnName = Path.GetRandomFileName();
                    file.FileName = RdnName + "_" + file.FileName;
                    file.FilePath = _restService.getDateFolder() + "\\" + "Selesai_Muat" + "\\" + file.FileName;
                    tmpData.Add(file);
                }



                DataTable SentCreateMenu = _restService.ListToDataTable<SentAddMenu>(data.Data.SentAddMenuModel, "");

                DataTable GetData = _restService.ListToDataTable<DetailFileType>(data.Data.DetailFileType, "");


                // Menggunakan ResultModel<ReturnMessageInsertDC> sebagai hasil dari SentOutboundData

                data.Data.DetailFileType = tmpData;

                foreach (DetailFileType file in data.Data.DetailFileType)
                {
                    _restService.SaveFile(PathFolder, file.fileContent, file.FileName, "Selesai_Muat");
                }


                var result = _restService.SentOutboundData(GetData, SentCreateMenu, _conString);

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


        [Authorize]
        [HttpPost("SentOrderNew")]
        public async Task<IActionResult> SentOrderNew([FromHeader(Name = "Authorization")] string token, QueryModel<SentOrder> data)
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




        [Authorize]
        [HttpGet("GetHeaderOrder/{Username}/{Role}/{PageNumber}/{filter}")]
        public async Task<IActionResult> GetHeaderOrder([FromHeader(Name = "Authorization")] string token, string Username, string Role, int PageNumber, string filter)
        {
            ResultModel<List<GetOrderData>> res = new ResultModel<List<GetOrderData>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<GetOrderData> Data = new List<GetOrderData>();

            try
            {
                HeaderData = _restService.GetHeaderOrder(_conString, Username,Role,PageNumber,filter, Convert.ToInt32(_RowsOfPage));
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

                    // Iterasi file hanya jika Data terisi
                    

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




        [Authorize]
        [HttpGet("GetDataOrderTotalRow/{Username}/{Role}/{filter}")]
        public async Task<IActionResult> GetHeaderOrder([FromHeader(Name = "Authorization")] string token, string Username, string Role, string filter)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            int ListData = 0;
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;

            try
            {
                HeaderData = _restService.GetDataOrderTotalRow(_conString, Username, Role, filter, Convert.ToInt32(_RowsOfPage));
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

        [Authorize]
        [HttpPost("SentAddOrderNew")]
        public async Task<IActionResult> SentAddOrderNew([FromHeader(Name = "Authorization")] string token, QueryModel<SentOrder> data)
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

        [Authorize]
        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromHeader(Name = "Authorization")] string token, QueryModel<updateOrderDetail> data)
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


        [Authorize]
        [HttpPost("UpdatePembayaran")]
        public async Task<IActionResult> UpdatePembayaran([FromHeader(Name = "Authorization")] string token, QueryModel<updatePembayaran> data)
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

        [Authorize]
        [HttpPost("DeleteItem")]
        public async Task<IActionResult> DeleteItem([FromHeader(Name = "Authorization")] string token, QueryModel<DeleteItem> data)
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

            [Authorize]
            [HttpPost("UpdateItem")]
        

            public async Task<IActionResult> UpdateItem([FromHeader(Name = "Authorization")] string token, QueryModel<SentUpdateItemModel> data)
            {
            
            DataTable dataTable = new DataTable();
            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();
            IActionResult actionResult = null;

            try
            {
                dataTable = _restService.UpdateItem(_conString, data.Data.IdItem, data.Data.NamaItem, data.Data.QTY, data.Data.Harga);

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








        [Authorize]
        [HttpDelete("DeleteItemMaster/{id}")]
        public async Task<IActionResult> DeleteItem([FromHeader(Name = "Authorization")] string token, string id)
        {
            IActionResult actionResult = null;

            DataTable dataTable = new DataTable();

            ResultModel<ReturnMessage> res = new ResultModel<ReturnMessage>();
            ReturnMessage dataResult = new ReturnMessage();

            try
            {
                // Tambahkan token ke header sebelum melakukan operasi
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"{token}");

                // Validasi id
                dataTable = _restService.DeleteItemMaster(_conString, id);
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




        [Authorize]
        [HttpGet("GetReportStok/{PageNumber}/{filter}")]
        public async Task<IActionResult> GetReportStok([FromHeader(Name = "Authorization")] string token, int PageNumber, string filter)
        {
            ResultModel<List<GetReportStockData>> res = new ResultModel<List<GetReportStockData>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<GetReportStockData> Data = new List<GetReportStockData>();

            try
            {
                HeaderData = _restService.GetReportStok(_conString, PageNumber, filter, Convert.ToInt32(_RowsOfPage));
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        GetReportStockData tmp = new GetReportStockData();
                        tmp.IdItem = dt["IdItem"].ToString();
                        tmp.NamaItem = dt["NamaItem"].ToString();
                        tmp.harga = dt["harga"].ToString();
                        tmp.qtyavailable = dt["qtyavailable"].ToString();
                        tmp.NamaJenis = dt["NamaJenis"].ToString();

                        Data.Add(tmp);

                    }

                    res.Data = Data;

                    // Iterasi file hanya jika Data terisi


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





        [Authorize]
        [HttpGet("GetReportStokTotalRow/{filter}")]
        public async Task<IActionResult> GetReportStokTotalRow([FromHeader(Name = "Authorization")] string token, string filter)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            int ListData = 0;
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;

            try
            {
                HeaderData = _restService.GetReportStokTotalRow(_conString, filter, Convert.ToInt32(_RowsOfPage));
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


        [Authorize]
        [HttpGet("GetReporOrderData/{PageNumber}/{filter}")]
        public async Task<IActionResult> GetReporOrderData([FromHeader(Name = "Authorization")] string token, int PageNumber, string filter)
        {
            ResultModel<List<GetReportOrderData>> res = new ResultModel<List<GetReportOrderData>>();
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;
            List<GetReportOrderData> Data = new List<GetReportOrderData>();

            try
            {
                HeaderData = _restService.GetReporOrderData(_conString, PageNumber, filter, Convert.ToInt32(_RowsOfPage));
                if (HeaderData.Rows.Count > 0)
                {
                    foreach (DataRow dt in HeaderData.Rows)
                    {
                        GetReportOrderData tmp = new GetReportOrderData();
                        tmp.IdItem = dt["IdItem"].ToString();
                        tmp.NamaItem = dt["NamaItem"].ToString();
                        tmp.Qty = dt["Qty"].ToString();
                        tmp.Total = dt["Total"].ToString();
                        tmp.HargaSatuan = dt["HargaSatuan"].ToString();
                        tmp.CreateDate = dt["Tanggal"].ToString();

                        Data.Add(tmp);


                    }

                    res.Data = Data;

                    // Iterasi file hanya jika Data terisi


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





        [Authorize]
        [HttpGet("GetReportOrderDataTotalRow/{filter}")]
        public async Task<IActionResult> GetReportOrderDataTotalRow([FromHeader(Name = "Authorization")] string token, string filter)
        {
            ResultModel<int?> res = new ResultModel<int?>();
            int ListData = 0;
            DataTable HeaderData = new DataTable();
            IActionResult actionResult = null;

            try
            {
                HeaderData = _restService.GetReportOrderDataTotalRow(_conString, filter, Convert.ToInt32(_RowsOfPage));
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



        
    }
}
