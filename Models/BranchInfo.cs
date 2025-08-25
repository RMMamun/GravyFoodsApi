namespace GravyFoodsApi.Models
{
    public class BranchInfo
    {
        public int Id { get; set; }
        public string BranchId { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Foreign Key
        public string CompanyId { get; set; } = string.Empty;

        // Navigation Property
        public CompanyInfo Company { get; set; }
    }

}
