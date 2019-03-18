using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMIE.Core.Data;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;
using SMIE.DAL.Specifications;

namespace SMIE.DAL.Services
{
    static class VideoDb
    {
        public static List<Video> Videos = new List<Video>
        {
            new Video{Id = 1, Name = "Lesson 1", Description = "This is Lesson 1 about ...", Link = "/Videos/8.mp4", PosterLink = "/Videos/video.jpg"},
            new Video{Id = 2, Name = "Lesson 2", Description = "This is Lesson 2 about ...", Link = "/Videos/8.mp4", PosterLink = "/Videos/video.jpg"},
            new Video{Id = 3, Name = "Lesson 3", Description = "This is Lesson 3 about ...", Link = "/Videos/8.mp4", PosterLink = "/Videos/video.jpg"}
        };
    }

    public class CatalogService : AppService, ICatalogService
    {
        static readonly List<Video> _cache = new List<Video>();
        static readonly object _cacheLock = new object();

        public CatalogService(IGenericRepository repository) : base(repository)
        {
        }

        public IEnumerable<Video> GetAll()
        {
            var result = GetAll<Video>(new VideoGetAll());
            Task.Run(() => AddToCache(result));
            return result;
        }

        public Video Get(int id)
        {
            return _cache.Count == 0 ?
                Get<Video>(new VideoGetById(id)) :
                _cache.FirstOrDefault(v => v.Id == id);

            //return Get<Video>(new VideoGetById(id));
        }

        static void AddToCache(IEnumerable<Video> videos)
        {
            lock (_cacheLock)
            {
                _cache.Clear();
                _cache.AddRange(videos);
            }
        }
    }
}
