using UnityEngine;

namespace Sheriff.Pers
{
    public class CharacterViewIdProvider : MonoBehaviour
    {
        [SerializeField] private string key;
        public string Key => key;
    }
}