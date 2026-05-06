
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Settings;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Immutable;

namespace GravyFoodsApi.MasjidServices
{
    public class PaymentMethodService : IPaymentMethodRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public PaymentMethodService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<PaymentMethodsDto>> CreateAsync(PaymentMethodsDto _dto)
        {
            ApiResponse<PaymentMethodsDto> apiRes = new();
            // Implementation to add Method info to database
            // Check if email or phone number already exists
            var isExisted = await _context.PaymentMethods.Where(x => x.PaymentMethodName == _dto.PaymentMethodName).AnyAsync();
            if (isExisted != null)
            {
                //return null;
                apiRes.Success = false;
                apiRes.Message = $"Method with name '{_dto.PaymentMethodName}' already exists.";
                return apiRes;
            }

            PaymentMethods newEntry = new PaymentMethods
            {
                PaymentMethodCode = _dto.PaymentMethodCode,
                PaymentMethodName = _dto.PaymentMethodName,
                AccountId = _dto.AccountId,

                CompanyId = _tenant.CompanyId,

            };

            await _context.PaymentMethods.AddAsync(newEntry);
            await _context.SaveChangesAsync();

            _dto.MethodId = newEntry.MethodId;

            //return ServiceResultWrapper<PaymentMethodsDto>.Ok(_dto);
            apiRes.Success = true;
            apiRes.Message = "Successfully create the Method";
            apiRes.Data = _dto;
            return apiRes;

        }


        public async Task<ApiResponse<PaymentMethodsDto?>> GetAsync(Guid Id)
        {
            ApiResponse<PaymentMethodsDto?> apiRes = new();

            try
            {

                var Method = (await _context.PaymentMethods.FirstOrDefaultAsync(c => c.MethodId == Id && c.CompanyId == _tenant.CompanyId));
                if (Method == null)
                {
                    apiRes.Success = true;
                    apiRes.Message = "Method not found";
                    apiRes.Data = null;
                    return apiRes;
                }

                PaymentMethodsDto newC = new PaymentMethodsDto
                {
                    MethodId = Method.MethodId,
                    PaymentMethodName = Method.PaymentMethodName,
                    PaymentMethodCode = Method.PaymentMethodCode,
                    AccountId = Method.AccountId
                    
                };

                apiRes.Data = newC;
                apiRes.Success = true;
                apiRes.Message = "Method found";


                return apiRes;
                //return Task.FromResult(Method);
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return apiRes;
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(PaymentMethodsDto dto)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();

            try
            {

                var existing = _context.PaymentMethods.FirstOrDefault(a => a.MethodId == dto.MethodId && a.CompanyId == _tenant.CompanyId);
                if (existing != null)
                {

                    existing.PaymentMethodName = dto.PaymentMethodName;
                    existing.PaymentMethodCode = dto.PaymentMethodCode;
                    existing.AccountId = dto.AccountId;
                    
                    await _context.SaveChangesAsync();

                    apiRes.Message = "Method info updated successfully.";
                    apiRes.Success = true;
                    apiRes.Data = true;

                    return apiRes;

                }
                else
                {
                    //throw new KeyNotFoundException($"App option with ID {dto.Id} not found.");
                    apiRes.Message = $"Method with ID {dto.MethodId} not found.";
                    apiRes.Success = false;
                    apiRes.Data = false;

                    return apiRes;
                }
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                apiRes.Data = false;
                return apiRes;
            }


        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid Id)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();

            try
            {

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                apiRes.Data = false;
                return apiRes;
            }
        }

        
    }
}
