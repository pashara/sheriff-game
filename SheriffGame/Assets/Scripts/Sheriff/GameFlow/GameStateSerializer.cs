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



        public string Serialize()
        {
            _data = new();
            _data.LastGeneratedId = _ecsContextProvider.LastGeneratedId;
            _data.StateType = _ecsContextProvider.Context.game.gameIdEntity.actualStateProviderWritable.Value?.ActualState?.Value?.GetType();
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
                    if (entity is IIdEntity idEntity && idEntity.hasId)
                    {
                        entitySerializeData.hasId = true;
                        entitySerializeData.id = idEntity.id.ID;
                    }
                    else
                    {
                        entitySerializeData.hasId = false;
                        entitySerializeData.id = -1;
                    }
                
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
            
            return JsonConvert.SerializeObject(_data, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }
        
        [Button]
        private void _Serialize()
        {
            json = Serialize();
        }


        [Button]
        ContextSerializeData DeSerialize()
        {
            
            return JsonConvert.DeserializeObject<ContextSerializeData>(json, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
    
    [Serializable]
    public class EntitySerializeData
    {
        [JsonProperty("has_id")] public bool hasId;
        [JsonProperty("id")] public long id;
        
        [JsonProperty("components")] public List<IComponent> Components = new();
    }

    public class ContextSerializeData
    {
        [JsonProperty("last_id")] public long LastGeneratedId;

        [JsonProperty("state")] public Type StateType;

        [JsonProperty("data")] public Dictionary<Type, List<EntitySerializeData>> SerializeData = new();
    }
}
