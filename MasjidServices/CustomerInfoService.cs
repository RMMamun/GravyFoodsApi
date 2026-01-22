using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Immutable;

namespace GravyFoodsApi.MasjidServices
{
    public class CustomerInfoService : ICustomerInfoService
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public CustomerInfoService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<ApiResponse<CustomerInfoDTO>> Create(CustomerInfoDTO customerInfo)
        {
            ApiResponse<CustomerInfoDTO> apiRes = new();
            // Implementation to add customer info to database
            // Check if email or phone number already exists
            var isExisted = await CheckCustomerByMobileOrEmail(customerInfo.PhoneNo, customerInfo.Email, _tenant.BranchId, _tenant.CompanyId);
            if (isExisted.Data == false)
            {
                //return null;
                apiRes.Success = false;
                apiRes.Message = $"Customer with '{customerInfo.Email}' OR '{customerInfo.PhoneNo}' already exists.";
                //return ServiceResultWrapper<CustomerInfoDTO>.Fail($"Customer with '{customerInfo.Email}' OR '{customerInfo.PhoneNo}' already exists.");
                return apiRes;
            }

            CustomerInfo newCustomer = new CustomerInfo
            {

                CustomerId = GenerateCustomerId(_tenant.CompanyId),
                CustomerName = customerInfo.CustomerName,
                Address = customerInfo.Address,
                PhoneNo = customerInfo.PhoneNo,
                Email = customerInfo.Email,
                BranchId = _tenant.BranchId,
                CompanyId = _tenant.CompanyId,

            };

            await _context.CustomerInfo.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            customerInfo.CustomerId = newCustomer.CustomerId;

            //return ServiceResultWrapper<CustomerInfoDTO>.Ok(customerInfo);
            apiRes.Success = true;
            apiRes.Message = "Successfully create the customer";
            apiRes.Data = customerInfo;
            return apiRes;

        }

        private string GenerateCustomerId(string companyCode)
        {
            // Generate a unique CustomerId, e.g., using GUID
            return companyCode + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
        }


        public async Task<ApiResponse<bool>> CheckCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            ApiResponse<bool> apiRes = new();
            try
            {
                var isExisted = (await _context.CustomerInfo.AnyAsync(c => c.Email == email || c.PhoneNo == PhoneNo && c.BranchId == _tenant.BranchId && c.CompanyId == _tenant.CompanyId));

                if (isExisted == true)
                {
                    apiRes.Data = true;
                    apiRes.Message = "Customer found";
                }
                else
                {
                    apiRes.Data = false;
                    apiRes.Message = "Customer nnot found";
                }

                apiRes.Success = true;

                return apiRes;
            }
            catch (Exception ex)
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;
                return apiRes;
            }
        }

        public async Task<ApiResponse<CustomerInfoDTO?>> GetCustomerInfoById(string Id, string branchId, string companyId)
        {
            ApiResponse<CustomerInfoDTO?> apiRes = new();

            try
            {

                var customer = (await _context.CustomerInfo.FirstOrDefaultAsync(c => c.CustomerId == Id && c.BranchId == _tenant.BranchId && c.CompanyId == _tenant.CompanyId));
                if (customer == null)
                {
                    apiRes.Success = true;
                    apiRes.Message = "Customer not found";
                    apiRes.Data = null;
                    return apiRes;
                }

                CustomerInfoDTO newC =  new CustomerInfoDTO
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Address = customer.Address,
                    Email = customer.Email,
                    PhoneNo = customer.PhoneNo
                };

                apiRes.Data = newC;
                apiRes.Success = true;
                apiRes.Message = "Customer found";


                return apiRes;
                //return Task.FromResult(customer);
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return apiRes;
            }
        }

        public async Task<ApiResponse<CustomerInfoDTO?>> GetCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            ApiResponse<CustomerInfoDTO?> apiRes = new();
            try
            {
                var customer = await _context.CustomerInfo.FirstOrDefaultAsync(c => c.Email == email || c.PhoneNo == PhoneNo && c.BranchId == _tenant.BranchId && c.CompanyId == _tenant.CompanyId);
                if (customer == null)
                {
                    apiRes.Success = true;
                    apiRes.Data = null;
                    apiRes.Message = "Customer not found";

                    return apiRes;
                }

                CustomerInfoDTO Cus = new CustomerInfoDTO
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Address = customer.Address,
                    Email = customer.Email,
                    PhoneNo = customer.PhoneNo
                };

                apiRes.Success = true;
                apiRes.Data = Cus;
                apiRes.Message = "Customer found";

                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Data = null;
                apiRes.Message = ex.Message;

                return apiRes;
            }
        }


        public async Task<ApiResponse<bool>> UpdateCustomerInfoAsync(CustomerInfo dto)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();

            try
            {

                var existing = _context.CustomerInfo.FirstOrDefault(a => a.CustomerId == dto.CustomerId && a.BranchId == _tenant.BranchId && a.CompanyId == _tenant.CompanyId);
                if (existing != null)
                {

                    existing.CustomerName = dto.CustomerName;
                    existing.Address = dto.Address;
                    existing.PhoneNo = dto.PhoneNo;
                    existing.Email = dto.Email;
                    //existing.BranchId = dto.BranchId;
                    //existing.CompanyId = dto.CompanyId;

                    //_context.CustomerInfo.Update(dto);    //*** using update make an error of tracking entity. So, we are not using it.

                    await _context.SaveChangesAsync();

                    apiRes.Message = "Customer info updated successfully.";
                    apiRes.Success = true;
                    apiRes.Data = true;

                    return apiRes;

                }
                else
                {
                    //throw new KeyNotFoundException($"App option with ID {dto.Id} not found.");
                    apiRes.Message = $"Customer with ID {dto.Id} not found.";
                    apiRes.Success = false;
                    apiRes.Data = false;

                    return apiRes;
                }
            }
            catch (Exception ex)
            {
                apiRes.Message =  ex.Message;
                apiRes.Success = false;
                apiRes.Data = false;
                return apiRes;
            }

            
        }

        public async Task<ApiResponse<IEnumerable<CustomerInfoDTO>?>> GetAllCustomersAsync(string branchId, string companyId)
        {
            ApiResponse<IEnumerable<CustomerInfoDTO>?> apiRes = new();
            try
            {
                //IEnumerable<CustomerInfo>? customer = _context.CustomerInfo.Where(c => c.BranchId == branchId && c.CompanyId == companyId).ToImmutableList();
                //return Task.FromResult(customer);

                var customers = await _context.CustomerInfo
                                    .Where(c => c.BranchId == _tenant.BranchId && c.CompanyId == _tenant.CompanyId)
                                    .ToListAsync();

                if (customers.Count() == 0)
                {
                    apiRes.Success = true;
                    apiRes.Data = null;
                    apiRes.Message = "No customer found";

                    return apiRes;
                }
                IEnumerable<CustomerInfoDTO> allCus = customers.Select(p => new CustomerInfoDTO
                {
                    CompanyId = p.CompanyId,
                    BranchId = p.BranchId,
                    CustomerId = p.CustomerId,
                    CustomerName = p.CustomerName,
                    PhoneNo = p.PhoneNo,
                    Address = p.Address,
                    Email = p.Email

                });

                apiRes.Success = true;
                apiRes.Data = allCus;
                apiRes.Message = "Customer found";

                return apiRes;
            }
            catch (Exception ex) 
            {
                apiRes.Message = ex.Message;
                apiRes.Success = false;

                return apiRes;


            }

        }

        
    }
}
