using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GravyFoodsApi.MasjidServices.Accounting
{
    public class JournalService : IJournalRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;

        public JournalService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        // CREATE JOURNAL
        public async Task<ApiResponse<Guid>> CreateAsync(JournalInfoDto dto)
        {
            ApiResponse<Guid> apiRes = new();

            try
            {
                if (dto.Details == null || !dto.Details.Any())
                {
                    //throw new Exception("Journal must have at least one line");
                    apiRes.Success = false;
                    apiRes.Message = "Journal must have at least one line";
                    return apiRes;

                }


                var totalDebit = dto.Details.Sum(x => x.Debit);
                var totalCredit = dto.Details.Sum(x => x.Credit);

                if (totalDebit != totalCredit)
                {
                    //throw new Exception("Journal not balanced");
                    apiRes.Success = false;
                    apiRes.Message = "Journal not balanced";
                    return apiRes;

                }
                

                var journal = new JournalInfo
                {
                    Id = Guid.NewGuid(),
                    CompanyId = _tenant.CompanyId,
                    BranchId = _tenant.BranchId,

                    Date = dto.Date,
                    ReferenceNo = dto.ReferenceNo,
                    SourceModule = dto.SourceModule,
                    Description = dto.Description,
                    IsPosted = false
                };

                journal.JournalDetails = dto.Details.Select(x => new JournalDetails
                {
                    Id = Guid.NewGuid(),
                    CompanyId = _tenant.CompanyId,
                    BranchId = _tenant.BranchId,

                    
                    JournalId = journal.Id,
                    ACCode = x.ACCode,
                    Description = x.Description,
                    Debit = x.Debit,
                    Credit = x.Credit
                }).ToList();

                _context.JournalInfo.Add(journal);
                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Message = "Journal saved successfully";
                apiRes.Data = journal.Id;
                return apiRes;
            }
            catch(Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Journal save failed with error: " + ex.Message;
                return apiRes;
            }
        }

        // POST JOURNAL
        public async Task<ApiResponse<bool>> PostAsync(Guid journalId, string user)
        {
            ApiResponse<bool> apiRes = new();

            try
            {
                var journal = await _context.JournalInfo
                .Include(x => x.JournalDetails)
                .FirstOrDefaultAsync(x => x.Id == journalId);

                if (journal == null)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Journal not found";
                    return apiRes;
                }


                if (journal.IsPosted)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Already posted";
                    return apiRes;
                }
                var totalDebit = journal.JournalDetails.Sum(x => x.Debit);
                var totalCredit = journal.JournalDetails.Sum(x => x.Credit);

                if (totalDebit != totalCredit)
                {
                    apiRes.Success = false;
                    apiRes.Message = "Journal not balanced";
                    return apiRes;
                }

                // OPTIONAL: prevent control account posting
                var controlAccounts = await _context.AccountInfo
                    .Where(a => a.IsControlAccount)
                    .Select(a => a.ACCode)
                    .ToListAsync();

                if (journal.JournalDetails.Any(x => controlAccounts.Contains(x.ACCode)))
                {
                    apiRes.Success = false;
                    apiRes.Message = "Cannot post to control account";
                    return apiRes;
                }

                journal.IsPosted = true;
                journal.PostedBy = user;
                journal.PostedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                apiRes.Success = true;
                apiRes.Data = true;
                apiRes.Message = "Journal posted successfully.";
                return apiRes;

            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Journal save failed with error: " + ex.Message;
                return apiRes;
            }
        }

        // REVERSE JOURNAL
        public async Task<ApiResponse<Guid>> ReverseAsync(Guid journalId, string user)
        {
            ApiResponse<Guid> apiRes = new();

            try
            {


                var original = await _context.JournalInfo
                    .Include(x => x.JournalDetails)
                    .FirstOrDefaultAsync(x => x.Id == journalId);

                if (original == null)
                {
                    //throw new Exception("Journal not found");
                    apiRes.Message = "Journal not found";
                    apiRes.Success = false;
                    return apiRes;
                }

                var reversal = new JournalInfo
                {
                    Id = Guid.NewGuid(),
                    CompanyId = original.CompanyId,
                    BranchId = original.BranchId,

                    Date = DateTime.UtcNow,
                    ReferenceNo = "REV-" + original.ReferenceNo,
                    SourceModule = original.SourceModule,
                    Description = "Reversal of " + original.ReferenceNo,
                    IsPosted = false
                };

                reversal.JournalDetails = original.JournalDetails.Select(x => new JournalDetails
                {
                    Id = Guid.NewGuid(),
                    CompanyId = x.CompanyId,
                    BranchId = x.BranchId,

                    JournalId = reversal.Id,
                    ACCode = x.ACCode,
                    Debit = x.Credit,   // 🔁 swap
                    Credit = x.Debit
                }).ToList();

                _context.JournalInfo.Add(reversal);
                await _context.SaveChangesAsync();



                apiRes.Message = "Reverse Journal saved.";
                apiRes.Success = true;
                apiRes.Data = reversal.Id;
                return apiRes;


            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = "Journal save failed with error: " + ex.Message;
                return apiRes;
            }

        }
    }
}
