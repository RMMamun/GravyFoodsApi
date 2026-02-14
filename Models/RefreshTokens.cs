namespace GravyFoodsApi.Models
{
    public class RefreshTokens
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = default!;

        public string TokenHash { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
