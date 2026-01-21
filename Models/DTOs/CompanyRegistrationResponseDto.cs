namespace GravyFoodsApi.Models.DTOs
{
    public class CompanyRegistrationResponseDto
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        public List<BranchesDto> Branches { get; set; } = new();
    }

    public class BranchesDto
    {
        public string BranchId { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
    }

}
