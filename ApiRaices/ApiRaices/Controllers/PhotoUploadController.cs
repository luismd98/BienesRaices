using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using ApiRaices.Models;
using System.Data.SqlClient;
using System.Data;

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

                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
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

                int ReceivedIdProperty = int.Parse(Request.Form["IdProperty"]);


                PropertyImage propertyImage = new PropertyImage();
                propertyImage.IdProperty = ReceivedIdProperty;
                propertyImage.Photo = fileName;
                propertyImage.Enabled = true;


                string query = @"INSERT INTO dbo.PropertyImage 
                (IdProperty, Photo, Enabled)
                values 
                    (
                    '" + propertyImage.IdProperty + @"'
                    ,'" + propertyImage.Photo + @"'
                    ,'" + propertyImage.Enabled + @"'
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

                var physicalPath = _env.ContentRootPath + "/Photos/property/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult("Picture uploaded.");
            }
            catch (System.Exception ex)
            {
                return new JsonResult("There was an error uploading the picture.\r\n"+ex.Message);
            }
        }
    }
}
