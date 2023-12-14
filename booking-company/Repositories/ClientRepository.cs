using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using booking_company.Model; // Adjust to your project's namespace
using Microsoft.Extensions.Configuration;

namespace booking_company.Repositories // Adjust to your project's namespace
{
    public class ClientRepository
    {
        private readonly IConfiguration _configuration;

        public ClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Client? CreateClient(SignUp model)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            try
            {
                var client = new Client
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    ClientPassword = HashPassword(model.Password) // Assuming HashPassword is a method to hash the password
                };
                client = CreateToken(client);

                context.Clients.Add(client);
                context.SaveChanges();
                return client;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Client? GetClientByUsername(string username)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            try
            {
                var client = context.Clients.SingleOrDefault(u => u.Username == username);
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client? GetClientByToken(string token)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            try
            {
                var client = context.Clients.SingleOrDefault(u => u.Token == token);
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Client? GetClientLogin(Login login)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            try
            {
                var client = context.Clients.SingleOrDefault(u => u.Username == login.Username && u.ClientPassword == HashPassword(login.Password)); // Assuming HashPassword for password checking
                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Client CreateToken(Client client)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, client.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = GetToken(authClaims);
            client.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return client;
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1), // Adjust token expiration as needed
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        // Utility method to hash passwords (if you don't have one already)
        private string HashPassword(string password)
        {
            // Implement password hashing here
            return password; // Replace with actual hashed password
        }
    }
}
