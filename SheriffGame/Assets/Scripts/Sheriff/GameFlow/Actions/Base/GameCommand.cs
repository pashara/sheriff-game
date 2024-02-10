namespace Sheriff.GameFlow
{
    public abstract class GameCommand<T1,T2> : IGameCommand where T1 : ActionParam where T2 : IGameCommand
    {
        public abstract void Apply();
        public abstract T2 Calculate(T1 param);

        protected abstract T1 AppliedParams { get; }
        
        public T2 Recalculate()
        {
            if (AppliedParams != null)
                return Calculate(AppliedParams);
            
            return default;
        }

        void IGameCommand.Recalculate() => Recalculate();
    }
}