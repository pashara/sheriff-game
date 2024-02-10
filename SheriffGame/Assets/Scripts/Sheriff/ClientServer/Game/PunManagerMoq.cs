using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.States.ClassicGame;
using UniRx;
using Zenject;

namespace Sheriff.ClientServer.Game
{
    public class PunManagerMoq : IPunSender, IInitializable
    {
        private ClassicGameControllerWrapper _gameControllerWrapper;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly ISessionInitializeDataProvider _sessionInitializeDataProvider;
        private ReactiveCollection<string> _receivedFromMasterCommands = new();
        private ReactiveCollection<string> _receivedFromSlaveCommands = new();

        public PunManagerMoq(
            ClassicGameControllerWrapper gameControllerWrapper,
            EcsContextProvider ecsContextProvider,
            ISessionInitializeDataProvider sessionInitializeDataProvider
            )
        {
            _gameControllerWrapper = gameControllerWrapper;
            _ecsContextProvider = ecsContextProvider;
            _sessionInitializeDataProvider = sessionInitializeDataProvider;
        }

        public IReadOnlyReactiveCollection<string> ReceivedFromMasterCommands => _receivedFromMasterCommands;
        public IReadOnlyReactiveCollection<string> ReceivedFromSlaveCommands => _receivedFromSlaveCommands;
        
        
        public void SendCommandToMaster(string json)
        {
            
        }
        public void NotifyCommand(string serialize)
        {
        }

        public void SendInitialGameState()
        {
        }

        public void SendGameState()
        {
        }

        public void IncView()
        {
        }

        public void DecView()
        {
        }

        public void Initialize()
        {
            var data = _sessionInitializeDataProvider.GetLoadData();
            if (data == null)
            {
                _gameControllerWrapper.StartGame(3);
            }
            else
            {
                _ecsContextProvider.FillData(data);
                _gameControllerWrapper.StartGame(data);
            }

        }
    }
}