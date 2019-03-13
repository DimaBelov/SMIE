using System;
using Microsoft.AspNetCore.Mvc;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;

namespace SMIE.Controllers
{
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
            if(!id.HasValue)
                throw new Exception("Video not found"); //TODO redirect to NotFound

            return View(_catalogService.Get(id.Value));
        }
    }
}
