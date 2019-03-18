using SMIE.Core.Data.Specification;

namespace SMIE.DAL.Specifications
{
    sealed class VideoGetAll : PgSqlSpecification
    {
        protected override string CreateCommandText()
        {
            return "select * from video_getall()";
        }
    }
}
