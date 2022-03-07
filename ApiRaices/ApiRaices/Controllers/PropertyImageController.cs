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
using FluentValidation.Results;
using System;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly  IConfiguration _configuration;

        public PropertyImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{IdProperty}")]
        public JsonResult GetPropertyImages( int IdProperty)
        {

            DataTable resultDataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder sqlQuery = new StringBuilder("SELECT ");
            sqlQuery.Append("Photo, IdPropertyImage ");
            sqlQuery.Append("FROM dbo.PropertyImage ");
            sqlQuery.Append("WHERE IdProperty = ");
            sqlQuery.Append(IdProperty);
            sqlQuery.Append(" AND Enabled = 1; ");

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery.ToString(), sqlConnection))
                {
                    sqlConnection.Open();

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


        [HttpPut]
        public JsonResult HideImage(PropertyImage propertyImage)
        {
            PropertyImageValidator validator = new PropertyImageValidator();
            ValidationResult validationResults = validator.Validate(propertyImage);

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
                    stringQuery.Append("UUPDATE dbo.PropertyImage SET ");
                    stringQuery.Append("Enabled = 0 ");
                    stringQuery.Append("WHERE IdPropertyImage = @IdPropertyImage");
                    stringQuery.Append("COMMIT TRANSACTION;");

                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                    int queryResult;
                    using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@IdPropertyImage", propertyImage.IdPropertyImage);

                            queryResult = sqlCommand.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    if (queryResult > 0)
                    {
                        return new JsonResult("Image deleted successfully.");
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
