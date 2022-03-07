using ApiRaices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Xml.Linq;
using System.Text;
using ApiRaices.Models.Validations;
using FluentValidation.Results;
using System;
using FluentValidation;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PropertyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable resultDataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder sqlQuery = new StringBuilder("SELECT ");
            sqlQuery.Append("p.IdProperty, p.Name, p.Address, p.Price, ");
            sqlQuery.Append("p.CodeInternal, p.Year, p.IdOwner, o.Name as OwnerName ");
            sqlQuery.Append("FROM dbo.Property p ");
            sqlQuery.Append("INNER JOIN dbo.Owner o  ");
            sqlQuery.Append("ON o.IdOwner = p.IdOwner; ");

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


        [HttpPost]
        public JsonResult Post(Property property)
        {
            //Value to avoid error in the validator, not used on the query
            property.IdProperty = 1;

            PropertyValidatior validator = new PropertyValidatior();
            ValidationResult validationResults = validator.Validate(property);

            if (!validationResults.IsValid)
            {
                string errorResult = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    errorResult += "Invalid data: " + failure.ErrorMessage + " \r\n";
                }
                return new JsonResult(errorResult);
            }
            else
            {
                string tmp = "";
                try
                {
                    StringBuilder stringQuery = new StringBuilder("BEGIN TRANSACTION; ");
                    stringQuery.Append("INSERT INTO dbo.Property ");
                    stringQuery.Append("(Name, Address, Price, ");
                    stringQuery.Append("CodeInternal, Year, IdOwner) ");
                    stringQuery.Append("VALUES ");
                    stringQuery.Append("(@Name, @Address, @Price, ");
                    stringQuery.Append("@CodeInternal, @Year, @IdOwner); ");
                    stringQuery.Append("COMMIT TRANSACTION;");

                    tmp = stringQuery.ToString();
                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                    int queryResult;
                    using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@Name", property.Name);
                            sqlCommand.Parameters.AddWithValue("@Address", property.Address);
                            sqlCommand.Parameters.AddWithValue("@Price", property.Price);
                            sqlCommand.Parameters.AddWithValue("@CodeInternal", property.CodeInternal);
                            sqlCommand.Parameters.AddWithValue("@Year", property.Year);
                            sqlCommand.Parameters.AddWithValue("@IdOwner", property.IdOwner);

                            queryResult = sqlCommand.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    if (queryResult > 0)
                    {
                        return new JsonResult("Property registered successfully.");
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult("Database error: " + ex.Message + tmp);
                }
            }

            return new JsonResult("Error. No changes were made.");
        }


        [HttpPut]
        public JsonResult Put(Property property)
        {

            PropertyValidatior validator = new();
            ValidationResult validationResults = validator.Validate(property);

            if (!validationResults.IsValid)
            {
                string errorResult = string.Empty;
                foreach (var failure in validationResults.Errors)
                {
                    errorResult += "Invalid data: " + failure.ErrorMessage + "\r\n";
                }
                return new JsonResult(errorResult);
            }
            else
            {
                try
                {
                    StringBuilder stringQuery = new StringBuilder("BEGIN TRANSACTION; ");
                    stringQuery.Append("UPDATE dbo.Property SET ");
                    stringQuery.Append("Name = @Name, ");
                    stringQuery.Append("Address = @Address, ");
                    stringQuery.Append("Price = @Price, ");
                    stringQuery.Append("CodeInternal = @CodeInternal, ");
                    stringQuery.Append("Year = @Year, ");
                    stringQuery.Append("IdOwner = @IdOwner ");
                    stringQuery.Append("WHERE IdProperty = @IdProperty; ");
                    stringQuery.Append("COMMIT TRANSACTION;");


                    int queryResult;
                    string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

                    using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                    {
                        sqlConnection.Open();
                        using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                        {
                            sqlCommand.Parameters.AddWithValue("@Name", property.Name);
                            sqlCommand.Parameters.AddWithValue("@Address", property.Address);
                            sqlCommand.Parameters.AddWithValue("@Price", property.Price);
                            sqlCommand.Parameters.AddWithValue("@CodeInternal", property.CodeInternal);
                            sqlCommand.Parameters.AddWithValue("@Year", property.Year);
                            sqlCommand.Parameters.AddWithValue("@IdOwner", property.IdOwner);
                            sqlCommand.Parameters.AddWithValue("@IdProperty", property.IdProperty);

                            queryResult = sqlCommand.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    if (queryResult > 0)
                    {
                        return new JsonResult("Property updated successfully.");
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
