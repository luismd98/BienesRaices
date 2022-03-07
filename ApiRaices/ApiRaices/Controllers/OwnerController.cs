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

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public OwnerController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select IdOwner, Name, Address, Photo, convert(varchar(10),Birthday,120) as Birthday from dbo.Owner";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myConn))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
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
                    result += "La propiedad '" + failure.PropertyName + "' es inválida: " + failure.ErrorMessage + Environment.NewLine;
                }
                return new JsonResult(result);
            }
            else
            {
                string query = @"INSERT INTO dbo.Owner 
                (Name, Address, Photo, Birthday)
                values 
                    (
                    '" + owner.Name + @"'
                    ,'" + owner.Address + @"'
                    ,'" + owner.Photo + @"'
                    ,'" + owner.Birthday + @"'
                    )";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
                SqlDataReader myReader;
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, myConn))
                    {
                        myReader = sqlCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myConn.Close();
                    }
                }
            }

            return new JsonResult("New Owner registered successfully.");
        }

        [HttpPut]
        public JsonResult Put(Owner owner)
        {

            string query = @"UPDATE dbo.Owner SET 
                Name = '" + owner.Name + @"'
                ,Address = '" + owner.Address + @"'
                ,Photo = '" + owner.Photo + @"'
                ,Birthday = '" + owner.Birthday + @"'
                where IdOwner = " + owner.IdOwner + @"";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BienesDbCon");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myConn))
                {
                    myReader = sqlCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult("Updated successfully");
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
