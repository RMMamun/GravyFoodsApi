using GravyFoodsApi.Data;
using GravyFoodsApi.Models;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidServices
{
    public class AppOptionsService : IAppOptionsRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;
        public AppOptionsService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }


        public async Task<ApiResponse<IEnumerable<AppOptionDto>>> GetAppOptionsAsync(string branchId, string companyId)
        {
            branchId = _tenant.BranchId;
            companyId = _tenant.CompanyId;

            var response = new ApiResponse<IEnumerable<AppOptionDto>>();
            try
            {
                var appOptions = _context.AppOptions
                    .Where(a => a.CompanyId == companyId && a.BranchId == branchId)
                    .Select(a => new AppOptionDto
                    {
                        Id = a.Id,
                        OptionKey = a.OptionKey,
                        OptionName = a.OptionName,
                        OptionGroup = a.OptionGroup,
                        IsEnabled = a.IsEnabled,
                        HasSelector = a.HasSelector,
                        SelectedValueId = a.SelectedValueId,
                        SelectorApiUrl = a.SelectorApiUrl,
                        DisplayOrder = a.DisplayOrder,
                        IsActive = a.IsActive,
                        BranchId = "",
                        CompanyId = "",


                    }
                    ).ToList();
                response.Data = appOptions;
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

        public async Task<ApiResponse<bool>> UpdateAsync(IEnumerable<AppOptionDto> appOptionDtos)
        {

            ApiResponse<bool> apiRes = new ApiResponse<bool>();

            try
            {
                //Implement Update 
                if (appOptionDtos == null)
                {
                    throw new ArgumentNullException(nameof(appOptionDtos), "App option DTOs cannot be null.");
                }

                foreach (var dto in appOptionDtos)
                {

                    var existingOption = _context.AppOptions.FirstOrDefault(a => a.Id == dto.Id && a.BranchId == _tenant.BranchId && a.CompanyId == _tenant.CompanyId);
                    if (existingOption != null)
                    {
                        existingOption.OptionKey = dto.OptionKey;
                        existingOption.OptionName = dto.OptionName;
                        existingOption.OptionGroup = dto.OptionGroup;
                        existingOption.IsEnabled = dto.IsEnabled;
                        existingOption.HasSelector = dto.HasSelector;
                        existingOption.SelectedValueId = dto.SelectedValueId;
                        existingOption.SelectorApiUrl = dto.SelectorApiUrl;
                        existingOption.DisplayOrder = dto.DisplayOrder;
                        existingOption.IsActive = dto.IsActive;
                        //existingOption.BranchId = dto.BranchId;
                        //existingOption.CompanyId = dto.CompanyId;

                        _context.AppOptions.Update(existingOption);
                        
                    }
                    else
                    {
                        //throw new KeyNotFoundException($"App option with ID {dto.Id} not found.");
                        apiRes.Message = $"App option with ID {dto.Id} not found.";
                        apiRes.Success = false;
                        apiRes.Data = false;

                        return apiRes;
                    }
                }

                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "App options updated successfully.";
                apiRes.Data = true;

                return apiRes;

            }
            catch (Exception ex)
            {
                
                apiRes.Success = false;
                apiRes.Message = $"Error updating app options: {ex.Message}";
                apiRes.Data = false;

                return apiRes;



            }
        }
    }
}