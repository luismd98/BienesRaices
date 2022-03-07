using ApiRaices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Text;
using ApiRaices.Models.Validations;
using System;
using FluentValidation;
using FluentValidation.Results;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyTraceController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PropertyTraceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{IdProperty}")]
        public JsonResult GetPropertyTransactionTrace(int IdProperty)
        {
            DataTable resultDataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder sqlQuery = new StringBuilder("SELECT ");
            sqlQuery.Append(" * ");
            sqlQuery.Append("FROM PropertyTrace ");
            sqlQuery.Append("WHERE IdProperty = @IdProperty;");

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery.ToString(), sqlConnection))
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@IdProperty", IdProperty);

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    resultDataTable.Load(sqlDataReader);

                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }
            catch (SqlException sqlException)
            {
                return new JsonResult("Database error: " + sqlException.Message);
            }
            return new JsonResult(resultDataTable);

        }


        [HttpPost]
        public JsonResult CommitNewTransaction(PropertyTrace propertyTrace)
        {
            PropertyTraceValidator validator = new PropertyTraceValidator();
            ValidationResult validationResults = validator.Validate(propertyTrace);

            if (!validationResults.IsValid)
            {
                string errorResult = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    errorResult += "Invalid data: " + failure.ErrorMessage;
                }
                return new JsonResult(errorResult);
            }
            else
            {
                try
                {
                    StringBuilder stringQuery = new StringBuilder("BEGIN TRANSACTION; ");

                    //First query (insert)
                    stringQuery.Append("INSERT INTO dbo.PropertyTrace ");
                    stringQuery.Append("(DateSale, Name, Value, Tax, IdProperty) ");
                    stringQuery.Append("VALUES ");
                    stringQuery.Append("(@DateSale, @Name, @Value, @Tax, @IdProperty) ");

                    //Second update query
                    stringQuery.Append("UPDATE dbo.Property SET ");
                    stringQuery.Append("IdOwner = @IdOwner, ");
                    stringQuery.Append("Name = @Name, ");
                    stringQuery.Append("Price = @Value ");
                    stringQuery.Append("WHERE IdProperty = @IdProperty;");

                    stringQuery.Append("COMMIT TRANSACTION;");

                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                    int queryResult;
                    using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@DateSale", propertyTrace.DateSale);
                            sqlCommand.Parameters.AddWithValue("@Name", propertyTrace.Name);
                            sqlCommand.Parameters.AddWithValue("@Value", propertyTrace.Value);
                            sqlCommand.Parameters.AddWithValue("@Tax", propertyTrace.Tax);
                            sqlCommand.Parameters.AddWithValue("@IdProperty", propertyTrace.IdProperty);

                            sqlCommand.Parameters.AddWithValue("@IdOwner", propertyTrace.IdOwner);


                            queryResult = sqlCommand.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    if (queryResult > 0)
                    {
                        return new JsonResult("Transaction has been successful.");
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
