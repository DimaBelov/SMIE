using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SMIE.DAL.Interfaces;

namespace SMIE.Controllers
{
    public class CatalogController : Controller
    {
        readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IActionResult Get()
        {
#if DEBUG
            Thread.Sleep(3000);
#endif
            return ViewComponent("Catalog");
        }
    }
}
