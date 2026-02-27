using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers.Accounting
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountInfoController : Controller
    {
        private readonly IAccountInfoRepository _repo;

        public AccountInfoController(IAccountInfoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAccountByIdAsync/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var account = await _repo.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _repo.GetParentAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("GetSearchedAccountsAsync/{strSearch}")]
        public async Task<IActionResult> GetSearchedAccountsAsync(string strSearch)
        {
            var accounts = await _repo.SearchAccountsAsync(strSearch);
            return Ok(accounts);
        }

    }
}
