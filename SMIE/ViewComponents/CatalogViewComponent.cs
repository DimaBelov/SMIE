using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using SMIE.Core.Web.Attributes;
using SMIE.DAL.Interfaces;
using SMIE.Models;

namespace SMIE.ViewComponents
{
    [ApiExceptionFilter]
    public class CatalogViewComponent : ViewComponent
    {
        readonly ICatalogService _catalogService;

        public CatalogViewComponent(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public IViewComponentResult Invoke(int numberOfItems)
        {
            return View(new CatalogModel
            {
                Videos = _catalogService.GetAll().ToList()
            });
        }
    }
}
