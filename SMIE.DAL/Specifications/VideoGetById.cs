using SMIE.Core.Data.Specification;

namespace SMIE.DAL.Specifications
{
    sealed class VideoGetById : PgSqlSpecification
    {
        public VideoGetById(int id)
        {
            AddParameter("_id", id);
        }

        protected override string CreateCommandText()
        {
            return "select * from video_getbyid(@_id)";
        }
    }
}
