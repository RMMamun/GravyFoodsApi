using GravyFoodsApi.MasjidRepository.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers.Accounting
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingReportsController : Controller
    {
        private readonly IAccountingReportRepository _repo;

        public AccountingReportsController(IAccountingReportRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("GetLedgerRptAsync/{accCode}/{fromDate}/{toDate}")]
        public async Task<IActionResult> Get(string accCode, DateTime fromDate, DateTime toDate)
        {
            var account = await _repo.GetLedger(accCode,fromDate,toDate);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
    }
}
