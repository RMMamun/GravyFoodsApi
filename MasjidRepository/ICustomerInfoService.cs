﻿using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;

namespace GravyFoodsApi.MasjidRepository
{
    public interface ICustomerInfoService 
    {
        Task<ServiceResultWrapper<CustomerInfo>> Create(CustomerInfoDTO customerInfo);
        Task<CustomerInfo?> GetCustomerInfoById(string Id,string branchId, string companyId);
        Task<CustomerInfo?> GetCustomerByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId);

        Task<bool> UpdateCustomerInfoAsync(CustomerInfo customerInfo);
        Task<IEnumerable<CustomerInfo>?> GetAllCustomersAsync(string branchId, string companyId);
    }
}
