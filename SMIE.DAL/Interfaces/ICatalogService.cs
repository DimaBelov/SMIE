using System.Collections.Generic;
using System.Threading.Tasks;
using SMIE.DAL.Entities;

namespace SMIE.DAL.Interfaces
{
    public interface ICatalogService
    {
        IEnumerable<Video> GetAll();
    }
}
