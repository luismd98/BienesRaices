using ApiRaices.Models;
using ApiRaices.Models.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _newLine = Environment.NewLine;

        public OwnerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public OwnerController()        {       }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable resultDataTable = new DataTable();
            JsonResult jResult;

            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder queryString = new StringBuilder("SELECT " + _newLine);
            queryString.Append("IdOwner, Name, Address, Photo, " + _newLine);
            queryString.Append("convert(varchar(10),Birthday,120) as Birthday " + _newLine);
            queryString.Append("FROM dbo.Owner");

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection))
                {
                    sqlConnection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(resultDataTable);

                    adapter.Dispose();
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                jResult = new JsonResult("Database error: " + ex.Message);
                jResult.StatusCode = 500;
                return jResult;
            }

            jResult = new JsonResult(resultDataTable);
            jResult.StatusCode = 200;
            return jResult;
        }


        [HttpPost]
        public JsonResult Post(Owner owner)
        {
            OwnerValidator validator = new OwnerValidator();
            ValidationResult validationResults = validator.Validate(owner);

            JsonResult jResult;

            if (!validationResults.IsValid)
            {
                string result = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    result += "Invalid data: " + failure.ErrorMessage + _newLine;
                }
                jResult = new JsonResult(result);
                jResult.StatusCode = 400;
                return jResult;
            }
            else
            {
                try
                {
                    StringBuilder query = new StringBuilder("BEGIN TRANSACTION; " + _newLine);
                    query.Append("INSERT INTO dbo.Owner ");
                    query.Append("(Name, Address, Photo, Birthday) ");
                    query.Append("VALUES ");
                    query.Append("(@Name, @Address, @Photo, @Birthday);");
                    query.Append("COMMIT TRANSACTION;");

                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                    int result;
                    using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                    {
                        myConn.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(query.ToString(), myConn))
                        {
                            sqlCommand.Parameters.AddWithValue("@Name", owner.Name);
                            sqlCommand.Parameters.AddWithValue("@Address", owner.Address);
                            sqlCommand.Parameters.AddWithValue("@Photo", owner.Photo);
                            sqlCommand.Parameters.AddWithValue("@Birthday", owner.Birthday);

                            result = sqlCommand.ExecuteNonQuery();

                            myConn.Close();
                        }
                    }
                    if (result > 0)
                    {
                        jResult = new JsonResult("New Owner registered successfully.");
                        jResult.StatusCode = 200;
                        return jResult;
                    }
                }
                catch (Exception ex)
                {
                    jResult = new JsonResult("Database error: " + ex.Message);
                    jResult.StatusCode = 500;
                    return jResult;
                }

            }
            jResult = new JsonResult("Error. No changes were made.");
            jResult.StatusCode = 400;
            return jResult;
        }


        [HttpPut]
        public JsonResult Put(Owner owner)
        {
            OwnerValidator validator = new OwnerValidator();
            ValidationResult validationResults = validator.Validate(owner);

            JsonResult jResult;

            if (!validationResults.IsValid)
            {
                string errorResult = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    errorResult += "Invalid data: " + failure.ErrorMessage + _newLine;
                }
                jResult = new JsonResult(errorResult);
                jResult.StatusCode = 400;
                return jResult;
            }
            else
            {
                try
                {
                    StringBuilder stringQuery = new StringBuilder("BEGIN TRANSACTION; " + _newLine);
                    stringQuery.Append("UPDATE dbo.Owner SET " + _newLine);
                    stringQuery.Append("Name = @Name, " + _newLine);
                    stringQuery.Append("Address = @Address, " + _newLine);
                    stringQuery.Append("Photo = @Photo, " + _newLine);
                    stringQuery.Append("Birthday = @Birthday " + _newLine);
                    stringQuery.Append("WHERE IdOwner = @IdOwner; " + _newLine);
                    stringQuery.Append("COMMIT TRANSACTION;" + _newLine);

                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                    int queryResult;
                    using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@Name", owner.Name);
                            sqlCommand.Parameters.AddWithValue("@Address", owner.Address);
                            sqlCommand.Parameters.AddWithValue("@Photo", owner.Photo);
                            sqlCommand.Parameters.AddWithValue("@Birthday", owner.Birthday);
                            sqlCommand.Parameters.AddWithValue("@IdOwner", owner.IdOwner);

                            queryResult = sqlCommand.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    if (queryResult > 0)
                    {
                        jResult = new JsonResult("Owner updated successfully.");
                        jResult.StatusCode = 200;
                        return jResult;
                    }

                }
                catch (Exception ex)
                {
                    jResult = new JsonResult("Database error: " + ex.Message);
                    jResult.StatusCode = 500;
                    return jResult;
                }

            }
            jResult = new JsonResult("Error. No changes were made.");
            jResult.StatusCode = 400;
            return jResult;
        }

        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            DataTable resultDataTable = new DataTable();
            JsonResult jResult;

            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder queryString = new StringBuilder("Select *  ");
            queryString.Append("from property ");
            queryString.Append("WHERE property.IdOwner = @IdOwner");


            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection))
                {
                    sqlConnection.Open();

                    cmd.Parameters.AddWithValue("@IdOwner", id);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(resultDataTable);

                    adapter.Dispose();
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                jResult = new JsonResult("Database error: " + ex.Message);
                jResult.StatusCode = 500;
                return jResult;
            }

            jResult = new JsonResult(resultDataTable);
            jResult.StatusCode = 200;
            return jResult;
        }


    }
}
