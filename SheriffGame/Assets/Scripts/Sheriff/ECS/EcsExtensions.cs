using System;
using System.Collections.Generic;
using Entitas;
using UniRx;

namespace Sheriff.ECS
{
    public static class EcsExtensions
    {
        private static readonly Dictionary<IComponent, object> _cache = new();
        private static readonly Dictionary<Type, Dictionary<int, object>> _cacheGeneric = new();

        public static IReadOnlyReactiveProperty<T> OnChange<T>(this IEntity entity, int componentId)
            where T : IComponent
        {
            if (!_cacheGeneric.TryGetValue(entity.GetType(), out var elementsCache))
            {
                var dictionary = new Dictionary<int, object>();
                _cacheGeneric[entity.GetType()] = dictionary;
                elementsCache = dictionary;
            }

            if (elementsCache.TryGetValue(componentId, out var property))
            {
                return property as IReadOnlyReactiveProperty<T>;
            }
            ReactiveProperty<T> value = new();
            elementsCache[componentId] = value;
            property = value;
            
            
            entity.OnComponentAdded += EntityOnOnComponentAdded;
            entity.OnComponentRemoved += EntityOnOnComponentRemoved;
            entity.OnComponentReplaced += EntityOnOnComponentReplaced;
            entity.OnDestroyEntity += EntityOnOnDestroyEntity;

            if (entity.HasComponent(componentId))
            {
                value.Value = (T)entity.GetComponent(componentId);
            }
            else
            {
                value.Value = default;
            }

            return value;

            void EntityOnOnDestroyEntity(IEntity entity1)
            {
                value.Dispose();
                elementsCache.Remove(componentId);
                
                entity.OnComponentAdded -= EntityOnOnComponentAdded;
                entity.OnComponentRemoved -= EntityOnOnComponentRemoved;
                entity.OnComponentReplaced -= EntityOnOnComponentReplaced;
                entity.OnDestroyEntity -= EntityOnOnDestroyEntity;
            }

            void EntityOnOnComponentReplaced(IEntity entity1, int index, IComponent previouscomponent, IComponent newcomponent)
            {
                value.Value = (T) newcomponent;
            }

            void EntityOnOnComponentAdded(IEntity entity1, int index, IComponent component1)
            {
                value.Value = (T) component1;
            }
            
            void EntityOnOnComponentRemoved(IEntity entity, int index, IComponent component)
            {
                value.Value = default;
            }
        }

        // public static IReadOnlyReactiveProperty<T> OnChange<T, K>(this K entity, T component) 
        //     where K : IEntity 
        //     where T : IComponent
        // {
        //     
        //     if (_cache.TryGetValue(component, out var property))
        //     {
        //         return property as IReadOnlyReactiveProperty<T>;
        //     }
        //     
        //     ReactiveProperty<T> value = new();
        //     _cache[component] = value;
        //     
        //     entity.OnComponentAdded += EntityOnOnComponentAdded;
        //     entity.OnComponentRemoved += EntityOnOnComponentRemoved;
        //     entity.OnComponentReplaced += EntityOnOnComponentReplaced;
        //     entity.OnDestroyEntity += EntityOnOnDestroyEntity;
        //
        //     return value;
        //
        //     void EntityOnOnDestroyEntity(IEntity entity1)
        //     {
        //         value.Dispose();
        //         _cache.Remove(component);
        //         
        //         entity.OnComponentAdded -= EntityOnOnComponentAdded;
        //         entity.OnComponentRemoved -= EntityOnOnComponentRemoved;
        //         entity.OnComponentReplaced -= EntityOnOnComponentReplaced;
        //         entity.OnDestroyEntity -= EntityOnOnDestroyEntity;
        //     }
        //
        //     void EntityOnOnComponentReplaced(IEntity entity1, int index, IComponent previouscomponent, IComponent newcomponent)
        //     {
        //         value.Value = (T) newcomponent;
        //     }
        //
        //     void EntityOnOnComponentAdded(IEntity entity1, int index, IComponent component1)
        //     {
        //         value.Value = (T) component1;
        //     }
        //     
        //     void EntityOnOnComponentRemoved(IEntity entity, int index, IComponent component)
        //     {
        //         value.Value = default;
        //     }
        // }
    }
}