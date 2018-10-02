namespace Byndyusoft.Extensions.Specifications.Sql
{
    public interface ISqlSpecification : ISpecification
    {
        string ToSql();
        dynamic Parameters { get; }
    }

    public interface ISqlSpecification<T> : ISqlSpecification, ISpecification<T>
    {
    }
}