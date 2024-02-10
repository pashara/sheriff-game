namespace Sheriff.Bootstrap
{
    public interface IGameSessionDataProvider
    {
        bool IsNetwork { get; }

        void Set(bool isNetwork);
    }
    
    public class GameSessionDataProvider : IGameSessionDataProvider
    {
        public bool IsNetwork { get; private set; }
        public void Set(bool isNetwork)
        {
            IsNetwork = isNetwork;
        }
    }
}