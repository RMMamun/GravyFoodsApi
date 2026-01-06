using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravyFoodsApi.Models.DTOs
{
    public class AppOptionDto
    {
        public int Id { get; set; }

        public string OptionKey { get; set; }
        public string OptionName { get; set; }
        public string OptionGroup { get; set; }

        public bool IsEnabled { get; set; }

        public bool HasSelector { get; set; }
        public int? SelectedValueId { get; set; }
        public string? SelectedValueText { get; set; }

        public string? SelectorApiUrl { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public string BranchId { get; set; }
        public string CompanyId { get; set; }
    }

}
