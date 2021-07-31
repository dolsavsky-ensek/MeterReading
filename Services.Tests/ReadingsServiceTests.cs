using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeterReading.Models;
using MeterReading.Repositories.Contracts;
using MeterReading.Services;
using MeterReading.Services.Contracts;
using Moq;
using NUnit.Framework;

namespace Services.Tests
{
    [TestFixture]
    public class ReadingsServiceTests
    {
        Fixture fixture = new Fixture();
        Mock<ICSVProcessingService> csvProcessingService;
        Mock<IValidationService> validationService;
        Mock<IReadingsRepo> readingsRepo;
        ReadingsService readingsService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            csvProcessingService = new Mock<ICSVProcessingService>();
            validationService = new Mock<IValidationService>();
            readingsRepo = new Mock<IReadingsRepo>();

            readingsService = new ReadingsService(csvProcessingService.Object, validationService.Object, readingsRepo.Object);
        }

        [TearDown]
        public void TearDown()
        {
            csvProcessingService.Invocations.Clear();
            validationService.Invocations.Clear();
            readingsRepo.Invocations.Clear();
        }

        #region ProcesReadings

        [Test]
        public void ProcesReadings_CSVProcessingReturnsNoData_ResultIsNull()
        {
            //Arrange
            var request = "test";
            var readings = fixture.CreateMany<MeterReadingItem>(0);

            csvProcessingService.Setup(x => x.CSVToModel(It.IsAny<string>())).Returns(readings);

            //Act
            var result = readingsService.ProcesReadings(request);

            //Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(result);

            csvProcessingService.Verify(x => x.CSVToModel(It.IsAny<string>()), Times.Once);
            validationService.Verify(x => x.ValidateReadings(It.IsAny<IEnumerable<MeterReadingItem>>()), Times.Never);
            readingsRepo.Verify(x => x.AddReadings(It.IsAny<IEnumerable<MeterReadingItem>>()), Times.Never);
        }


        [Test]
        public void ProcesReadings_ReadingsAreValidated_ResultHasCorrectReadings()
        {
            //Arrange
            var request = "test";
            var readings = fixture.CreateMany<MeterReadingItem>(3);
            var validResults = fixture.CreateMany<MeterReadingItem>(4).ToList();
            var invalidResults = fixture.CreateMany<MeterReadingItem>(2).ToList();
            var validatedReadings = fixture.Build<ReadingsResult>()
                .With(x => x.InvalidResults, invalidResults)
                .With(x => x.ValidResults, validResults)
                .Create();

            csvProcessingService.Setup(x => x.CSVToModel(It.IsAny<string>())).Returns(readings);
            validationService.Setup(x => x.ValidateReadings(It.IsAny<IEnumerable<MeterReadingItem>>())).Returns(validatedReadings);

            //Act
            var result = readingsService.ProcesReadings(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<ReadingsResult>(result, "IsInstanceOf");
                Assert.AreEqual(validResults.ToList().Count, result.ValidResults.Count, "Count");
                Assert.AreEqual(invalidResults.ToList().Count, result.InvalidResults.Count, "Count");
            });

            csvProcessingService.Verify(x => x.CSVToModel(It.IsAny<string>()), Times.Once);
            validationService.Verify(x => x.ValidateReadings(It.IsAny<IEnumerable<MeterReadingItem>>()), Times.Once);
            readingsRepo.Verify(x => x.AddReadings(It.IsAny<IEnumerable<MeterReadingItem>>()), Times.Once);
        }

        #endregion
    }
}
