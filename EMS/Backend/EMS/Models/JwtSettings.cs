namespace EMS.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryMinutes { get; set; }
        //public JwtSettings()
        //{
        //    SecretKey = "defaultSecretKey"; 
        //    Issuer = "http://localhost:5000/"; 
        //    Audience = "http://localhost:5000/";  
        //    ExpiryMinutes = 5;  
        //    RefreshTokenExpiryMinutes = 30;  
        //}

    }


}
