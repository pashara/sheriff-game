using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Sheriff.GameFlow.States.ClassicGame;
using ThirdParty.Randoms;
using Zenject;

namespace Sheriff
{
    public class SimpleMonoBehaviourInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EcsContextProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomService>().FromInstance(new RandomService(0));
            Container.BindInterfacesAndSelfTo<ClassicGameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<TestScript>().AsSingle()
                .WithArguments(new object[]  { new Ee("SimpleMonoBehaviourInstaller") });


            Container.Bind<IterationEnvironmentFactory>().FromSubContainerResolve().ByInstaller<EnvironemntSubContainerInstaller>().AsCached();
            
            
            
            Container.Resolve<TestScript>();
        }
    }
}