using BCrypt.Net;


namespace EMS.Helpers
{

    public static class AuthenticationHelper
    {
       
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyLogin(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }

}
