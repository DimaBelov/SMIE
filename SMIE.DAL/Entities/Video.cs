using SMIE.Core.Data.Dapper;

namespace SMIE.DAL.Entities
{
    public class Video
    {
        [DatabaseMap("id")]
        public int Id { get; set; }

        [DatabaseMap("name")]
        public string Name { get; set; }

        [DatabaseMap("link")]
        public string Link { get; set; }

        [DatabaseMap("description")]
        public string Description { get; set; }

        [DatabaseMap("poster_link")]
        public string PosterLink { get; set; }
    }
}
