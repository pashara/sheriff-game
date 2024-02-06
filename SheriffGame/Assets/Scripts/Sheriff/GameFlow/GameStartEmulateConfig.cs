using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Sheriff.GameFlow
{

    public interface ISessionInitializeDataProvider
    {
        string Data { get; }

        ContextSerializeData GetLoadData();
    }
    
    public class GameStartEmulateConfig : MonoBehaviour, ISessionInitializeDataProvider
    {
        [SerializeField] private string initialJson;
        public string Data => initialJson;
        
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
