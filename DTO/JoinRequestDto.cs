namespace GravyFoodsApi.DTO
{
    public class JoinRequestDto
    {
        public required string PlayerId { get; set; }
        public string? JoinRequestCode { get; set; }
    }
}
