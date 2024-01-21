using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.Rules
{
    public class BaseRuleConfigSO : SerializedScriptableObject
    {
        [SerializeField] private BaseRuleConfig config;


        public T Rules<T>() where T : BaseRuleConfig
        {
            return config as T;
        }
    }
}