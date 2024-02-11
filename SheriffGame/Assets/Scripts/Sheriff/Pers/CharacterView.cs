using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.Pers
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private List<CharacterViewIdProvider> elements;
        [SerializeField] private CharacterViewIdProvider sheriff;
        [SerializeField] CharacterViewIdProvider @default;
        
        private string _lastAppliedKey = string.Empty;
        public void MakeSheriff(bool isSheriff)
        {
            if (isSheriff)
            {
                foreach (var element in elements)
                    element.gameObject.SetActive(false);
                sheriff.gameObject.SetActive(true);
            }
            else
            {
                sheriff.gameObject.SetActive(false);
                Apply(_lastAppliedKey);
            }
        }

        [Button]
        public void Apply(string key)
        {
            var element = elements.FirstOrDefault(x => x.Key.Equals(key));
            if (element == null)
            {
                foreach (var e in elements)
                    e.gameObject.SetActive(false);
                @default.gameObject.SetActive(true);
                
                return;
            }

            _lastAppliedKey = element.Key;
            
            foreach (var e in elements)
            {
                e.gameObject.SetActive((e == element));
            }
        }

        [Button]
        public void Apply(int id)
        {
            var element = elements.ElementAt(id % elements.Count);
            if (element == null)
            {
                foreach (var e in elements)
                    e.gameObject.SetActive(false);
                @default.gameObject.SetActive(true);
                
                return;
            }

            _lastAppliedKey = element.Key;
            
            foreach (var e in elements)
            {
                e.gameObject.SetActive((e == element));
            }
        }
    }
}
