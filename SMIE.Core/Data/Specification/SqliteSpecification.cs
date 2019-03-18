using System.Data;

namespace SMIE.Core.Data.Specification
{
    public abstract class SqliteSpecification : SpecificationBase
    {
        public override CommandType GetCommandType()
        {
            return CommandType.Text;
        }
    }
}
