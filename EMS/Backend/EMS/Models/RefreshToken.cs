using EMS.Models;

namespace EMS.Models
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }

    }
}
