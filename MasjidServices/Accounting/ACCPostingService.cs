using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.Accounting;
using GravyFoodsApi.Models.DTOs;
using GravyFoodsApi.Models.DTOs.Accounting;

namespace GravyFoodsApi.MasjidServices.Accounting
{


    public class ACCPostingService : IACCPostingRepository
    {

        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;
        public ACCPostingService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public Task<ApiResponse<bool>> PostJournal(JournalInfoDto journal)
        {
            throw new NotImplementedException();
        }

        //public async Task<ApiResponse<bool>> PostJournal(JournalInfoDto journal)
        //{
        //    try
        //    {

        //        if (journal.IsPosted)
        //            throw new Exception("Already posted");

        //        var debit = journal.Lines.Sum(x => x.Debit);
        //        var credit = journal.Lines.Sum(x => x.Credit);

        //        if (debit != credit)
        //            throw new Exception("Journal not balanced");


        //        JournalInfo newEntry = new JournalInfo
        //        {
        //            Date = journal.Date,
        //            Description = journal.Description,
        //            ReferenceNo = journal.ReferenceNo,
        //            SourceModule = journal.SourceModule,
        //            IsPosted = true,
        //            PostedAt = DateTime.UtcNow,
        //            PostedBy = _tenant.UserId,
        //            JournalDetails = journal.Lines.Select(x => new JournalDetails
        //            {
        //                ACCode = x.ACCode,
        //                Debit = x.Debit,
        //                Credit = x.Credit,
        //                Description = x.Description
        //            }).ToList()
        //        };


        //        _context.JournalInfo.Add(newEntry);

        //        await _context.SaveChangesAsync();
        //        return new ApiResponse<bool>
        //        {
        //            Success = true,
        //            Message = "Journal posted successfully",
        //            Data = true
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<bool>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //            Data = false
        //        };
        //    }
        //}
    }
}
