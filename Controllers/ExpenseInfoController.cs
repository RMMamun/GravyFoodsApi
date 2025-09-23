using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseInfoController : Controller
    {

        private readonly IExpenseInfoService _repository;

        public ExpenseInfoController(IExpenseInfoService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseInfoDto>> Create([FromBody] ExpenseInfoDto expenseHead)
        {
            try
            {
                var created = await _repository.CreateAsync(expenseHead);
                return Ok(created);
                //return CreatedAtAction(nameof(GetExpenseHeadById), new { id = created.Data.Id, branchId = created.Data.BranchId, companyId = created.Data.CompanyId }, created.Data);

            }
            catch (Exception ex)
            {
                //return BadRequest("Not found");
                return BadRequest(ex);
            }

        }


        [HttpGet("{id:int}/{branchId}/{companyId}")]
        public async Task<ActionResult<ExpenseInfoDto>> GetExpenseInfoByIdAsync(int id, string branchId, string companyId)
        {
            try
            {


                var expenseHead = await _repository.GetExpenseInfoById(id, branchId, companyId);
                if (expenseHead == null)
                {
                    return NotFound();
                }
                return Ok(expenseHead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<ExpenseInfoDto>>> GetAllExpenseInfoAsync(string branchId, string companyId)
        {
            try
            {
                var expenseHead = await _repository.GetAllExpenseInfoAsync(branchId, companyId);
                if (expenseHead == null)
                {
                    return NotFound();
                }
                return Ok(expenseHead);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
