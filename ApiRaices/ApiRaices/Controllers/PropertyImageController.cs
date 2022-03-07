using ApiRaices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly  IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PropertyImageController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("{IdProperty}")]
        public JsonResult Get( int IdProperty)
        {
            string query = @"SELECT Photo, IdPropertyImage
                FROM dbo.PropertyImage
                WHERE IdProperty =" + IdProperty + @" 
                AND Enabled = 1;";


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

        /*
        [HttpPost]
        public JsonResult Post(PropertyImage propertyImage)
        {
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
            return new JsonResult("Added successfully");
        }
        */

        [HttpPut]
        public JsonResult Put(PropertyImage PropertyImage)
        {
            string query = @"UPDATE dbo.PropertyImage SET 
                Enabled = 0
                WHERE IdPropertyImage = " + PropertyImage.IdPropertyImage + ";";


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

        /*
        public static bool RegisterImage(PropertyImage propertyImage, IConfiguration config)
        {
            string query = @"INSERT INTO dbo.PropertyImage 
                (IdProperty, Photo, Enabled)
                values 
                    (
                    '" + propertyImage.IdProperty + @"'
                    ,'" + propertyImage.Photo + @"'
                    ,'" + propertyImage.Enabled + @"'
                    )";

            DataTable table = new DataTable();


            string sqlDataSource = config.GetConnectionString("BienesDbCon");
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
            return true;
        }
        */
    }
}
