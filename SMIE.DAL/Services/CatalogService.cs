using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;

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

    public class CatalogService : ICatalogService
    {
        public IEnumerable<Video> GetAll()
        {
            return VideoDb.Videos;
        }

        public Video Get(int id)
        {   //TODO брать из кэша
            return VideoDb.Videos.FirstOrDefault(v => v.Id == id);
        }
    }
}
