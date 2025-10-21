namespace GravyFoodsApi.Models.DTOs
{
    public class UserMenuAssignmentRequest
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public List<int> MenuIds { get; set; } = new();
    }

}
