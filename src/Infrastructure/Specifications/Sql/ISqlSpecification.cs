namespace Byndyusoft.Extensions.Specifications.Sql
{
    public interface ISqlSpecification<T>
    {
        string ToSql();
        dynamic Parameters { get; }
    }
}