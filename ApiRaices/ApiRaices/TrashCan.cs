namespace ApiRaices
{
    public class TrashCan
    {


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

        /*
         * 
        //
        [Route("SaveFile")]
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
                        errorResult += "Invalid data: " + failure.ErrorMessage + _newLine;
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
                return new JsonResult("anon.jpg");
            }
        }
         */
    }
}
