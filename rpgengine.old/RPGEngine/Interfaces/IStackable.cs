namespace RPGEngine
{
    public interface IStackable : ICatalogable
    {
        ushort Quantity { get; }
    }
}
