using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Sheriff.GameFlow;
using UnityEngine;
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

            SubscribeCreate();
        }
        
        public EcsContextProvider(ContextSerializeData serializeData)
        {
            Context = new();
            _internalId = serializeData.LastGeneratedId;

            Dictionary<Type, (List<Type>, Func<Entity>)> elements = new()
            {
                { typeof(CardContext), (CardComponentsLookup.componentTypes.ToList(), Context.card.CreateEntity) },
                { typeof(GameContext), (GameComponentsLookup.componentTypes.ToList(), Context.game.CreateEntity) },
                { typeof(InputContext), (InputComponentsLookup.componentTypes.ToList(), Context.input.CreateEntity) },
                { typeof(PlayerContext), (PlayerComponentsLookup.componentTypes.ToList(), Context.player.CreateEntity) },
            };
            
            foreach (var s in serializeData.SerializeData)
            {
                if (elements.TryGetValue(s.Key, out var d))
                {
                    foreach (var data in s.Value)
                    {
                        var entity = d.Item2.Invoke();
                        foreach (var component in data.Components)
                        {
                            var entityIndex = d.Item1.IndexOf(component.GetType());
                            if (entityIndex >= 0)
                            {
                                entity.AddComponent(entityIndex, component);
                            }
                            else
                            {
                                Debug.LogError($"Cant set object of type {component.GetType()}");
                            }
                        }
                    }
                }
            }

            SubscribeCreate();
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

        private void SubscribeCreate()
        {
            foreach (var context in Context.allContexts)
                context.OnEntityCreated += GameOnOnEntityCreated;
        }

    }
}