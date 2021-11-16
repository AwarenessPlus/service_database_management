using DatabaseServices.Controllers;
using DatabaseServices.Data;
using DatabaseServices.DTO;
using DatabaseServices.Services;
using DomainModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace DatabaseServices.Test
{
    public class PatientsControllerTest
    { 
        private readonly Mock<IPatientsServices> patientsMoq;
        private readonly PatientsController _controller;
        private readonly DatabaseServicesContext contextMoq;

        public PatientsControllerTest()
        {
            patientsMoq = new();
            var options = new DbContextOptionsBuilder<DatabaseServicesContext>()
            .UseInMemoryDatabase(databaseName: "AwarenessDatabase")
            .Options;
            contextMoq = new(options);
            _controller = new(patientsMoq.Object, contextMoq);
        }

        [Fact]
        public void OK_GetPing_Success()
        {
            ObjectResult objectResult = (ObjectResult)_controller.GetPing();
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
        }

        [Fact]
        public async void OK_GetPatient_CorrectDocumentID()
        {
            PatientDTO patient = new();
            int documentID = 0;
            patientsMoq.Setup(e => e.GetPatient(It.IsAny<int>())).ReturnsAsync(patient);
            ObjectResult objectResult =(ObjectResult) await _controller.GetPatient(documentID);
            Assert.IsType<OkObjectResult>(objectResult as OkObjectResult);
            
        }
        [Fact]
        public async void ERROR_GetPatient_WrongDocumentID()
        {
            PatientDTO patient = null;
            int documentID = 0;
            patientsMoq.Setup(e => e.GetPatient(It.IsAny<int>())).ReturnsAsync(patient);
            ObjectResult objectResult = (ObjectResult)await _controller.GetPatient(documentID);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
        [Fact]
        public async void OK_PostPatient_CorrectPatientData()
        {
            Patient patientDB = new();
            PatientDTO patient = new();
            patientsMoq.Setup(e => e.PostPatient(It.IsAny<PatientDTO>())).ReturnsAsync(patientDB);
            ObjectResult objectResult = (ObjectResult)await _controller.PostPatient(patient);
            Assert.IsType<CreatedAtActionResult>(objectResult as CreatedAtActionResult);
            Assert.NotNull(objectResult);
        }
        [Fact]
        public async void ERROR_PostPatient_WrongPatientData()
        {
            Patient patientDB = null;
            PatientDTO patient = new();
            patientsMoq.Setup(e => e.PostPatient(It.IsAny<PatientDTO>())).ReturnsAsync(patientDB);
            ObjectResult objectResult = (ObjectResult)await _controller.PostPatient(patient);
            Assert.IsType<BadRequestObjectResult>(objectResult as BadRequestObjectResult);
        }
    }
}
