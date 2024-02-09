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


        public void FillData(ContextSerializeData serializeData)
        {
            {
                foreach (var context in Context.allContexts)
                {
                    context.DestroyAllEntities();
                }

                foreach (var context in Context.allContexts)
                    context.OnEntityCreated -= GameOnOnEntityCreated;
            }


            _internalId = serializeData.LastGeneratedId;

            Dictionary<Type, (List<Type>, Func<Entity>, IContext)> elements = new()
            {
                { typeof(CardContext), (CardComponentsLookup.componentTypes.ToList(), Context.card.CreateEntity, Context.card) },
                { typeof(GameContext), (GameComponentsLookup.componentTypes.ToList(), Context.game.CreateEntity, Context.game) },
                { typeof(InputContext), (InputComponentsLookup.componentTypes.ToList(), Context.input.CreateEntity, Context.input) },
                { typeof(PlayerContext), (PlayerComponentsLookup.componentTypes.ToList(), Context.player.CreateEntity, Context.player) },
            };
            
            foreach (var s in serializeData.SerializeData)
            {
                if (elements.TryGetValue(s.Key, out var d))
                {
                    foreach (var data in s.Value)
                    {
                        IEntity entity = null;
                        if (data.hasId && d.Item3 is IIdContext idContext)
                        {
                            entity = idContext.GetEntityWithId(data.id);
                        }


                        if (entity != null)
                        {
                            entity.RemoveAllComponents();
                        }
                        else
                        {
                            entity = d.Item2.Invoke();
                        }
                        
                        foreach (var component in data.Components)
                        {
                            var entityIndex = d.Item1.IndexOf(component.GetType());
                            if (entityIndex >= 0)
                            {
                                entity.ReplaceComponent(entityIndex, component);
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