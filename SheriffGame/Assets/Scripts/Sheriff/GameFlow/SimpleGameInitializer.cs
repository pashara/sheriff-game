using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.States;
using Sheriff.GameFlow.States.ClassicGame.States.Initialize;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck;
using Sheriff.GameFlow.States.ClassicGame.States.Shopping;
using Sheriff.Rules;
using Sheriff.Rules.ClassicRules;
using ThirdParty.Randoms;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow
{
    public class SimpleGameInitializer : MonoBehaviour
    {
        [SerializeField] private BaseRuleConfigSO configProvider;

        private DiContainer _container;
        private ClassicGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(
            DiContainer container, 
            EcsContextProvider ecsContextProvider,
            ClassicGameStateMachine gameStateMachine,
            IRandomService randomService
            )
        {
            _container = container;
            var _randomService = randomService.CreateSubService();
            _gameStateMachine = gameStateMachine;
            Initialize();
        }

        private void Initialize()
        {
            var subContainer = _container.CreateSubContainer();
            var rules = configProvider.Rules<ClassicRuleConfig>();
            subContainer.BindInterfacesAndSelfTo<ClassicRuleConfig>().FromInstance(rules);
            subContainer.BindInterfacesAndSelfTo<ClassicGameController>().AsSingle();
            
            _gameStateMachine.Put(subContainer.Instantiate<ShoppingSubState>());
            _gameStateMachine.Put(subContainer.Instantiate<SetSherifStatusSubState>());
            _gameStateMachine.Put(subContainer.Instantiate<SherifCheckSubState>());
            _gameStateMachine.Put(subContainer.Instantiate<InitializeGameSubState>());



            subContainer.Resolve<ClassicGameController>().StartGame();
        }

    }
}