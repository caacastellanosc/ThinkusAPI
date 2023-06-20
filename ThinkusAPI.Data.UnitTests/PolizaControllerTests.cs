using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinkusAPI.Controllers;
using ThinkusAPI.Utilities;
using Xunit;

namespace ThinkusAPI.Data.UnitTests
{
    public class PolizaControllerTests
    {
        private readonly PolizaController _controller;
        private readonly Mock<IInsuranceService> _insuranceServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public PolizaControllerTests()
        {
            _insuranceServiceMock = new Mock<IInsuranceService>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new PolizaController(_insuranceServiceMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task CreateAsync_InvalidPoliza_ReturnsBadRequestWithValidationErrors()
        {
            // Arrange
            var insuranceServiceMock = new Mock<IInsuranceService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new PolizaController(insuranceServiceMock.Object, configurationMock.Object);

            var invalidPoliza = new Poliza(); // Crear una instancia de Poliza inválida

            insuranceServiceMock.Setup(service => service.CreateAsync(It.IsAny<Poliza>()))
                .Throws(new Exception("La póliza debe estar vigente"));

            // Act
            var result = await controller.CreateAsync(invalidPoliza);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<Response<Poliza>>(badRequestResult.Value);
            Assert.False(response.status);
            Assert.Equal("Se ha presentado un error al crear la poliza puede ser alguno de los mencionados : ", response.msg);
            Assert.NotNull(response.errors);
            Assert.NotEmpty(response.errors);
            // Agrega más aserciones según sea necesario para validar los errores de validación

            insuranceServiceMock.Verify(service => service.CreateAsync(It.IsAny<Poliza>()), Times.Once);
        }



        [Fact]
        public async Task GetAllAsync_PolizasEncontradas_DebeDevolver200ConPolizas()
        {
            // Arrange
            var mockInsuranceService = new Mock<IInsuranceService>();
            var polizasMock = new List<Poliza>()
            {
                new Poliza { Id = "1", NumeroPoliza = "P001", NombreCliente = "John Doe" },
                new Poliza { Id = "2", NumeroPoliza = "P002", NombreCliente = "Jane Smith" }
            };
            mockInsuranceService.Setup(service => service.GetAllAsync()).ReturnsAsync(polizasMock);

            var controller = new PolizaController(mockInsuranceService.Object, null);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }



        [Fact]
        public async Task GetAllAsync_NoPolizasEncontradas_DebeDevolver204()
        {
            // Arrange
            var mockInsuranceService = new Mock<IInsuranceService>();
            var polizasMock = new List<Poliza>();
            mockInsuranceService.Setup(service => service.GetAllAsync()).ReturnsAsync(polizasMock);

            var controller = new PolizaController(mockInsuranceService.Object, null);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }












    }
}
