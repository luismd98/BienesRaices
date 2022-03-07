using ApiRaices.Controllers;
using ApiRaices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace NUnitTesting
{
    public class OwnerTest
    {
        //Unit of work => State under test => Expected behavior
        //Arrange => Act => Assert

        private  OwnerController _controller;
        private Owner _owner;

        private IConfiguration _configuration;
        private IConfiguration _dbErrorConfig;
                
        private System.Random _random;


        [SetUp]
        public void Setup()
        {
            var myConfiguration = new Dictionary<string, string>{
                {"ConnectionStrings:BienesDbCon", 
                    "Data Source=.\\SQLExpress; Initial Catalog=BienesRaicesTest; Integrated Security=True"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _random = new System.Random();
        }

        private IConfiguration ErrorConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>{
                {"ConnectionStrings:BienesDbCon",
                    "Data Source=.\\SQLExpress; Initial Catalog=AnotherDB; Integrated Security=True"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        #region -> Successful behavior
        [Test]
        [Category("Owner, Successful behavior")]
        public void Get_Owner_List_Success_Connection()
        {
            //Arrange
            _controller = new OwnerController(_configuration);

            //Act
            JsonResult result = _controller.Get();

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        [Category("Owner, Successful behavior")]
        public void Register_New_Owner_Success()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Post(_owner);

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        [Category("Owner, Successful behavior")]
        public void Update_Owner_Data_Success()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            int year = _random.Next(1905, 2003);
            int month = _random.Next(1, 12);
            int day = _random.Next(1, 28);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                IdOwner = 1,
                Name = "Updated Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = year + "-"+month+"-"+day
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        #endregion

        #region -> Database Error

        [Test]
        [Category("Owner, Database Error")]
        public void Get_Owner_List_Connection_DB_ERROR()
        {
            //Arrange
            _dbErrorConfig = ErrorConfiguration();
            _controller = new OwnerController(_dbErrorConfig);

            //Act
            JsonResult result = _controller.Get();

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        [Test]
        [Category("Owner, Database Error")]
        public void Register_New_Owner_DB_ERROR()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _dbErrorConfig = ErrorConfiguration();
            _controller = new OwnerController(_dbErrorConfig);
            _owner = new Owner
            {
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Post(_owner);

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        [Test]
        [Category("Owner, Database Error")]
        public void Update_Owner_Data_DB_ERROR()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _dbErrorConfig = ErrorConfiguration();
            _controller = new OwnerController(_dbErrorConfig);
            _owner = new Owner
            {
                IdOwner = 1,
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        #endregion

        #region -> Data validation error
        [Test]
        [Category("Owner | Post | Data validation error")]
        public void Register_New_Owner_No_Name_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Post(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        [Category("Owner | Post | Data validation error")]
        public void Register_New_Owner_No_Address_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                Name = "Test user #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Post(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }


        [Test]
        [Category("Owner | Post | Data validation error")]
        public void Register_New_Owner_No_Birthday_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId
            };

            //Act
            JsonResult result = _controller.Post(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        ///

        [Test]
        [Category("Owner | Put | Data validation error")]
        public void Update_Owner_No_Name_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                IdOwner = 1,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        [Category("Owner | Put | Data validation error")]
        public void Update_Owner_No_Address_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                IdOwner = 1,
                Name = "Test user #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-07"
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }


        [Test]
        [Category("Owner | Put | Data validation error")]
        public void Update_Owner_No_Birthday_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                IdOwner = 1,
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        [Category("Owner | Put | Data validation error")]
        public void Update_Owner_No_IdOwner_Error()
        {
            //Arrange
            int randomId = _random.Next(10, 1000);

            _controller = new OwnerController(_configuration);
            _owner = new Owner
            {
                Name = "Test user #" + randomId,
                Address = "Test address #" + randomId,
                Photo = "Test photo #" + randomId,
                Birthday = "1990-03-09"
            };

            //Act
            JsonResult result = _controller.Put(_owner);

            //Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        #endregion


    }
}