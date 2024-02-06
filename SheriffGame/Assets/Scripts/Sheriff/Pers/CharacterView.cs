using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.Pers
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private List<CharacterViewIdProvider> elements;


        [Button]
        public void Apply(string key)
        {
            var element = elements.FirstOrDefault(x => x.Key.Equals(key));
            if (element == null) return;
            foreach (var e in elements)
            {
                e.gameObject.SetActive((e == element));
            }
        }
    }
}
