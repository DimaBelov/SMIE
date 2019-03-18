using System.Data;
using SMIE.Core.Data.Dapper;

namespace SMIE.Core.Data
{
    public interface ISpecification
    {
        WrappedParameters GetParameters();
        string GetCommandText();
        int Output();
        int? CommandTimeOut { get; }
        CommandType GetCommandType();
    }
}
