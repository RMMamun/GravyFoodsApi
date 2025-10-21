namespace GravyFoodsApi.Models.DTOs
{
    public class NavMenuItemDto
    {

        public int MenuId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? IconCss { get; set; }
        public string? IconImagePath { get; set; }
        public bool IsSeparator { get; set; }
        public string BranchId { get; set; }
        public string CompanyId { get; set; }

        public List<NavMenuItemDto> Children { get; set; } = new();
    }
}
