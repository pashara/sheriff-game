using System;
using Zenject;

namespace Sheriff.GameFlow.IterationEnvironments
{
    public class IterationEnvironmentFactory
    {
        private readonly DiContainer _container;

        public IterationEnvironmentFactory(DiContainer container)
        {
            _container = container;
        }
        
        public IterationEnvironment Create(DiContainer container)
        {
            return container.Instantiate<IterationEnvironment>();
        }
        public IterationEnvironment Create(Action<DiContainer> container)
        {
            return _container.Instantiate<IterationEnvironment>(new [] { container });
        }
    }
}