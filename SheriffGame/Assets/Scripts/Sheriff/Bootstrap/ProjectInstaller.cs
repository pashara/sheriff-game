using Zenject;

namespace Sheriff.Bootstrap
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameSessionDataProvider>().AsSingle();
        }
    }
}
