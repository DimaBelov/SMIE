using System.Collections.Generic;
using SMIE.DAL.Entities;

namespace SMIE.DAL.Interfaces
{
    public interface ICatalogService
    {
        IEnumerable<Video> GetAll();

        Video Get(int id);
    }
}
