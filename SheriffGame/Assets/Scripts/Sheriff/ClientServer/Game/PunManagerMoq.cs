using Sheriff.GameFlow.States.ClassicGame;
using UniRx;
using Zenject;

namespace Sheriff.ClientServer.Game
{
    public class PunManagerMoq : IPunSender, IInitializable
    {
        private ClassicGameControllerWrapper _gameControllerWrapper;
        private ReactiveCollection<string> _receivedFromMasterCommands = new();
        private ReactiveCollection<string> _receivedFromSlaveCommands = new();

        public PunManagerMoq(ClassicGameControllerWrapper gameControllerWrapper)
        {
            _gameControllerWrapper = gameControllerWrapper;
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
            _gameControllerWrapper.StartGame(3);
        }
    }
}