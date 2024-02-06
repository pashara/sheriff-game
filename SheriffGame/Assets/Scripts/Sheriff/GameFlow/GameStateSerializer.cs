using System;
using System.Collections.Generic;
using Entitas;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow
{
    public class GameStateSerializer : SerializedMonoBehaviour
    {
        
        [Inject] private EcsContextProvider _ecsContextProvider;

        [SerializeField] private ContextSerializeData _data;
        [SerializeField] private string json;
        
        [Button]
        private void Serialize()
        {
            _data = new();

            _data.LastGeneratedId = _ecsContextProvider.LastGeneratedId;
            _data.SerializeData = new();
            
            var contexts = new List<(Type, Func<Entity[]>)>()
            {
                (_ecsContextProvider.Context.player.GetType(), () => _ecsContextProvider.Context.player.GetEntities()),
                (_ecsContextProvider.Context.game.GetType(), () => _ecsContextProvider.Context.game.GetEntities()),
                (_ecsContextProvider.Context.card.GetType(), () => _ecsContextProvider.Context.card.GetEntities()),
                (_ecsContextProvider.Context.input.GetType(), () => _ecsContextProvider.Context.input.GetEntities())
            };

            foreach (var context in contexts)
            {
                var entities = context.Item2.Invoke();
                var type = context.Item1;

                var collection = new List<EntitySerializeData>();
                _data.SerializeData.Add(type, collection);
                
                
                foreach (var entity in entities)
                {
                    var entitySerializeData = new EntitySerializeData();
                    entitySerializeData.Components = new();
                
                    foreach (var component in entity.GetComponents())
                    {
                        var hasButtonAttribute = Attribute.IsDefined(component.GetType(), typeof(ECSSerializeAttribute));
                        if (hasButtonAttribute)
                        {
                            entitySerializeData.Components.Add(component);
                        }
                    }

                    if (entitySerializeData.Components.Count != 0)
                    {
                        collection.Add(entitySerializeData);
                    }
                }
            }
            
            json = JsonConvert.SerializeObject(_data, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
    
    [Serializable]
    public class EntitySerializeData
    {
        [JsonProperty("components")] public List<IComponent> Components = new();
    }

    [Serializable]
    public class ContextSerializeData
    {
        [JsonProperty("last_id")] public long LastGeneratedId;

        [JsonProperty("data")] public Dictionary<Type, List<EntitySerializeData>> SerializeData = new();
    }
}
