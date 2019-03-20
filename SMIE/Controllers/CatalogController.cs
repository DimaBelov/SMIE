using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SMIE.Core.Web.Attributes;
using SMIE.DAL.Interfaces;
using SMIE.Models;

namespace SMIE.Controllers
{
    [ApiExceptionFilter]
    public class CatalogController : Controller
    {
        readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IActionResult Get()
        {
            //return ViewComponent("Catalog");
            return PartialView("_CatalogPartial", new CatalogModel
            {
                Videos = _catalogService.GetAll().ToList()
            });
        }

        public IActionResult Test()
        {
            throw new Exception("Test exception");
        }
    }
}
