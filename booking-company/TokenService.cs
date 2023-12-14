namespace booking_company.Services
{
    public class TokenService
    {
        public static string? GetToken(HttpRequest Request)
        {
            if (Request != null)
            {
                string? Token = Request.Headers["Authorization"];

                return Token.Replace("Bearer", "").Trim();
            }
            return null;

        }
    }
}


