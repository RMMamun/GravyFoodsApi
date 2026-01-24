using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public async Task<ActionResult<ApiResponse<bool>>> Create([FromBody] ExpenseHeadDto expenseHead)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();
            try
            {
                var created = await _repository.CreateAsync(expenseHead);
                return Ok(created);
                //return CreatedAtAction(nameof(GetExpenseHeadById), new { id = created.Data.Id, branchId = created.Data.BranchId, companyId = created.Data.CompanyId }, created.Data);
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return BadRequest(apiRes);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] ExpenseHeadDto expenseHead)
        {
            ApiResponse<bool> apiRes = new ApiResponse<bool>();
            try
            {
                var created = await _repository.UpdateExpenseHeadAsync(expenseHead);
                return Ok(created);
                //return CreatedAtAction(nameof(GetExpenseHeadById), new { id = created.Data.Id, branchId = created.Data.BranchId, companyId = created.Data.CompanyId }, created.Data);
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return BadRequest(apiRes);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<ExpenseHeadDto>>> GetExpenseHeadByIdAsync(int id)
        {
            ApiResponse<ExpenseHeadDto> apiRes = new ApiResponse<ExpenseHeadDto>();

            try
            {
                var expenseHead = await _repository.GetExpenseHeadById(id);
                
                return Ok(expenseHead);
            }
            catch (Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return BadRequest(apiRes);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpenseHeadDto>>>> GetAllExpenseHeadAsync()
        {
            ApiResponse<IEnumerable<ExpenseHeadDto>> apiRes = new ApiResponse<IEnumerable<ExpenseHeadDto>>();

            try
            {
                var expenseHead = await _repository.GetAllExpenseHeadAsync();
                return Ok(expenseHead);
            }
            catch(Exception ex)
            {
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                return BadRequest(apiRes);
            }
        }

    }
}
