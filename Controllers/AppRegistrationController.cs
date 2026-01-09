using Microsoft.AspNetCore.Mvc;

namespace GravyFoodsApi.Controllers
{
    public class AppRegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
