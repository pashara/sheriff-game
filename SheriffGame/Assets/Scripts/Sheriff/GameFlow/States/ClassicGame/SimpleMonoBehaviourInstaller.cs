using Sheriff.ECS;
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
        [SerializeField] private GameViewController gameViewController;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EcsContextProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomService>().FromInstance(new RandomService(0));
            Container.BindInterfacesAndSelfTo<ClassicGameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameViewController>().FromInstance(gameViewController);
            Container.BindInterfacesAndSelfTo<CommandsApplyService>().AsSingle();


            Container.Bind<IterationEnvironmentFactory>().FromSubContainerResolve().ByInstaller<EnvironemntSubContainerInstaller>().AsCached();
        }
    }
}