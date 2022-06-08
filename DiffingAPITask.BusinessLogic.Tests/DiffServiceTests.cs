using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DiffingAPITask.BusinessLogic.DTO;
using DiffingAPITask.BusinessLogic.Services;
using DiffingAPITask.BusinessLogic.Services.Interfaces;
using DiffingAPITask.Data.Entities;
using DiffingAPITask.Data.Repositories.Interfaces;
using Moq;
using Xunit;

namespace DiffingAPITask.BusinessLogic.Tests
{
    public class DiffServiceTests
    {
        [Fact]
        public async Task ComparesTheSizesOfDataParts()
        {
            // Arrange
            var testItem = new DataItem
            {
                Id = 1,
                Left = "AAA=",
                Right = "AQABAQ=="
            };

            var mockRepo = new Mock<IDataItemRepository>();
            var mockValidationService = new Mock<IDataValidationService>();
            
            mockRepo.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(testItem);
            mockValidationService.Setup(x => x.IsBase64String(It.IsAny<string>())).Returns(true);

            var service = new DiffService(mockRepo.Object, mockValidationService.Object);

            var expectedResult = new DiffResultDTO()
            {
                DiffResultType = DiffResultDTO.ResultType.SizeDoNotMatch
            };

            // Act
            var actualResult = await service.GetDiffResult(1);
            // Assert
            Assert.Equal(expectedResult.DiffResultType, actualResult.DiffResultType);
        }

        [Fact]
        public async Task ProvidesContentDoNotMatchResultType()
        {
            // Arrange
            var testItem = new DataItem
            {
                Id = 1,
                Left = "AAAAAA==",
                Right = "AQABAQ=="
            };

            var mockRepo = new Mock<IDataItemRepository>();
            var mockValidationService = new Mock<IDataValidationService>();
            
            mockRepo.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(testItem);
            mockValidationService.Setup(x => x.IsBase64String(It.IsAny<string>())).Returns(true);

            var service = new DiffService(mockRepo.Object, mockValidationService.Object);

            var expectedResult = new DiffResultDTO()
            {
                DiffResultType = DiffResultDTO.ResultType.ContentDoNotMatch
            };

            // Act
            var actualResult = await service.GetDiffResult(1);
            // Assert
            Assert.Equal(expectedResult.DiffResultType, actualResult.DiffResultType);
        }

        [Fact]
        public async Task ShowsDiffsWithOffsetsAndLengths()
        {
            // Arrange
            var testItem = new DataItem
            {
                Id = 1,
                Left = "AAAAAA==",
                Right = "AQABAQ=="
            };

            var mockRepo = new Mock<IDataItemRepository>();
            var mockValidationService = new Mock<IDataValidationService>();
            
            mockRepo.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(testItem);
            mockValidationService.Setup(x => x.IsBase64String(It.IsAny<string>())).Returns(true);

            var service = new DiffService(mockRepo.Object, mockValidationService.Object);

            var expectedResult = new DiffResultDTO()
            {
                DiffResultType = DiffResultDTO.ResultType.ContentDoNotMatch
            };

            var diffs = new List<object>
            {
                new { offset = 0, length = 1 },
                new { offset = 2, length = 2}
            };

            expectedResult.Extensions.Add("diffs", diffs);

            // Act
            var actualResult = await service.GetDiffResult(1);
            // Assert
            var expectedJson = JsonSerializer.Serialize(expectedResult.Extensions["diffs"]);
            var actualJson = JsonSerializer.Serialize(actualResult.Extensions["diffs"]);
            
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task DeterminesIfDataPartsAreEqual()
        {
            // Arrange
            var testItem = new DataItem
            {
                Id = 1,
                Left = "AAAAAA==",
                Right = "AAAAAA=="
            };

            var mockRepo = new Mock<IDataItemRepository>();
            var mockValidationService = new Mock<IDataValidationService>();
            
            mockRepo.Setup(x => x.FindAsync(It.IsAny<int>())).ReturnsAsync(testItem);
            mockValidationService.Setup(x => x.IsBase64String(It.IsAny<string>())).Returns(true);

            var service = new DiffService(mockRepo.Object, mockValidationService.Object);

            var expectedResult = new DiffResultDTO()
            {
                DiffResultType = DiffResultDTO.ResultType.Equals
            };

            // Act
            var actualResult = await service.GetDiffResult(1);
            // Assert
            Assert.Equal(expectedResult.DiffResultType, actualResult.DiffResultType);
        }
    }
}