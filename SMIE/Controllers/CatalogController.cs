using System.Threading.Tasks;
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
            return ViewComponent("Catalog");
        }
    }
}
