using System;
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


        public void OnReady<T>() where T : IClassicSubState
        {
            OnReady(typeof(T));
        }

        public void OnReady(Type type)
        {
            if (type == typeof(InitializeGameSubState))
            {
                _gameStateMachine.Enter<SetSherifStatusSubState>();
                return;
            }
            
            if (type == typeof(SetSherifStatusSubState))
            {
                _gameStateMachine.Enter<ShoppingSubState>();
                return;
            }
            
            if (type == typeof(ShoppingSubState))
            {
                _gameStateMachine.Enter<SherifCheckSubState>();
                return;
            }
            
            if (type == typeof(SherifCheckSubState))
            {
                _gameStateMachine.Enter<SetSherifStatusSubState>();
                return;
            }
        }
        
    }
}