using Zenject;

namespace Sheriff.GameFlow.IterationEnvironments
{
    public class EnvironemntSubContainerInstaller : Installer<EnvironemntSubContainerInstaller>
    {
        public bool IsEnabled => true;

        public override void InstallBindings()
        {
            Container.Bind<IterationEnvironmentFactory>().AsCached();
        }
    }
}