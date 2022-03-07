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
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");

            StringBuilder query = new StringBuilder("SELECT "+_newLine);
            query.Append("IdOwner, Name, Address, Photo, "+ _newLine);
            query.Append("convert(varchar(10),Birthday,120) as Birthday " + _newLine);
            query.Append("FROM dbo.Owner");

            try
            {
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                using (SqlCommand cmd = new SqlCommand(query.ToString(), myConn))
                {
                    myConn.Open();

                    SqlDataReader myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }
            catch (SqlException ex)
            {
                return new JsonResult("Database error: "+ex.Message);
            }
            return new JsonResult(table);
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
                string tmp = string.Empty;
                try
                {
                    StringBuilder query = new StringBuilder("BEGIN TRANSACTION; " + _newLine);
                    query.Append("INSERT INTO dbo.Owner ");
                    query.Append("(Name, Address, Photo, Birthday) ");
                    query.Append("VALUES " );
                    query.Append("(@Name, @Address, @Photo, @Birthday);" );
                    query.Append("COMMIT TRANSACTION;" );

                    tmp = query.ToString();
                    DataTable table = new DataTable();
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
                    return new JsonResult("Database error: " + ex.Message + " === " + tmp);
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
                    query.Append("UPDATE dbo.Owner SET " + _newLine);
                    query.Append("Name = @Name, " + _newLine);
                    query.Append("Address = @Address, " + _newLine);
                    query.Append("Photo = @Photo, " + _newLine);
                    query.Append("Birthday = @Birthday " + _newLine);
                    query.Append("WHERE IdOwner = @IdOwner; " + _newLine);
                    query.Append("COMMIT TRANSACTION;" + _newLine);


                    DataTable table = new DataTable();
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
                            sqlCommand.Parameters.AddWithValue("@IdOwner", owner.IdOwner);

                            result = sqlCommand.ExecuteNonQuery();

                            myConn.Close();
                        }
                    }
                    if (result > 0)
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



        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;

                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (System.Exception)
            {
                return new JsonResult("anon.jpg");
            }
        }
    }
}
