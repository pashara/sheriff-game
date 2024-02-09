using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Sheriff.GameFlow
{

    public interface ISessionInitializeDataProvider
    {

        ContextSerializeData GetLoadData();
    }

    public class LoadedSessionDataProvider : ISessionInitializeDataProvider
    {
        private string Data { get; }

        public LoadedSessionDataProvider(string json)
        {
            Data = json;
        }
        
        public ContextSerializeData GetLoadData()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<ContextSerializeData>(Data, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
    
    public class GameStartEmulateConfig : MonoBehaviour, ISessionInitializeDataProvider
    {
        [SerializeField] private string initialJson;
        
        public ContextSerializeData GetLoadData()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<ContextSerializeData>(initialJson, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
