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
    public class PropertyTraceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PropertyTraceController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("{IdProperty}")]
        public JsonResult Get(int IdProperty)
        {
            string query = @"SELECT * FROM PropertyTrace WHERE IdProperty = " + IdProperty + ";";
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
                if (table == null || table.Rows.Count == 0)
                {
                    return new JsonResult("There are no transactions available for this property.");
                }
            }
            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(PropertyTrace propertyTrace)
        {
            string query = @"BEGIN TRANSACTION;
                    INSERT INTO dbo.PropertyTrace 
                    (DateSale, Name, Value, Tax, IdProperty)
                    VALUES 
                    (
                    '" + propertyTrace.DateSale + @"'
                    ,'" + propertyTrace.Name + @"'
                    ,'" + propertyTrace.Value + @"'
                    ,'" + propertyTrace.Tax + @"'
                    ,'" + propertyTrace.IdProperty + @"'
                    );
                UPDATE dbo.Property SET 
                IdOwner = '" + propertyTrace.IdOwner + @"'
                WHERE IdProperty = " + propertyTrace.IdProperty+ @"
                COMMIT TRANSACTION;";

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
            return new JsonResult("Sale has been successful.");
        }

    }
}
