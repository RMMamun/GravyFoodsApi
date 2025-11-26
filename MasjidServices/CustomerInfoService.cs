using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Immutable;

namespace GravyFoodsApi.MasjidServices
{
    public class CustomerInfoService : ICustomerInfoService
    {
        private readonly MasjidDBContext _context;

        public CustomerInfoService(MasjidDBContext context)
        {
            _context = context;
        }

        public async Task<ServiceResultWrapper<CustomerInfo>> Create(CustomerInfoDTO customerInfo)
        {
            // Implementation to add customer info to database
            // Check if email or phone number already exists
            bool isExisted = await CheckCustomerByMobileOrEmail(customerInfo.PhoneNo, customerInfo.Email, customerInfo.BranchId, customerInfo.CompanyId);
            if (isExisted)
            {
                //return null;
                return ServiceResultWrapper<CustomerInfo>.Fail($"Customer with '{customerInfo.Email}' OR '{customerInfo.PhoneNo}' already exists.");
            }

            CustomerInfo newCustomer = new CustomerInfo
            {

                CustomerId = GenerateCustomerId(customerInfo.CompanyId),
                CustomerName = customerInfo.CustomerName,
                Address = customerInfo.Address,
                PhoneNo = customerInfo.PhoneNo,
                Email = customerInfo.Email,
                BranchId = customerInfo.BranchId,
                CompanyId = customerInfo.CompanyId,

            };

            await _context.CustomerInfo.AddAsync(newCustomer);
            await _context.SaveChangesAsync();
            return ServiceResultWrapper<CustomerInfo>.Ok(newCustomer);
        }

        private string GenerateCustomerId(string companyCode)
        {
            // Generate a unique CustomerId, e.g., using GUID
            return companyCode + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
        }


        public Task<bool> CheckCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            var isExisted = _context.CustomerInfo.Any(c => c.Email == email || c.PhoneNo == PhoneNo && c.BranchId == branchId && c.CompanyId == companyId);
            return Task.FromResult(isExisted);
        }

        public Task<CustomerInfo?> GetCustomerInfoById(string Id, string branchId, string companyId)
        {
            var customer = _context.CustomerInfo.FirstOrDefault(c => c.CustomerId == Id && c.BranchId == branchId && c.CompanyId == companyId);
            return Task.FromResult(customer);
        }

        public Task<CustomerInfo?> GetCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            var customer = _context.CustomerInfo.FirstOrDefault(c => c.Email == email || c.PhoneNo == PhoneNo && c.BranchId == branchId && c.CompanyId == companyId);
            return Task.FromResult(customer);
        }


        public Task<bool> UpdateCustomerInfoAsync(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerInfo>?> GetAllCustomersAsync(string branchId, string companyId)
        {
            //IEnumerable<CustomerInfo>? customer = _context.CustomerInfo.Where(c => c.BranchId == branchId && c.CompanyId == companyId).ToImmutableList();
            //return Task.FromResult(customer);

            var customers = await _context.CustomerInfo
                                .Where(c => c.BranchId == branchId && c.CompanyId == companyId)
                                .ToListAsync();

            return customers.ToImmutableList();

        }

    }
}
