using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
                var expense = await _repository.CreateAsync(expenseHead);
                if (expense.Success == false)
                {
                    return NotFound(expense);
                }
                return Ok(expense);
            }
            catch (Exception ex)
            {
                ApiResponse<ExpenseInfoDto> apiRes = new();
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return BadRequest(apiRes);
            }
        }


        [HttpGet("{id:int}/{branchId}/{companyId}")]
        public async Task<ActionResult<ApiResponse<ExpenseInfoDto>>> GetExpenseInfoByIdAsync(int id, string branchId, string companyId)
        {
            try
            {
                var expense = await _repository.GetExpenseInfoById(id, branchId, companyId);
                if (expense.Success == false)
                {
                    return NotFound(expense);
                }
                return Ok(expense);
            }
            catch (Exception ex)
            {
                ApiResponse<ExpenseInfoDto> apiRes = new();
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return BadRequest(apiRes);
            }
        }


        [HttpGet("{branchId}/{companyId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpenseInfoDto>>>> GetAllExpenseInfoAsync(string branchId, string companyId)
        {
            try
            {
                var expense = await _repository.GetAllExpenseInfoAsync(branchId, companyId);
                if (expense.Success == false)
                {
                    return NotFound(expense);
                }
                return Ok(expense);
            }
            catch (Exception ex)
            {
                ApiResponse<IEnumerable<ExpenseInfoDto>> apiRes = new();
                apiRes.Success = false;
                apiRes.Message = ex.Message;

                return BadRequest(apiRes);
            }
        }


        [HttpGet("GetExpensesDateWiseAsync/{strSearch}/{fromDate:Datetime}/{toDate:Datetime}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpenseInfoDto>>>> GetExpensesDateWiseAsync(string strSearch, DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (strSearch == "-")
                {
                    strSearch = "";
                }
                var expense = await _repository.GetExpensesInDateRangeAsync(strSearch,fromDate,toDate);
                if (expense.Success == false)
                {
                    return NotFound(expense);
                }
                return Ok(expense);
            }
            catch (Exception ex)
            {
                ApiResponse<IEnumerable<ExpenseInfoDto>> apiRes = new();
                apiRes.Success = false;
                apiRes.Message = ex.Message;
                
                return BadRequest(apiRes);
            }
        }


    }
}
