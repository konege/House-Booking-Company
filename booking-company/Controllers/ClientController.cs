using Microsoft.AspNetCore.Mvc;
using booking_company.Model; // Replace with your actual namespace
using booking_company.Repositories; // Replace with your actual namespace

namespace booking_company.Controllers // Replace with your actual namespace
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    public class ClientController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login model)
        {
            var clientRepo = new ClientRepository(_configuration); // Assuming ClientRepository handles the client logic
            var client = clientRepo.GetClientLogin(model);
            if (client != null)
            {
                // Logic for successful login
                return Ok(client); // Consider returning a DTO instead of the client entity
            }
            else
            {
                // Logic for failed login
                return Unauthorized(); // Or any other appropriate response
            }
        }

        [HttpPost]
        public IActionResult SignUp([FromBody] SignUp model)
        {
            var clientRepo = new ClientRepository(_configuration);
            var client = clientRepo.CreateClient(model);
            return Ok(client);
        }

        // You can add more actions for other functionalities as needed
    }
}
