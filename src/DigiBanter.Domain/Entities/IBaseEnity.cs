namespace DigiBanter.Domain.Entities;

public interface IBaseEnity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}
