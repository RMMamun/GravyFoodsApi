using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs.Accounting;
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


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountInfoDto accountInfoDto)
        {
            if (accountInfoDto == null)
                return BadRequest("Account information is required.");
            var createdAccount = await _repo.CreateAccountAsync(accountInfoDto);

            return Ok(createdAccount);

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AccountInfoDto accountInfoDto)
        {
            if (accountInfoDto == null || string.IsNullOrEmpty(accountInfoDto.Id))
                return BadRequest("Valid account information with ID is required.");

            var updatedAccount = await _repo.UpdateAccountAsync(accountInfoDto);
            return Ok(updatedAccount);
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
            var accounts = await _repo.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("GetParentAccountsAsync")]
        public async Task<IActionResult> GetParentAccountsAsync()
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
