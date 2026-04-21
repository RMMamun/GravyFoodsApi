using GravyFoodsApi.MasjidRepository.Accounting;
using GravyFoodsApi.Models.DTOs.Accounting;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers.Accounting
{
    [ApiController]
    [Route("api/journals")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalRepository _repo;

        public JournalController(IJournalRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create(JournalInfoDto dto)
        {
            var id = await _repo.CreateAsync(dto);
            return Ok(id);
        }

        [HttpPost("{id}/post")]
        public async Task<IActionResult> Post(Guid id)
        {
            await _repo.PostAsync(id, "SYSTEM"); // replace with logged user
            return Ok();
        }

        [HttpPost("{id}/reverse")]
        public async Task<IActionResult> Reverse(Guid id)
        {
            var newId = await _repo.ReverseAsync(id, "SYSTEM");
            return Ok(newId);
        }
    }
}
