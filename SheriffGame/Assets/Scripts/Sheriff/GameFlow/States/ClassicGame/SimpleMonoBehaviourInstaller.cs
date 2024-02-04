using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.IterationEnvironments;
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
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CardsProvider>().FromInstance(_cardsProvider);
            
            Container.BindInterfacesAndSelfTo<EcsContextProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomService>().FromInstance(new RandomService(0));
            Container.BindInterfacesAndSelfTo<ClassicGameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameViewController>().FromInstance(gameViewController);
            Container.BindInterfacesAndSelfTo<CommandsApplyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SheriffCheckHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SherifSelectService>().AsSingle();


            Container.Bind<IterationEnvironmentFactory>().FromSubContainerResolve().ByInstaller<EnvironemntSubContainerInstaller>().AsCached();
        }
    }
}