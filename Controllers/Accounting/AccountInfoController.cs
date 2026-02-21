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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
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
    }
}
