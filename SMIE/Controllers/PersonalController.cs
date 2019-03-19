using Microsoft.AspNetCore.Mvc;

namespace SMIE.Controllers
{
    public class PersonalController : Controller
    {
        public IActionResult Personal()
        {
            return View("Personal");
        }
    }
}
