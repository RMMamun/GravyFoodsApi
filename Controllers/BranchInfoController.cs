using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchInfoController : Controller
    {
        private readonly IBranchInfoRepository _repository;

        public BranchInfoController(IBranchInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{branchId}/{companyId}")]
        public async Task<IActionResult> GetBranchById(string branchId, string companyId)
        {
            var product = await _repository.GetBranchInfoAsync(branchId, companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetAllBranchesAsync(string companyId)
        {
            var product = await _repository.GetAllBranchesAsync(companyId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] BranchInfoDto branch)
        {
            /*****************************************************************************************
                Branch will create by Client Subscription process. Or can create till his limit
            *****************************************************************************************/

            //var created = await _repo.Create(branch);
            //return CreatedAtAction(nameof(Get), new { id = created.Id }, created);

            var branchExists = await _repository.GetBranchInfoAsync(branch.BranchId, branch.CompanyId);
            if (branchExists != null)
            {
                return Ok(branchExists);
            }


            var result = await _repository.CreateBranchInfoAsync(branch);
            return Ok(result);

        }


        [HttpPut]
        public async Task<ActionResult<bool>> Update(BranchInfoDto branch)
        {
            var branchExists = await _repository.GetBranchInfoAsync(branch.BranchId, branch.CompanyId);
            if (branchExists == null)
            {
                return Ok(branchExists);
            }


            var result = await _repository.UpdateBranchInfoAsync(branch);
            return Ok(result);
        }

    }
}
