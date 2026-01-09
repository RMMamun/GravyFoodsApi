using GravyFoodsApi.Data;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{

    public class AppRegistrationService
    {
        private readonly MasjidDBContext _context;

        public AppRegistrationService(MasjidDBContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<IEnumerable<AppOptionDto>>> GetAppOptionsAsync(string branchId, string companyId)
        {
            var response = new ApiResponse<IEnumerable<AppOptionDto>>();
            try
            {
                //var appOptions = _context.AppOptions
                //    .Where(a => a.CompanyId == companyId && a.BranchId == branchId)
                //    .Select(a => new AppOptionDto
                //    {
                //        Id = a.Id,
                //        OptionKey = a.OptionKey,
                //        OptionName = a.OptionName,
                //        OptionGroup = a.OptionGroup,
                //        IsEnabled = a.IsEnabled,
                //        HasSelector = a.HasSelector,
                //        SelectedValueId = a.SelectedValueId,
                //        SelectorApiUrl = a.SelectorApiUrl,
                //        DisplayOrder = a.DisplayOrder,
                //        IsActive = a.IsActive,
                //        BranchId = a.BranchId,
                //        CompanyId = a.CompanyId,


                //    }
                //    ).ToList();
                //response.Data = appOptions;
                response.Success = true;
                response.Message = "App options retrieved successfully.";

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving app options: {ex.Message}";

                return response;
            }
        }


    }
}
