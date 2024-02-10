namespace Sheriff.GameFlow
{
    public interface IGameCommand
    {
        void Apply();

        void Recalculate();
    }
}