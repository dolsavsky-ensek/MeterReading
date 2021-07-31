using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using MeterReading.Controllers;
using MeterReading.Models;
using MeterReading.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Controllers.Tests
{
    [TestFixture]
    public class MeterReadingUploadsControllerTests
    {
        Fixture fixture = new Fixture();
        Mock<IReadingsService> readingsService;
        Mock<ILogger<MeterReadingUploadsController>> logger;
        MeterReadingUploadsController meterReadingUploadsController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            readingsService = new Mock<IReadingsService>();
            logger = new Mock<ILogger<MeterReadingUploadsController>>();

            meterReadingUploadsController = new MeterReadingUploadsController(readingsService.Object, logger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            readingsService.Invocations.Clear();
        }

        #region Post

        [Test]
        public async Task Post_NoFile_ResultIsBadRequest()
        {
            //Arrange
            var csv = "";
            SetUpController(csv);

            //Act
            var result = await meterReadingUploadsController.Post();
            
            //Assert
            readingsService.Verify(x => x.ProcesReadings(It.IsAny<string>()), Times.Never);
            
            Assert.IsInstanceOf <BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Post_ReadingsServiceReturnsNull_ResultIsBadRequest()
        {
            //Arrange
            var csv = "test";
            SetUpController(csv);

            readingsService.Setup(x => x.ProcesReadings(It.IsAny<string>())).Returns(() => null);

            //Act
            var result = await meterReadingUploadsController.Post();

            //Assert
            readingsService.Verify(x => x.ProcesReadings(It.IsAny<string>()), Times.Once);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Post_ReadingServiceReturnsReadings_ResultIsOK()
        {
            //Arrange
            var csv = "test";
            var readingsResult = fixture.Create<ReadingsResult>();
            SetUpController(csv);

            readingsService.Setup(x => x.ProcesReadings(It.IsAny<string>())).Returns(readingsResult);

            //Act
            var result = await meterReadingUploadsController.Post();
            var okResult = result as OkObjectResult;

            //Assert
            readingsService.Verify(x => x.ProcesReadings(It.IsAny<string>()), Times.Once);

            Assert.Multiple(() => {
                Assert.IsNotNull(okResult);
                Assert.AreEqual(200, okResult.StatusCode);
            });
        }

        #endregion

        #region PrivateMethods

        private void SetUpController(string csv)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var httpContext = new DefaultHttpContext()
            {
                Request = { Body = stream, ContentLength = stream.Length }
            };
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            meterReadingUploadsController = new MeterReadingUploadsController(readingsService.Object, logger.Object) { ControllerContext = controllerContext };
        }

        #endregion
    }
}
