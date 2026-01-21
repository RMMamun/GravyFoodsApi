using GravyFoodsApi.DTO;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class POSSubscriptionController : Controller
    {
        private readonly IPOSSubscription _posSubs;
        private readonly ILoggingService _logService;
        public POSSubscriptionController(IPOSSubscription posSubs, ILoggingService logService)
        {
            _posSubs = posSubs;
            _logService = logService;
        }

        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateSubscription")]
        public async Task<ActionResult<POSSubscription>> CreateSubscription(POSSubscription subscription)
        {
            try
            {

                if (subscription == null)
                {
                    return NotFound();
                }

                SubscriptionDto dto = new SubscriptionDto();
                dto.DeviceKey = subscription.DeviceKey;
                dto.SubscriptionEndDate = subscription.SubscriptionEndDate;

                var check = await _posSubs.isExisted(dto);
                if (check == true)
                {
                    return NotFound();
                }
                else
                {
                    var user = await _posSubs.Create(subscription);
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateSubscription")]
        public async Task<ActionResult<bool>> UpdateSubscription(POSSubscription subscription)
        {
            try
            {

                if (subscription == null)
                {
                    return NotFound();
                }

                SubscriptionDto dto = new SubscriptionDto();
                dto.DeviceKey = subscription.DeviceKey;
                dto.SubscriptionEndDate = subscription.SubscriptionEndDate;

                var check = await _posSubs.isExisted(dto);
                if (check == true)
                {
                    return NotFound();
                }
                else
                {
                    var user = await _posSubs.UpdateSubscriptionAsync(subscription);
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST: api/UserInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CheckSubscription")]
        public async Task<ActionResult<int>> CheckSubscription(SubscriptionDto subscription)
        {
            try
            {
                if (subscription == null)
                {
                    return NotFound();
                }

                SubscriptionDto dto = new SubscriptionDto();
                dto.DeviceKey = subscription.DeviceKey;
                dto.SubscriptionEndDate = subscription.SubscriptionEndDate;

                var check = await _posSubs.isExisted(dto);
                if (check == false)
                {
                    return NotFound();
                }
                else
                {
                    //Logging log = new Logging();
                    //log.EntryDateTime = DateTime.Now;
                    //log.SourceName = "POS: Loging Request";
                    //log.LogDescription = sub.DeviceKey + " " + sub.SubscriptionEndDate;

                    //var logstatus = await _logService.Create(log);

                    var days = await _posSubs.CheckSubscription(subscription);
                    return days;
                }



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<POSSubscription>>> GetAllAsync()
        {
            try
            {
                var sub = await _posSubs.GetAllAsync();

                if (sub == null || !sub.Any()) // Optionally check if the collection is empty
                {
                    return NotFound(); // Returns a 404 Not Found response
                }

                return Ok(sub); // Wrap the result in Ok() to return a 200 OK response
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Returns a 400 Bad Request response with the error message
            }
        }

        
        [HttpGet("CompanyRegistrationVerification/{regCode}")]
        public async Task<ActionResult<ApiResponse<CompanyRegistrationResponseDto?>>> GetCompanyRegistrationVerification(string regCode)
        {
            //public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductByIdAsync(string productId, string branchId, string companyId)

            var result = await _posSubs.GetCompanyRegistrationVerificationAsync(regCode);
            if (result.Success == false)
                return NotFound(result);

            return Ok(result);
        }






    }
}
