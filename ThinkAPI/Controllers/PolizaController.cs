using ThinkusAPI.Utilities;
using Bogus;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace ThinkusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolizaController : ControllerBase
    {
        
        private readonly IInsuranceService _insuranceService;
        private readonly IConfiguration _configuration;
        private IConfigurationRoot configuration;

        public PolizaController(IInsuranceService insuranceService, IConfiguration configuration)
        {

            _insuranceService = insuranceService;
            _configuration = configuration;
        }

    

        [HttpPost]
        [Route("GenerateData")]
        [SwaggerOperation(Summary = "Genera una poliza ficticia y la guarda en la base de datos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Poliza generada exitosamente", typeof(Poliza))]
        public async Task<IActionResult> GenerateData()
        {
            var fakePoliza = new Faker<Poliza>()
                .RuleFor(p => p.NumeroPoliza, f => f.Random.Number(1000, 9999).ToString())
                .RuleFor(p => p.NombreCliente, f => f.Person.FullName)
                .RuleFor(p => p.IdentificacionCliente, f => f.Random.AlphaNumeric(10))
                .RuleFor(p => p.FechaNacimientoCliente, f => f.Date.Between(new DateTime(1950, 1, 1), new DateTime(2003, 12, 31)))
                .RuleFor(p => p.FechaTomaPoliza, f => f.Date.Past())
                .RuleFor(p => p.Coberturas, f => f.Lorem.Sentence())
                .RuleFor(p => p.ValorMaximoCubierto, f => f.Random.Decimal(1000, 10000))
                .RuleFor(p => p.NombrePlan, f => f.Lorem.Word())
                .RuleFor(p => p.CiudadResidenciaCliente, f => f.Address.City())
                .RuleFor(p => p.DireccionResidenciaCliente, f => f.Address.StreetAddress())
                .RuleFor(p => p.PlacaAutomotor, f => f.Random.Replace("??? ###"))
                .RuleFor(p => p.ModeloAutomotor, f => f.Vehicle.Model())
                .RuleFor(p => p.TieneInspeccion, f => f.Random.Bool())
                .RuleFor(p => p.FechaInicioVigencia, f => DateTime.Now.Date)
                .RuleFor(p => p.FechaFinVigencia, f => DateTime.Now.Date.AddDays(f.Random.Int(1, 30)));

            var newPoliza = fakePoliza.Generate();

            await _insuranceService.CreateAsync(newPoliza);

            return Ok(newPoliza);
        }

        [HttpGet]
        [Route("GenerateToken")]
        [SwaggerOperation(
            Summary = "Generate JWT token",
            Description = "Generates a JWT token for authentication",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Token generated successfully", Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<TokenResponse> GenerateToken()
        {
            try
            {
                // Código de generación de token

                // Verificar si las claves están presentes en la configuración
                if (string.IsNullOrEmpty(_configuration["Jwt:Secret"]) ||
                    string.IsNullOrEmpty(_configuration["Jwt:ValidAudience"]) ||
                    string.IsNullOrEmpty(_configuration["Jwt:ValidIssuer"]))
                {
                    throw new InvalidOperationException("Configuration keys for JWT are missing or empty.");
                }

                var claims = new[]
                {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Email, "johndoe@example.com"),
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = credentials,
                    Audience = _configuration["Jwt:ValidAudience"],
                    Issuer = _configuration["Jwt:ValidIssuer"]
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var response = new TokenResponse
                {
                    Token = tokenString
                };

                if (string.IsNullOrEmpty(tokenString))
                {
                    return StatusCode(500, "An error occurred while generating the token.");
                }

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                // Manejo de la excepción ArgumentNullException
                return BadRequest("One or more configuration keys for JWT are missing or have a null value.");
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, "An error occurred while generating the token.");
            }
        }





        [HttpGet]
        [Authorize]
        [Route("GetAllAsync")]
        [SwaggerOperation(
            Summary = "Get all polizas",
            Description = "Retrieves all the polizas",
            Tags = new[] { "Polizas" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Polizas retrieved successfully", typeof(Response<List<Poliza>>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No polizas found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving polizas", typeof(Response<List<Poliza>>))]
        public async Task<IActionResult> GetAllAsync()
        {
            Response<List<Poliza>> _response = new Response<List<Poliza>>();

            try
            {
                var polizas = await _insuranceService.GetAllAsync();

                if (polizas.Count > 0)
                {
                    var response = new Response<List<Poliza>>() { status = true, msg = "ok", value = polizas };
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception ex)
            {
                var response = new Response<List<Poliza>>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /*
        [HttpGet]
        [Authorize]
        [Route("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                Poliza poliza = await _insuranceService.GetByIdAsync(id);

                if (poliza != null)
                {
                    var response = new Response<Poliza>() { status = true, msg = "ok", value = poliza };
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception ex)
            {
                var response = new Response<Poliza>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }*/

        [HttpGet]
        [Authorize]
        [Route("GetBySpecificAsync")]
        [SwaggerOperation(
        Summary = "Get poliza by specific parameter",
        Description = "Retrieves a poliza by a specific parameter",
        Tags = new[] { "Polizas" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Poliza retrieved successfully", typeof(Response<Poliza>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "No poliza found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while retrieving poliza", typeof(Response<Poliza>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<Poliza>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Response<Poliza>))]
        public async Task<IActionResult> GetBySpecificAsync(string parametro)
        {
            try
            {
                Poliza poliza = await _insuranceService.GetBySpecificAsync(parametro);

                if (poliza != null)
                {
                    var response = new Response<Poliza>() { status = true, msg = "ok", value = poliza };
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception ex)
            {
                var response = new Response<Poliza>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("CreateAsync")]
        [SwaggerOperation(
    Summary = "Create a new poliza",
    Description = "Creates a new poliza",
    Tags = new[] { "Polizas" }
)]
        [SwaggerResponse(StatusCodes.Status200OK, "Poliza created successfully", typeof(Response<Poliza>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while creating the poliza", typeof(Response<Poliza>))]
        public async Task<IActionResult> CreateAsync([FromBody] Poliza poliza)
        {
            try
            {
                await _insuranceService.CreateAsync(poliza);
                // Ejemplo de respuesta exitosa:
                var response = new Response<Poliza>() { status = true, msg = "ok", value = poliza };
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "La póliza debe estar vigente")
                {
                    ModelState.AddModelError("fechaInicioVigencia", "La póliza debe estar vigente");
                    ModelState.AddModelError("fechaFinVigencia", "La póliza debe estar vigente");

                    var validationErrors = ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ValidationError { field = e.Key, message = e.Value.Errors.First().ErrorMessage })
                        .ToList();



                    // Construir la respuesta de validación
                    var validationResponse = new Response<Poliza>()
                    {
                        status = false,
                        msg = "Se ha presentado un error al crear la poliza puede ser alguno de los mencionados : ",
                        value = null,
                        errors = validationErrors
                    };

                    return BadRequest(validationResponse);
                }

                var response = new Response<Poliza>() { status = false, msg = ex.Message, value = null };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }




        #region generacion_de_token
        public class TokenResponse
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }
        #endregion


    }


}
