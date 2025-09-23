using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseHeadController : Controller
    {
        private readonly IExpenseHeadService _repository;

        public ExpenseHeadController(IExpenseHeadService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseHead>> Create([FromBody] ExpenseHeadDto expenseHead)
        {
            var created = await _repository.CreateAsync(expenseHead);
            return Ok(created);
            //return CreatedAtAction(nameof(GetExpenseHeadById), new { id = created.Data.Id, branchId = created.Data.BranchId, companyId = created.Data.CompanyId }, created.Data);
        }

        [HttpGet("{id:int}/{branchId}/{companyId}")]
        public async Task<ActionResult<ExpenseHead>> GetExpenseHeadByIdAsync(int id, string branchId, string companyId)
        {
            
            var expenseHead = await _repository.GetExpenseHeadById(id, branchId, companyId);
            if (expenseHead == null)
            {
                return NotFound();
            }
            return Ok(expenseHead);

        }

        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<ExpenseHead>>> GetAllExpenseHeadAsync(string branchId, string companyId)
        {

            var expenseHead = await _repository.GetAllExpenseHeadAsync(branchId, companyId);
            if (expenseHead == null)
            {
                return NotFound();
            }
            return Ok(expenseHead);

        }

    }
}
