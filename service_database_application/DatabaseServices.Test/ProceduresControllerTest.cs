using DatabaseServices.Controllers;
using DatabaseServices.Data;
using DatabaseServices.DTO;
using DatabaseServices.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Xunit;

namespace DatabaseServices.Test
{
    public class ProceduresControllerTest
    {
        private readonly Mock<IProceduresServices> proceduresMoq;
        private readonly ProceduresController _controller;
        private readonly DatabaseServicesContext contextMoq;
        private readonly Mock<IWebHostEnvironment> environmentMoq;
        private readonly Mock<IFileSystem> _fileSystem;

        public ProceduresControllerTest()
        {
            proceduresMoq = new();
            var options = new DbContextOptionsBuilder<DatabaseServicesContext>()
             .UseInMemoryDatabase(databaseName: "AwarenessDatabase")
             .Options;
            contextMoq = new(options);
            environmentMoq = new();
            _controller = new(proceduresMoq.Object, contextMoq, environmentMoq.Object);
        }

        [Fact]
        public void OK_GetPing_Success()
        {
            ObjectResult objectResult = (ObjectResult)_controller.GetPing();
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }
        [Fact]
        public async void OK_GetProceduresByMedic_MedicHasProcedures()
        {
            List<ProcedureDTO> list = new();
            int medicID = 0;
            proceduresMoq.Setup(e => e.GetProceduresByMedic(It.IsAny<int>())).ReturnsAsync(list);
            ObjectResult objectResult = (ObjectResult)await _controller.GetProceduresByMedic(medicID);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
            Assert.NotNull(objectResult);
            Assert.IsType<List<ProcedureDTO>>(JsonConvert.DeserializeObject<List<ProcedureDTO>>(objectResult.Value.ToString()));
        }
        [Fact]
        public async void ERROR_GetProceduresByMedic_MedicHasNotProcedures()
        {
            List<ProcedureDTO> list = null;
            int medicID = 0;
            proceduresMoq.Setup(e => e.GetProceduresByMedic(It.IsAny<int>())).ReturnsAsync(list);
            ObjectResult objectResult = (ObjectResult)await _controller.GetProceduresByMedic(medicID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }

        [Fact]
        public async void OK_PostProcedure_ProcedureDataOk()
        {
            ProcedureDTO procedure = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.PostProcedure(It.IsAny<ProcedureDTO>())).ReturnsAsync(procedureID);
            ObjectResult objectResult = (ObjectResult)await _controller.PostProcedure(procedure);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
            Assert.NotNull(objectResult.Value);
        }

        [Fact]
        public async void OK_DeleteProcedure_ProcedureDeleted()
        {
            int procedureID = 0;
            proceduresMoq.Setup(e => e.DeleteProcedure(It.IsAny<int>())).ReturnsAsync(true);
            ObjectResult objectResult = (ObjectResult)await _controller.DeleteProcedure(procedureID);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }
        [Fact]
        public async void ERROR_DeleteProcedure_ProcedureNotExist()
        {
            int procedureID = 0;
            proceduresMoq.Setup(e => e.DeleteProcedure(It.IsAny<int>())).ReturnsAsync(false);
            ObjectResult objectResult = (ObjectResult)await _controller.DeleteProcedure(procedureID);
            Assert.IsType<NotFoundObjectResult>(objectResult as NotFoundObjectResult);
        }
        [Fact]
        public async void OK_UploadVideo_VideoLoadedSuccesfully()
        {
            Mock<IFormFile> fileMock = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.UploadVideo(It.IsAny<IFormFile>(), It.IsAny<int>())).ReturnsAsync("Done");
            ObjectResult objectResult = (ObjectResult)await _controller.PostVideoFile(fileMock.Object, procedureID);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }
        [Fact]
        public async void ERROR_UploadVideo_EmptyRequest()
        {
            Mock<IFormFile> fileMock = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.UploadVideo(It.IsAny<IFormFile>(), It.IsAny<int>())).ReturnsAsync("NoFileDataInRequest");
            ObjectResult objectResult = (ObjectResult)await _controller.PostVideoFile(fileMock.Object, procedureID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
        [Fact]
        public async void ERROR_UploadVideo_FormatNotSupported()
        {
            Mock<IFormFile> fileMock = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.UploadVideo(It.IsAny<IFormFile>(), It.IsAny<int>())).ReturnsAsync("WrongFileFormat");
            ObjectResult objectResult = (ObjectResult)await _controller.PostVideoFile(fileMock.Object, procedureID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
        [Fact]
        public async void ERROR_UploadVideo_ProcedureNotExist()
        {
            Mock<IFormFile> fileMock = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.UploadVideo(It.IsAny<IFormFile>(), It.IsAny<int>())).ReturnsAsync("ProcedureNotExist");
            ObjectResult objectResult = (ObjectResult)await _controller.PostVideoFile(fileMock.Object, procedureID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
        [Fact]
        public async void ERROR_UploadVideo_ProcedureHasAVideoAlready()
        {
            Mock<IFormFile> fileMock = new();
            int procedureID = 0;
            proceduresMoq.Setup(e => e.UploadVideo(It.IsAny<IFormFile>(), It.IsAny<int>())).ReturnsAsync("VideoAlreadyUploaded");
            ObjectResult objectResult = (ObjectResult)await _controller.PostVideoFile(fileMock.Object, procedureID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
    }
}
