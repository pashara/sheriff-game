using System;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.States;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck;
using Sheriff.GameFlow.States.ClassicGame.States.Shopping;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameController
    {
        private readonly ClassicGameStateMachine _gameStateMachine;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly LinkWithVisualService _linkWithVisualService;

        public ClassicGameController(
            ClassicGameStateMachine gameStateMachine,
            EcsContextProvider ecsContextProvider,
            LinkWithVisualService linkWithVisualService
            )
        {
            _gameStateMachine = gameStateMachine;
            _ecsContextProvider = ecsContextProvider;
            _linkWithVisualService = linkWithVisualService;
        }
        
        public void StartGame()
        {
            _gameStateMachine.Enter<InitializeGameState>();
        }

        public void StartGame(Type loadDataStateType)
        {
            _linkWithVisualService.Link();
            
            var actualStateProvider = new ActualStateProviderProvider();
            _ecsContextProvider.Context.game.gameIdEntity.ReplaceActualStateProviderWritable(actualStateProvider);
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                playerEntity.ReplaceActualStateProvider(actualStateProvider);
            }
            
            _gameStateMachine.Enter(loadDataStateType);
        }

        public void OnReady<T>() where T : ClassicGameState
        {
            OnReady(typeof(T));
        }

        public void OnReady(Type type)
        {
            if (type == typeof(InitializeGameState))
            {
                _gameStateMachine.Enter<SetSheriffStatusState>();
                return;
            }
            
            if (type == typeof(SetSheriffStatusState))
            {
                _gameStateMachine.Enter<ShoppingState>();
                return;
            }
            
            if (type == typeof(ShoppingState))
            {
                _gameStateMachine.Enter<SheriffCheckState>();
                return;
            }
            
            if (type == typeof(SheriffCheckState))
            {
                _gameStateMachine.Enter<SetSheriffStatusState>();
                return;
            }
        }
    }
}