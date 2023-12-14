using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // For Authorization
using booking_company.Model; // Replace with the correct namespace for your models
using booking_company.Repositories; // Replace with the correct namespace for your repositories
using Microsoft.Extensions.Configuration;
using booking_company.Services;

namespace booking_company.Controllers // Replace with the correct namespace for your controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class HouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public HouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Endpoint to list available houses based on query
        [HttpGet]
        public IActionResult GetAvailableHouses([FromQuery] QueryHouse query)
        {
            var houseRepo = new HouseRepository();
            var houses = houseRepo.GetAvailableHouses(query);
            return Ok(houses);
        }

        // Endpoint to book a house
        [HttpPost]
        public IActionResult BookHouse([FromBody] BookHouse bookingDetails)
        {
            var houseRepo = new HouseRepository();
            var clientRepo = new ClientRepository(_configuration);

            var token = TokenService.GetToken(Request);

            var client = clientRepo.GetClientByToken(token);
            if (client != null)
            {
                var response = houseRepo.BookHouse(bookingDetails, client);
                return Ok(response);
            }
            else
            {
                return Ok("Invalid Client Token !!!");
            }
        }

        // Endpoint to create a new house listing
        [HttpPost]
        [Authorize] // Authorize attribute to restrict access
        public IActionResult CreateHouse([FromBody] House houseDetails)
        {
             var houseRepo = new HouseRepository();
             var response = houseRepo.CreateHouse(houseDetails);
             return Ok(response);
        }

        // Add more actions as needed for your application
    }
}
