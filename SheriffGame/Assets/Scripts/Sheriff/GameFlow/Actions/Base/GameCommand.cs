namespace Sheriff.GameFlow
{
    public abstract class GameCommand<T1,T2> : IGameCommand where T1 : ActionParam where T2 : IGameCommand
    {
        public abstract void Apply();
        public abstract T2 Calculate(T1 param);
    }
}