using System;
using System.Collections;
using System.Collections.Generic;
using Sheriff.GameFlow.States.ClassicGame;
using Zenject;

namespace Sheriff.GameFlow.IterationEnvironments
{
    public sealed class IterationEnvironment : IDisposable
    {
        private DiContainer _container;

        public DiContainer Container => _container;

        public IterationEnvironment(DiContainer container)
        {
            _container = container.CreateSubContainer();
            _container.Bind<IterationEnvironment>().FromInstance(this);
        }

        public T Instantiate<T>()
        {
            return Container.Instantiate<T>();
        }


        public T Command<T>() where T : IGameCommand
        {
            return Container.Instantiate<T>();
        }
        
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public T ResolveId<T>(object identifier)
        {
            return Container.ResolveId<T>(identifier);
        }

        public void Dispose()
        {
            _container.UnbindAll();
            _container = null;
        }

        public List<T> ResolveIdAll<T>(object identifier)
        {
            return Container.ResolveIdAll<T>(identifier);
        }
    }
}