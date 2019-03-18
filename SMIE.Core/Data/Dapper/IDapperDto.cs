namespace SMIE.Core.Data.Dapper
{
    /// <summary>
    /// Принимает прочитанные даппером из базы значения и создает на их основе сущность
    /// </summary>
    public interface IDapperDto<out T> where T : class
    {
        T GetEntity();
    }
}
