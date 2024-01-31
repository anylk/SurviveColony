public interface ICollectableItem:IIndicatable
{
    public void CollectItem();
    public bool activeOnWorld { get; }
}