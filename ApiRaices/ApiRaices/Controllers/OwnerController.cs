using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using ApiRaices.Models;
using System.IO;
using ApiRaices.Models.Validations;
using FluentValidation.Results;
using System;
using System.Data.SqlTypes;
using System.Text;
using System.Net;
using System.Xml.Linq;
using Microsoft.Extensions.Primitives;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly string _newLine = Environment.NewLine;

        public OwnerController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable resultDataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder queryString = new StringBuilder("SELECT "+_newLine);
            queryString.Append("IdOwner, Name, Address, Photo, "+ _newLine);
            queryString.Append("convert(varchar(10),Birthday,120) as Birthday " + _newLine);
            queryString.Append("FROM dbo.Owner");

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(queryString.ToString(), sqlConnection))
                {
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    resultDataTable.Load(sqlDataReader);

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                return new JsonResult("Database error: "+ex.Message);
            }
            return new JsonResult(resultDataTable);
        }


        [HttpPost]
        public JsonResult Post(Owner owner)
        {
            OwnerValidator validator = new OwnerValidator();
            ValidationResult validationResults = validator.Validate(owner);

            if (!validationResults.IsValid)
            {
                string result = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    result += "Invalid data: " + failure.ErrorMessage + _newLine;
                }
                return new JsonResult(result);
            }
            else
            {
                try
                {
                    StringBuilder query = new StringBuilder("BEGIN TRANSACTION; " + _newLine);
                    query.Append("INSERT INTO dbo.Owner ");
                    query.Append("(Name, Address, Photo, Birthday) ");
                    query.Append("VALUES " );
                    query.Append("(@Name, @Address, @Photo, @Birthday);" );
                    query.Append("COMMIT TRANSACTION;" );

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
                        return new JsonResult("New Owner registered successfully.");
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult("Database error: " + ex.Message);
                }
                
            }
            return new JsonResult("Error. No changes were made.");
        }



        [HttpPut]
        public JsonResult Put(Owner owner)
        {
            OwnerValidator validator = new OwnerValidator();
            ValidationResult validationResults = validator.Validate(owner);

            if (!validationResults.IsValid)
            {
                string errorResult = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    errorResult += "Invalid data: " + failure.ErrorMessage + _newLine;
                }
                return new JsonResult(errorResult);
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
                        return new JsonResult("Owner updated successfully.");
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult("Database error: " + ex.Message);
                }

            }
            return new JsonResult("Error. No changes were made.");
        }


    }
}
