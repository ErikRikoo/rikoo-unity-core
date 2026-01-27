namespace Core.GameplayElementCode.Lifecycle
{
    public interface IUpdatableFlag
    {
        public void OnUpdate();

        public int UpdateOrder => 0;
    }
}