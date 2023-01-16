namespace CarApi2.Domain.Common
{
    public interface IAgrregatesRoot<out TKey>
    {
        TKey Id { get; }
    }
}
