using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.View;
using ThirdParty.Randoms;
using UnityEngine;
using Zenject;

namespace Sheriff
{
    public class SimpleMonoBehaviourInstaller : MonoInstaller
    {
        [SerializeField] private CardsProvider _cardsProvider;
        [SerializeField] private GameViewController gameViewController;
        [SerializeField] private PlayerSpawnService playerSpawnService;
        [SerializeField] private GameStartEmulateConfig gameStartEmulateConfig;
        public override void InstallBindings()
        {
            var initialEcsData = gameStartEmulateConfig.GetLoadData();

            Container.BindInterfacesTo<GameStartEmulateConfig>().FromInstance(gameStartEmulateConfig).AsSingle();
            Container.BindInterfacesTo<CardsProvider>().FromInstance(_cardsProvider);
            
            Container.BindInterfacesAndSelfTo<RandomService>().FromInstance(new RandomService(0));
            Container.BindInterfacesAndSelfTo<ClassicGameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameViewController>().FromInstance(gameViewController);
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().FromInstance(playerSpawnService);
            Container.BindInterfacesAndSelfTo<LinkWithVisualService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CommandsApplyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SheriffCheckHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SherifSelectService>().AsSingle();
            
            
            if (initialEcsData != null)
            {
                var instance = new EcsContextProvider(initialEcsData);
                Container.Bind<EcsContextProvider>().FromInstance(instance);
            }
            else
            {
                var instance = new EcsContextProvider();
                Container.Bind<EcsContextProvider>().FromInstance(instance);
            }
        }
    }
}