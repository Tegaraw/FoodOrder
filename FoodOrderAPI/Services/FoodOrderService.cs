﻿using DocumentFormat.OpenXml.Presentation;
using FoodOrderAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace FoodOrderAPI.Services
{
    public class FoodOrderService
    {
        string logresult = "";
        private static object _lockObject = new object();

   
        internal DataTable LoginUser(RequestLogin data, string _conString)
        {
            lock (_lockObject)
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[Login]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Username", data.Username);
                        command.Parameters.AddWithValue("@Password", data.Password);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return dt;
            }
        }

        public string CreateJwtToken(string audienceRES, string issuerRES, string signingRES, string name, string username, RequestLogin req)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingRES));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim ("Username",username),
                new Claim ("Name", name)
            };
            var token = new JwtSecurityToken(
                issuer: issuerRES,
                audience: audienceRES,
                claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1))
                ;

            var tokken = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(tokken);
            return tokken;
        }


        internal DataTable GetMasterItem(string _conString, string IdJenis)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetMasterItem]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdJenis", IdJenis);
                 





                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable GetCategory(string _conString)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetKategory]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable UpdateTersedia(string _conString, string IdJenis, string tersedia)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[UpdateMasterItemTersedia]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdItem", IdJenis);
                    command.Parameters.AddWithValue("@tersedia", tersedia);
                  



                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }

        public DataTable ListToDataTable<T>(List<T> list, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }
            return dt;

        }


        internal ResultModel<ReturnMessage> SentOutboundData(DataTable ListData, DataTable ListDataLoadingManifest, string _conString)
        {
            lock (_lockObject)
            {
                var result = new ResultModel<ReturnMessage>();
                var dataResult = new ReturnMessage();

                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();

                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[InsertMasterItem]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                       


                        command.Parameters.AddWithValue("@DataMasterItem", ListDataLoadingManifest);
                        command.Parameters.AddWithValue("@DataFileImage", ListData);
                        int ret = command.ExecuteNonQuery();

                        if (ret >= 0)
                        {
                            result.isSuccess = true;
                            dataResult.message = "Operation succeeded";
                            result.Data = dataResult;
                            result.ErrorCode = "00";
                        }
                        else
                        {
                            result.isSuccess = false;
                            dataResult.message = "Operation failed";
                            result.Data = dataResult;
                            result.ErrorCode = "02";
                        }
                    }
                    catch (SqlException ex)
                    {
                        result.isSuccess = false;
                        dataResult.message = ex.Message;
                        result.Data = dataResult;
                        result.ErrorCode = "99";
                        result.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return result;
            }
        }

        internal ResultModel<ReturnMessage> SentOrderNew(QueryModel<SentOrder> data, DataTable ListData, string _conString)
        {
            lock (_lockObject)
            {
                var result = new ResultModel<ReturnMessage>();
                var dataResult = new ReturnMessage();

                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();

                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[InsertOrder]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@NomorMeja", data.Data.SentOrderModel.NomorMeja);
                        command.Parameters.AddWithValue("@PaidBy", data.Data.SentOrderModel.PaidBy);
                        command.Parameters.AddWithValue("@CreateBy", data.Data.SentOrderModel.CreateBy);

                        command.Parameters.AddWithValue("@DetailOrder", ListData);

                        int ret = command.ExecuteNonQuery();

                        if (ret >= 0)
                        {
                            result.isSuccess = true;
                            dataResult.message = "Operation succeeded";
                            result.Data = dataResult;
                            result.ErrorCode = "00";
                        }
                        else
                        {
                            result.isSuccess = false;
                            dataResult.message = "Operation failed";
                            result.Data = dataResult;
                            result.ErrorCode = "02";
                        }
                    }
                    catch (SqlException ex)
                    {
                        result.isSuccess = false;
                        dataResult.message = ex.Message;
                        result.Data = dataResult;
                        result.ErrorCode = "99";
                        result.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return result;
            }
        }

        internal DataTable GetHeaderOrder(string _conString, string Username, string Role, int PageNumber, string filter, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetHeaderOrder]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", filter);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }

        internal DataTable GetDataOrderTotalRow(string _conString, string Username, string Role, string filter, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetHeaderOrderTotalRow]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Role", Role);
                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", filter);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable GetDetailOrder(string _conString, string IdHeaderOrder)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetDetailOrder]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdHeaderOrder", IdHeaderOrder);






                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal ResultModel<ReturnMessage> SentAddOrderNew(QueryModel<SentOrder> data, DataTable ListData, string _conString)
        {
            lock (_lockObject)
            {
                var result = new ResultModel<ReturnMessage>();
                var dataResult = new ReturnMessage();

                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();

                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[InsertTambahOrder]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("@IdHeaderOrder", data.Data.SentOrderModel.IdHeaderOrder);
                        command.Parameters.AddWithValue("@DetailOrder", ListData);

                        int ret = command.ExecuteNonQuery();

                        if (ret >= 0)
                        {
                            result.isSuccess = true;
                            dataResult.message = "Operation succeeded";
                            result.Data = dataResult;
                            result.ErrorCode = "00";
                        }
                        else
                        {
                            result.isSuccess = false;
                            dataResult.message = "Operation failed";
                            result.Data = dataResult;
                            result.ErrorCode = "02";
                        }
                    }
                    catch (SqlException ex)
                    {
                        result.isSuccess = false;
                        dataResult.message = ex.Message;
                        result.Data = dataResult;
                        result.ErrorCode = "99";
                        result.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return result;
            }
        }


        internal DataTable UpdateOrder(string _conString, string IdItem, int Qty, string Total)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[UpdateOrder]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdItem", IdItem);
                    command.Parameters.AddWithValue("@QTY", Qty);
                    command.Parameters.AddWithValue("@Total", Total);




                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }



        internal DataTable UpdatePembayaran(string _conString, string IdHeaderOrder, string username)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[UpdatePembayaran]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdHeaderOrder", IdHeaderOrder);
                    command.Parameters.AddWithValue("@UserName", username);
                




                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable DeleteItem(string _conString, string IdHeaderOrder, string IdDetail)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[DeleteOrder]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdHeaderOrder", IdHeaderOrder);
                    command.Parameters.AddWithValue("@IdDetailOrder", IdDetail);





                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        public void MakeLog(string text)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("///////////// START" + Convert.ToString(DateTime.Now));
            try
            {
                //string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath);
                string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "File Log Restaurant");
                Directory.CreateDirectory(directory);

                string logFile = Path.Combine(directory, DateTime.Now.ToString("ddMMyyyy") + "power Restaurant" + ".txt");

                // Append the log entry to the file
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(text);
                    logresult += text + Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or throw it further.
                Console.WriteLine("An error occurred while writing to the log file: " + ex.Message + " " + date);
            }
            Console.WriteLine("///////////// END" + Convert.ToString(DateTime.Now));
        }

        public string getDateFolder()
        {
            string datePath = DateTime.Now.ToString(@"yyyy\\MM\\dd");

            return datePath;
        }

        public void SaveFile(string path, byte[] content, string filename, string IdSite)
        {
            try
            {
                string datePath = DateTime.Now.ToString("yyyy/MM/dd");
                string fullFolderPath = Path.Combine(path, datePath + "\\" + IdSite);
                string filePath = Path.Combine(fullFolderPath, filename);

                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                // Create a new file     
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    fs.Write(content, 0, content.Length);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public byte[] DownloadFile(string path)
        {
            byte[]? fileBytes = null;
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return fileBytes;
        }



        internal DataTable UpdateItem(string _conString, string IdItem,string NamaItem, string QTY, string harga)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[UpdateItem]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdItem", IdItem);
                    command.Parameters.AddWithValue("@NamaItem", NamaItem);

                    command.Parameters.AddWithValue("@QTY", QTY);
                    command.Parameters.AddWithValue("@Harga", harga);




                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable DeleteItemMaster(string _conString, string id)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[DeleteItemMaster]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@IdItem", id);
                   




                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


        internal DataTable GetReportStok(string _conString, int PageNumber, string Condition, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetReportStok]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                  
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", Condition);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }
        internal DataTable GetReportStokTotalRow(string _conString, string Condition, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetReportStokTotalRow]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", Condition);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }




        internal DataTable GetReporOrderData(string _conString, int PageNumber, string Condition, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetReportOrder]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@PageNumber", PageNumber);
                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", Condition);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }





        internal DataTable GetReportOrderDataTotalRow(string _conString, string Condition, int RowsOfPage)
        {
            DataTable Data = new DataTable(); ;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[GetReportOrderTotalRow]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@RowsOfPage", RowsOfPage);
                    command.Parameters.AddWithValue("@Condition", Condition);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(Data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return Data;
        }


    }
}
