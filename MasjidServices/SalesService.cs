
using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace GravyFoodsApi.MasjidServices
{

    public class SalesService : ISalesService
    {
        private readonly MasjidDBContext _context;

        public SalesService(MasjidDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesInfo>> GetAllSalesAsync()
        {
            return await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .Include(s => s.CustomerInfo)
                .ToListAsync();
        }

        public async Task<SalesInfo?> GetSaleByIdAsync(string salesId)
        {
            return await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .Include(s => s.CustomerInfo)
                .FirstOrDefaultAsync(s => s.SalesId == salesId);
        }

        public async Task<SalesInfo> CreateSaleAsync(SalesInfo sale)
        {
            _context.SalesInfo.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<SalesInfo?> UpdateSaleAsync(string salesId, SalesInfo sale)
        {
            var existing = await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .FirstOrDefaultAsync(s => s.SalesId == salesId);

            if (existing == null)
                return null;

            // Update master fields
            _context.Entry(existing).CurrentValues.SetValues(sale);

            // Replace details if provided
            if (sale.SalesDetails?.Any() == true)
            {
                _context.SalesDetails.RemoveRange(existing.SalesDetails);
                existing.SalesDetails = sale.SalesDetails;
            }

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteSaleAsync(string salesId)
        {
            var existing = await _context.SalesInfo
                .Include(s => s.SalesDetails)
                .FirstOrDefaultAsync(s => s.SalesId == salesId);

            if (existing == null)
                return false;

            _context.SalesDetails.RemoveRange(existing.SalesDetails);
            _context.SalesInfo.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
