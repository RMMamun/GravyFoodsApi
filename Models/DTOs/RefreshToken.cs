namespace GravyFoodsApi.Models.DTOs
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public UserInfo UserInfo { get; set; } = default!;

        public string TokenHash { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
