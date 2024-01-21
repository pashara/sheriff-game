using Sheriff.GameFlow.States.ClassicGame.States;
using Sheriff.GameFlow.States.ClassicGame.States.Initialize;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck;
using Sheriff.GameFlow.States.ClassicGame.States.Shopping;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameController
    {
        private readonly ClassicGameStateMachine _gameStateMachine;

        public ClassicGameController(ClassicGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void StartGame()
        {
            _gameStateMachine.Enter<InitializeGameSubState>();
        }


        public void OnReady<T>() where T : ClassicGameState
        {
            if (typeof(T) == typeof(InitializeGameState))
            {
                _gameStateMachine.Enter<SetSherifStatusSubState>();
                return;
            }
            
            if (typeof(T) == typeof(SetSheriffStatusState))
            {
                _gameStateMachine.Enter<ShoppingSubState>();
                return;
            }
            
            if (typeof(T) == typeof(ShoppingState))
            {
                _gameStateMachine.Enter<SherifCheckSubState>();
                return;
            }
        }
        
    }
}