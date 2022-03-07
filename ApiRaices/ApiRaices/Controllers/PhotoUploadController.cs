using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using ApiRaices.Models;
using System.Data.SqlClient;
using System.Data;
using ApiRaices.Models.Validations;
using FluentValidation.Results;
using FluentValidation;
using System.Text;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    public class PhotoUploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PhotoUploadController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;

                FileValidator validator = new FileValidator();
                ValidationResult validationResults = validator.Validate(postedFile);

                if (!validationResults.IsValid)
                {
                    string errorResult = string.Empty;
                    foreach (var failure in validationResults.Errors)
                    {
                        errorResult += "Invalid data: " + failure.ErrorMessage + System.Environment.NewLine;
                    }
                    return new JsonResult(errorResult);
                }
                else
                {
                    var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    return new JsonResult(fileName);
                }
            }
            catch (System.Exception)
            {
                return new JsonResult("anon.png");
            }
        }

        [Route("SavePropertyFile")]
        [HttpPost]
        public JsonResult SavePropertyFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;

                FileValidator fileValidator = new FileValidator();
                ValidationResult fileValidationResults = fileValidator.Validate(postedFile);


                if (!fileValidationResults.IsValid)
                {
                    string errorResult = string.Empty;
                    foreach (var failure in fileValidationResults.Errors)
                    {
                        errorResult += "Invalid data: " + failure.ErrorMessage;
                    }
                    return new JsonResult(errorResult);
                }
                else
                {   
                    int ReceivedIdProperty = int.Parse(Request.Form["IdProperty"]);

                    PropertyImage propertyImage = new PropertyImage();
                    propertyImage.IdProperty = ReceivedIdProperty;
                    propertyImage.Photo = fileName;
                    propertyImage.Enabled = true;

                    PropertyImageValidator propImageValidator = new PropertyImageValidator();
                    ValidationResult propImageValidationResults = propImageValidator.Validate(propertyImage);


                    if (!propImageValidationResults.IsValid)
                    {
                        string errorResult = string.Empty;
                        foreach (var failure in fileValidationResults.Errors)
                        {
                            errorResult += "Invalid data: " + failure.ErrorMessage;
                        }
                        return new JsonResult(errorResult);
                    }
                    else
                    {
                        StringBuilder stringQuery = new StringBuilder("BEGIN TRANSACTION; ");
                        stringQuery.Append("INSERT INTO dbo.PropertyImage ");
                        stringQuery.Append("(IdProperty, Photo, Enabled) ");
                        stringQuery.Append("VALUES ");
                        stringQuery.Append("(@IdProperty, @Photo, @Enabled) ");
                        stringQuery.Append("COMMIT TRANSACTION;");

                        try
                        {
                            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                            int queryResult;

                            using (SqlConnection sqlConnection = new SqlConnection(sqlDataSource))
                            {
                                sqlConnection.Open();
                                using (SqlCommand sqlCommand = new SqlCommand(stringQuery.ToString(), sqlConnection))
                                {
                                    sqlCommand.Parameters.AddWithValue("@IdProperty", propertyImage.IdProperty);
                                    sqlCommand.Parameters.AddWithValue("@Photo", propertyImage.Photo);
                                    sqlCommand.Parameters.AddWithValue("@Enabled", propertyImage.Enabled);

                                    queryResult = sqlCommand.ExecuteNonQuery();

                                    sqlConnection.Close();
                                }
                            }
                            if (queryResult <= 0)
                            {
                                return new JsonResult("Picture could not be uploaded.");
                            }
                        }
                        catch (SqlException ex)
                        {
                            return new JsonResult("Database error.\r\n" + ex.Message);
                        }                        

                        var physicalPath = _env.ContentRootPath + "/Photos/property/" + fileName;

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                        }

                        return new JsonResult("Picture uploaded.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                return new JsonResult("There was an error uploading the picture.\r\n"+ex.Message);
            }
        }
    }
}
