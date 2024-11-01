using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using MasjidApi.Models;
using MasjidApi.Data;
using MasjidApi.MasjidRepository;
using MasjidApi.DTO;

namespace MasjidApi.Controllers
{
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("[controller]")]
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
                    return user;
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
                    //log.LogDescription = subscription.DeviceKey + " " + subscription.SubscriptionEndDate;

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


    }
}
