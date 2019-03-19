using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;

namespace SMIE.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        readonly ICatalogService _catalogService;

        public VideoController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IActionResult Watch(Video video)
        {
            return View();
        }

        [Route("Video/{id}")]
        public IActionResult Watch(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            return View(_catalogService.Get(id.Value));
        }
    }
}
