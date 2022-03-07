using ApiRaices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace ApiRaices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PropertyController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT  
                p.IdProperty, p.Name, p.Address, p.Price, 
                p.CodeInternal, p.Year, p.IdOwner, o.Name as OwnerName
                FROM dbo.Property p
                INNER JOIN dbo.Owner o 
                ON o.IdOwner = p.IdOwner;";
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
        public JsonResult Post(Property property)
        {
            //IdProperty, Name, Address, Price, CodeInternal, Year, IdOwner
            string query = @"INSERT INTO dbo.Property 
                (Name, Address, Price, CodeInternal, Year, IdOwner)
                values 
                    (
                    '" + property.Name + @"'
                    ,'" + property.Address + @"'
                    ,'" + property.Price + @"'
                    ,'" + property.CodeInternal + @"'
                    ,'" + property.Year + @"'
                    ,'" + property.IdOwner + @"'
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

        [HttpPut]
        public JsonResult Put(Property property)
        {
            //(Name, Address, Price, CodeInternal, Year, IdOwner)

            string query = @"UPDATE dbo.Property SET 
                Name = '" + property.Name + @"'
                ,Address = '" + property.Address + @"'
                ,Price = '" + property.Price + @"'
                ,CodeInternal = '" + property.CodeInternal + @"'
                ,Year = '" + property.Year + @"'
                ,IdOwner = '" + property.IdOwner + @"'
                where IdProperty = " + property.IdProperty + @"";


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
    }
}
