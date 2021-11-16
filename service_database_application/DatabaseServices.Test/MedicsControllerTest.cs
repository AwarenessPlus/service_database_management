using DatabaseServices.Controllers;
using DatabaseServices.Data;
using DatabaseServices.DTO;
using DatabaseServices.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace DatabaseServices.Test
{
    public class MedicsControllerTest
    {
        private readonly Mock<IMedicsServices> medicsMoq;
        private readonly MedicsController _controller;
        private readonly DatabaseServicesContext contextMoq;

        public MedicsControllerTest()
        {
            medicsMoq = new();
            var options = new DbContextOptionsBuilder<DatabaseServicesContext>()
            .UseInMemoryDatabase(databaseName: "AwarenessDatabase")
            .Options;
            contextMoq = new(options);
            _controller = new(medicsMoq.Object, contextMoq);
        }

        [Fact]
        public void OK_GetPing_Success()
        {
            ObjectResult objectResult = (ObjectResult)_controller.GetPing();
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }

        [Fact]
        public void OK_GetMedic_CorrectUserName()
        {
            MedicDTO medic = new();
            string username = "";
            medicsMoq.Setup(e => e.GetMedic(It.IsAny<string>())).Returns(medic);
            ObjectResult objectResult = (ObjectResult) _controller.GetMedic(username);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
            Assert.NotNull(objectResult);
            Assert.IsType<MedicDTO>(JsonConvert.DeserializeObject<MedicDTO>(objectResult.Value.ToString()));
        }

        [Fact]
        public void ERROR_GetMedic_WrongUserName()
        {
            MedicDTO medic = null;
            string username = "";
            medicsMoq.Setup(e => e.GetMedic(It.IsAny<string>())).Returns(medic);
            ObjectResult objectResult = (ObjectResult)_controller.GetMedic(username);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }

        [Fact]
        public async void OK_PutMedic_CorrectUserNameAndData()
        {
            MedicDTO medic = new();
            string username = "";
            medicsMoq.Setup(e => e.PutMedic(It.IsAny<string>(),It.IsAny<MedicDTO>())).ReturnsAsync("Updated");
            ObjectResult objectResult = (ObjectResult) await _controller.PutMedic(username, medic);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        } 

        [Fact]
        public async void ERROR_PutMedic_WrongRequest()
        {
            MedicDTO medic = new();
            string username = "";
            medicsMoq.Setup(e => e.PutMedic(It.IsAny<string>(), It.IsAny<MedicDTO>())).ReturnsAsync("Mismatch");
            ObjectResult objectResult = (ObjectResult)await _controller.PutMedic(username, medic);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }

        [Fact]
        public async void ERROR_PutMedic_MedicNotExist()
        {
            MedicDTO medic = new();
            string username = "";
            medicsMoq.Setup(e => e.PutMedic(It.IsAny<string>(), It.IsAny<MedicDTO>())).ReturnsAsync("NotFound");
            ObjectResult objectResult = (ObjectResult)await _controller.PutMedic(username, medic);
            Assert.IsType<NotFoundObjectResult>(objectResult as NotFoundObjectResult);
        }
    }
}
