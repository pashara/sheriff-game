using System;
using Entitas;
using Sheriff.GameFlow;
using Zenject;

namespace Sheriff.ECS
{
    public class EcsContextProvider : IInitializable, IDisposable
    {
        public Contexts Context { get; private set; }


        private long _internalId;
        public long LastGeneratedId => _internalId;
        
        public EcsContextProvider()
        {
            Context = new();
            _internalId = 0;

            foreach (var context in Context.allContexts)
                context.OnEntityCreated += GameOnOnEntityCreated;
            
        }
        
        public void Initialize()
        {
        }

        public void Dispose()
        {
            foreach (var context in Context.allContexts)
                context.OnEntityCreated -= GameOnOnEntityCreated;
        }
        

        private void GameOnOnEntityCreated(IContext context, IEntity entity)
        {
            if (entity is IIdEntity idEntity)
            {
                idEntity.AddId(_internalId);
                _internalId++;
            }
        }
    }
}