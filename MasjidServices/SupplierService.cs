using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.ComponentModel.Design;

namespace GravyFoodsApi.MasjidServices
{
    public class SupplierService : ISupplierRepository
    {
        private readonly MasjidDBContext _context;

        public SupplierService(MasjidDBContext context)
        {
            _context = context;
        }

        public async Task<SupplierInfo> Create(SupplierDTO supplierInfo)
        {
            try
            {


                // Implementation to add supplier info to database
                // Check if email or phone number already exists
                bool isExisted = await CheckSupplierByMobileOrEmail(supplierInfo.PhoneNo, supplierInfo.Email, supplierInfo.BranchId, supplierInfo.CompanyId);
                if (isExisted == true)
                {
                    return null;
                    //return ServiceResultWrapper<SupplierInfo>.Fail($"Supplier with '{supplierInfo.Email}' OR '{supplierInfo.PhoneNo}' already exists.");
                }

                SupplierInfo newSupplier = new SupplierInfo
                {

                    SupplierId = GenerateSupplierId(supplierInfo.CompanyId),
                    SupplierName = supplierInfo.SupplierName,
                    Address = supplierInfo.Address,
                    PhoneNo = supplierInfo.PhoneNo,
                    Email = supplierInfo.Email,
                    RepresentativeName = supplierInfo.RepresentativeName,
                    RepresentativePhone = supplierInfo.RepresentativePhone,
                    BranchId = supplierInfo.BranchId,
                    CompanyId = supplierInfo.CompanyId,

                };

                await _context.SupplierInfo.AddAsync(newSupplier);
                await _context.SaveChangesAsync();
                return newSupplier;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GenerateSupplierId(string companyCode)
        {
            string str = companyCode + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            //check if already exists, 
            var isExist = _context.SupplierInfo.Any(c => c.SupplierId == str);
            if (isExist)
            {
                //recursively call the function until a unique ID is found
                return GenerateSupplierId(companyCode);
            }
            return str;
        }

        public Task<bool> CheckSupplierByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            var isExisted = _context.SupplierInfo.Any(c => c.Email == email || c.PhoneNo == PhoneNo && (c.BranchId == branchId && c.CompanyId == companyId));
            return Task.FromResult(isExisted);
        }

        public Task<IEnumerable<SupplierInfo>?> GetAllSuppliersAsync(string branchId, string companyId)
        {
            IEnumerable<SupplierInfo>? suppliers = _context.SupplierInfo.Where(c => c.BranchId == branchId && c.CompanyId == companyId).ToImmutableList();
            return Task.FromResult(suppliers);
        }

        public Task<SupplierInfo?> GetSupplierInfoById(string Id, string branchId, string companyId)
        {
            var supplier = _context.SupplierInfo.FirstOrDefault(c => c.SupplierId == Id && (c.BranchId == branchId && c.CompanyId == companyId));
            return Task.FromResult(supplier);
        }

        public Task<SupplierInfo?> GetSupplierByMobileOrEmail(string PhoneNo, string email, string branchId, string companyId)
        {
            var supplier = _context.SupplierInfo.FirstOrDefault(c => c.Email == email || c.PhoneNo == PhoneNo && (c.BranchId == branchId && c.CompanyId == companyId));
            return Task.FromResult(supplier);
        }


        public async Task<bool> UpdateSupplierInfoAsync(SupplierDTO supplierInfo)
        {
            try
            {

                // Implementation to add supplier info to database
                // Check if email or phone number already exists
                SupplierInfo? newSupplier = await GetSupplierInfoById(supplierInfo.SupplierId, supplierInfo.BranchId, supplierInfo.CompanyId);
                if (newSupplier == null)
                {
                    return false;
                    //return await Task.FromResult("supplier not existed");
                    //return ServiceResultWrapper<SupplierInfo>.Fail($"Supplier with '{supplierInfo.Email}' OR '{supplierInfo.PhoneNo}' already exists.");
                }



                newSupplier.SupplierName = supplierInfo.SupplierName;
                newSupplier.Address = supplierInfo.Address;
                newSupplier.PhoneNo = supplierInfo.PhoneNo;
                newSupplier.Email = supplierInfo.Email;
                newSupplier.RepresentativeName = supplierInfo.RepresentativeName;
                newSupplier.RepresentativePhone = supplierInfo.RepresentativePhone;
                newSupplier.BranchId = supplierInfo.BranchId;
                newSupplier.CompanyId = supplierInfo.CompanyId;


                //_context.SupplierInfo.Update(newSupplier);
                await _context.SaveChangesAsync();
                //return newSupplier;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        public async Task<bool> DeleteAsync(string id, string branchId, string companyId)
        {
            try
            {
                var newSupplier = await this.GetSupplierInfoById(id,branchId,companyId);
                _context.SupplierInfo.Remove(newSupplier);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
    }
}
